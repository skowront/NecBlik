using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZigBee.Common.WpfExtensions.Interfaces
{
    /// <summary>
    /// Interface for a window that could be extended by new features.
    /// </summary>
    public interface IFeaturesExtensibleWindow
    {
        public void AddNewFeaturesUI(UIElement uIElement);
    }
}
