using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZigBee.Interfaces;
using ZigBee.Models;
using ZigBee.ViewModels;

namespace ZigBee.Views.Controls
{
    public class ZigBeeDesignerCanvas:DesignerCanvas
    {
        private const int BackgroundZIndex = -1;

        private UIElement background;

        public ZigBeeDesignerCanvas() : base()
        {

        }

        protected override void OnDrop(DragEventArgs e)
        {
            //base.OnDrop(e);
            if (e.Data.GetData(typeof(ZigBeeViewModel)) is ZigBeeViewModel)
            {
                var data = (ZigBeeViewModel)e.Data.GetData(typeof(ZigBeeViewModel));
                if (data != null)
                {
                    //var newItem = new ZigBeeDesignerItem(data, new ZigBeeControl(data));
                    var newItem = new DesignerItem();
                    newItem.Content = new ZigBeeControl(data);
                    newItem.Payload = data;

                    Point position = e.GetPosition(this);
                    DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y));

                    //Canvas.SetZIndex(newItem, this.Children.Count);
                    this.Children.Add(newItem);
                    SetConnectorDecoratorTemplate(newItem);

                    //////update selection
                    //this.SelectionService.SelectItem(newItem);
                    //newItem.Focus();
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

                    var diagramZigBee = new DiagramZigBee() { ZigBeeGuid = viewModel.Guid, Point = new Point<double>(DesignerCanvas.GetLeft(item), DesignerCanvas.GetTop(item)), Size = new Size(item.Width, item.Height) };
                    diagramZigBees.Add(diagramZigBee);
                }
            }
            return diagramZigBees;
        }

        public void LoadDiagramZigBees(IEnumerable<DiagramZigBee> diagramZigBees, IEnumerable<ZigBeeViewModel> availableZigBees)
        {
            foreach(var item in diagramZigBees)
            {
                var designerItem = new DesignerItem();
                ZigBeeViewModel zigBeeViewModel = null;
                foreach(var zigBee in availableZigBees)
                {
                    if(item.ZigBeeGuid == zigBee.Guid)
                    {
                        zigBeeViewModel = zigBee;
                    }
                }
                designerItem.Content = new ZigBeeControl(zigBeeViewModel);
                DesignerCanvas.SetLeft(designerItem, item.Point.X);
                DesignerCanvas.SetTop(designerItem, item.Point.Y);
                designerItem.Width = item.Size.Width;
                designerItem.Height = item.Size.Height;
                this.Children.Add(designerItem);
            }
        }

        public void SetBackground(UIElement background)
        {
            if(this.background!=null)
            {
                DesignerItem currentBg = new DesignerItem(); ;
                foreach(var item in this.Children)
                {
                    if(item is DesignerItem)
                    {
                        var di = (DesignerItem)item;
                        if(di.Content==this.background)
                        {
                            currentBg = di;
                            break;
                        }
                    }
                }
                this.Children.Remove(currentBg);
            }
            this.background = background;
            this.Children.Add(new DesignerItem() { Content = background});
            DesignerCanvas.SetZIndex(this.background, ZigBeeDesignerCanvas.BackgroundZIndex);
        }

        public void ClearCanvas()
        {
            this.Children.Clear();
        }

        public void ClearSelection()
        {
            this.DeleteCurrentSelection();
        }
    }
}
