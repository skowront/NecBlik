using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Models;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.ViewModels;
using NecBlik.Core.GUI.Factories;

namespace NecBlik.Views.Controls
{
    public class DeviceDesignerCanvas:DesignerCanvas
    {
        private const int BackgroundZIndex = -1;

        private FrameworkElement background;

        public DeviceDesignerCanvas() : base()
        {
            this.Focusable = true;
            this.IsHitTestVisible = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            //base.OnDrop(e);
            if (e.Data.GetData(typeof(DeviceViewModel)) is DeviceViewModel)
            {
                var data = (DeviceViewModel)e.Data.GetData(typeof(DeviceViewModel));
                if (data != null)
                {
                    this.AddDevice(data, e.GetPosition(this));
                }
            }
        }

        public void AddDevice(DeviceViewModel deviceViewModel, Point point)
        {
            var newItem = new DesignerItem();
            var content = DeviceGuiAnyFactory.Instance.GetDeviceControl(deviceViewModel);
            newItem.Content = content;
            newItem.Payload = deviceViewModel;
            
            foreach(var item in this.Children)
            {
                if(item is DesignerItem)
                {
                    if(((DesignerItem)item).Payload is DeviceViewModel)
                    {
                        var itemvm = ((DeviceViewModel)((DesignerItem)item).Payload);
                        if(itemvm.GetCacheId()==deviceViewModel.GetCacheId())
                        {
                            return;
                        }
                    }
                }
            }

            Point position = point;
            DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X));
            DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y));

            this.Children.Add(newItem);
            SetConnectorDecoratorTemplate(newItem);
        }

        public void UpdateDevice(DeviceViewModel deviceViewModel)
        {
            foreach (var item in this.Children)
            {
                if (item is DesignerItem)
                {
                    if (((DesignerItem)item).Payload is DeviceViewModel)
                    {
                        var itemvm = ((DeviceViewModel)((DesignerItem)item).Payload);
                        if (itemvm.GetCacheId() == deviceViewModel.GetCacheId())
                        {
                            ((DesignerItem)item).Content = DeviceGuiAnyFactory.Instance.GetDeviceControl(deviceViewModel);
                            ((DesignerItem)item).Payload = deviceViewModel;
                            return;
                        }
                    }
                }
            }
        }


        protected IEnumerable<DesignerItem> GetDesignerItems()
        {
            return this.Children.OfType<DesignerItem>();
        }

        protected IEnumerable<Connection> GetConnections()
        {
            return this.Children.OfType<Connection>();
        }

        public IEnumerable<DiagramDevice> GetDiagramDevices()
        {
            var designerItems = this.GetDesignerItems();
            var diagramdevices = new List<DiagramDevice>();
            foreach(var item in designerItems)
            {
                if(item.Content is IDevicePresenter)
                {
                    var devicePresenter = (IDevicePresenter)item.Content;
                    var viewModel = devicePresenter.GetDeviceViewModel();

                    var diagramDevice = new DiagramDevice() { CachedObjectId = viewModel.GetCacheId(), Point = new Point<double>(DesignerCanvas.GetLeft(item), DesignerCanvas.GetTop(item)), Size = new Size(item.ActualWidth, item.ActualHeight) };
                    diagramdevices.Add(diagramDevice);
                }
            }
            return diagramdevices;
        }

        public void LoadDiagramDevicess(IEnumerable<DiagramDevice> diagramDevices, IEnumerable<DeviceViewModel> availableDevices)
        {
            this.Children.Clear();
            foreach(var item in diagramDevices)
            {
                var designerItem = new DesignerItem();
                DeviceViewModel deviceViewModel = null;
                foreach(var device in availableDevices)
                {
                    if(item.CachedObjectId == device.GetCacheId())
                    {
                        deviceViewModel = device;
                    }
                }
                designerItem.Content = DeviceGuiAnyFactory.Instance.GetDeviceControl(deviceViewModel);
                DesignerCanvas.SetLeft(designerItem, item.Point.X);
                DesignerCanvas.SetTop(designerItem, item.Point.Y);
                designerItem.Width = item.Size.Width;
                designerItem.Height = item.Size.Height;
                this.Children.Add(designerItem);
            }
        }

        public void RemoveDevice(DeviceViewModel deviceViewModel)
        {
            if(deviceViewModel==null)
            {
                return;
            }
            UIElement toRemove = null;
            foreach (var item in this.Children)
            {
                if (item is DesignerItem)
                {
                    if (((DesignerItem)item).Payload is DeviceViewModel)
                    {
                        var itemvm = ((DeviceViewModel)((DesignerItem)item).Payload);
                        if (itemvm.GetCacheId() == deviceViewModel.GetCacheId())
                        {
                            toRemove = (DesignerItem)item;
                            break;
                        }
                    }
                }
            }
            if(toRemove!=null)
            {
                this.Children.Remove(toRemove);
            }
        }

        public void SetBackground(FrameworkElement background,DiagramItemMetadata backgroundMeta)
        {
            if(this.background!=null)
            {
                if(this.Children.Contains(this.background))
                    this.Children.Remove(this.background);
            }
            this.background = background;
            var di = this.background;
            this.Children.Add(di);
            DeviceDesignerCanvas.SetZIndex(di, DeviceDesignerCanvas.BackgroundZIndex);
            DeviceDesignerCanvas.SetLeft(di,backgroundMeta.Point.X);
            DeviceDesignerCanvas.SetTop(di,backgroundMeta.Point.Y);
            di.HorizontalAlignment = HorizontalAlignment.Left;
            di.VerticalAlignment = VerticalAlignment.Top;
            if(double.IsNaN(backgroundMeta.Size.Width) || double.IsNaN(backgroundMeta.Size.Height))
            {
                return;
            }
            
            di.Width = backgroundMeta.Size.Width;
            di.Height = backgroundMeta.Size.Height;
        }

        public FrameworkElement GetBackground()
        {
            return this.background;
        }

        public void UpdateBackgroundMetadata(DiagramItemMetadata backgroundMeta)
        {
            var di = this.background;
            di.Width = backgroundMeta.Size.Width;
            di.Height = backgroundMeta.Size.Height;
        }

        public DiagramItemMetadata GetMapMetadata()
        {
            FrameworkElement backgroundContainer =null;
            foreach(var child in this.Children)
            {
                if (child == this.background)
                {
                    backgroundContainer = (FrameworkElement)child;
                }
            }
            if (backgroundContainer == null)
            {
                return new DiagramItemMetadata();
            }
            var item = new DiagramItemMetadata()
            {
                Point = new Point<double>(DeviceDesignerCanvas.GetLeft(backgroundContainer),DeviceDesignerCanvas.GetTop(backgroundContainer)),
                Size = new Size(backgroundContainer.ActualWidth, backgroundContainer.ActualHeight)
            };
            return item;
        }

        public void ClearCanvas()
        {
            this.Children.Clear();
        }

        public void DeleteSelection()
        {
            this.DeleteCurrentSelection();
        }
    }
}
