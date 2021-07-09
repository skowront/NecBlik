using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZigBee.ViewModels;

namespace ZigBee.Views.Controls
{
    public class ZigBeeDesignerCanvas:DesignerCanvas
    {
        protected override void OnDrop(DragEventArgs e)
        {
            //base.OnDrop(e);
            if (e.Data.GetData(typeof(ZigBeeViewModel)) is ZigBeeViewModel)
            {
                var data = (ZigBeeViewModel)e.Data.GetData(typeof(ZigBeeViewModel));
                if (data != null)
                {
                    var newItem = new DesignerItem();
                    newItem.Content = new ZigBeeControl(data);

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
    }
}
