using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.Models;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.ViewModels;
using ZigBee.Core.GUI.Factories;

namespace ZigBee.Views.Controls
{
    public class ZigBeeDesignerCanvas:DesignerCanvas
    {
        private const int BackgroundZIndex = -1;

        private FrameworkElement background;

        public ZigBeeDesignerCanvas() : base()
        {
            this.Focusable = true;
            this.IsHitTestVisible = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            //base.OnDrop(e);
            if (e.Data.GetData(typeof(ZigBeeViewModel)) is ZigBeeViewModel)
            {
                var data = (ZigBeeViewModel)e.Data.GetData(typeof(ZigBeeViewModel));
                if (data != null)
                {
                    this.AddZigBee(data, e.GetPosition(this));
                }
            }
        }

        public void AddZigBee(ZigBeeViewModel zigBeeViewModel, Point point)
        {
            var newItem = new DesignerItem();
            var content = ZigBeeGuiAnyFactory.Instance.GetZigBeeControl(zigBeeViewModel);
            newItem.Content = content;
            newItem.Payload = zigBeeViewModel;
            
            foreach(var item in this.Children)
            {
                if(item is DesignerItem)
                {
                    if(((DesignerItem)item).Payload is ZigBeeViewModel)
                    {
                        var itemvm = ((ZigBeeViewModel)((DesignerItem)item).Payload);
                        if(itemvm.GetCacheId()==zigBeeViewModel.GetCacheId())
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

        public void UpdateZigBee(ZigBeeViewModel zigBeeViewModel)
        {
            foreach (var item in this.Children)
            {
                if (item is DesignerItem)
                {
                    if (((DesignerItem)item).Payload is ZigBeeViewModel)
                    {
                        var itemvm = ((ZigBeeViewModel)((DesignerItem)item).Payload);
                        if (itemvm.GetCacheId() == zigBeeViewModel.GetCacheId())
                        {
                            ((DesignerItem)item).Content = ZigBeeGuiAnyFactory.Instance.GetZigBeeControl(zigBeeViewModel);
                            ((DesignerItem)item).Payload = zigBeeViewModel;
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

        public IEnumerable<DiagramZigBee> GetDiagramZigBees()
        {
            var designerItems = this.GetDesignerItems();
            var diagramZigBees = new List<DiagramZigBee>();
            foreach(var item in designerItems)
            {
                if(item.Content is IZigBeePresenter)
                {
                    var zigBeePresenter = (IZigBeePresenter)item.Content;
                    var viewModel = zigBeePresenter.GetZigBeeViewModel();

                    var diagramZigBee = new DiagramZigBee() { CachedObjectId = viewModel.GetCacheId(), Point = new Point<double>(DesignerCanvas.GetLeft(item), DesignerCanvas.GetTop(item)), Size = new Size(item.ActualWidth, item.ActualHeight) };
                    diagramZigBees.Add(diagramZigBee);
                }
            }
            return diagramZigBees;
        }

        public void LoadDiagramZigBees(IEnumerable<DiagramZigBee> diagramZigBees, IEnumerable<ZigBeeViewModel> availableZigBees)
        {
            this.Children.Clear();
            foreach(var item in diagramZigBees)
            {
                var designerItem = new DesignerItem();
                ZigBeeViewModel zigBeeViewModel = null;
                foreach(var zigBee in availableZigBees)
                {
                    if(item.CachedObjectId == zigBee.GetCacheId())
                    {
                        zigBeeViewModel = zigBee;
                    }
                }
                designerItem.Content = ZigBeeGuiAnyFactory.Instance.GetZigBeeControl(zigBeeViewModel);
                DesignerCanvas.SetLeft(designerItem, item.Point.X);
                DesignerCanvas.SetTop(designerItem, item.Point.Y);
                designerItem.Width = item.Size.Width;
                designerItem.Height = item.Size.Height;
                this.Children.Add(designerItem);
            }
        }

        public void RemoveZigBee(ZigBeeViewModel zigBeeViewModel)
        {
            if(zigBeeViewModel==null)
            {
                return;
            }
            UIElement toRemove = null;
            foreach (var item in this.Children)
            {
                if (item is DesignerItem)
                {
                    if (((DesignerItem)item).Payload is ZigBeeViewModel)
                    {
                        var itemvm = ((ZigBeeViewModel)((DesignerItem)item).Payload);
                        if (itemvm.GetCacheId() == zigBeeViewModel.GetCacheId())
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
            ZigBeeDesignerCanvas.SetZIndex(di, ZigBeeDesignerCanvas.BackgroundZIndex);
            ZigBeeDesignerCanvas.SetLeft(di,backgroundMeta.Point.X);
            ZigBeeDesignerCanvas.SetTop(di,backgroundMeta.Point.Y);
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
                Point = new Point<double>(ZigBeeDesignerCanvas.GetLeft(backgroundContainer),ZigBeeDesignerCanvas.GetTop(backgroundContainer)),
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
