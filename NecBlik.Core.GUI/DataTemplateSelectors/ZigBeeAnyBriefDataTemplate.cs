using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NecBlik.Core.GUI.Factories;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.DataTemplateSelectors
{
    public class ZigBeeAnyDataBriefTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ZigBeeNetworkViewModel)
                return ZigBeeGuiAnyFactory.Instance.GetNetworkBriefDataTemplate((item as ZigBeeNetworkViewModel).Model);
            else
                return new DataTemplate();
        }
    }
}
