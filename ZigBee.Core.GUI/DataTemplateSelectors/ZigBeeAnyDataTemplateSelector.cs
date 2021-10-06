using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZigBee.Core.GUI.Factories;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.DataTemplateSelectors
{
    public class ZigBeeAnyDataTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ZigBeeNetworkViewModel)
                return ZigBeeGuiAnyFactory.Instance.GetNetworkDataTemplate((item as ZigBeeNetworkViewModel).Model);
            else
                return new DataTemplate();
        }
    }
}
