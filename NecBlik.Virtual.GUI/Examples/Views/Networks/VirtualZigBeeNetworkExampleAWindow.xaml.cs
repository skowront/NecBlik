using DiagramDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NecBlik.Core.GUI;
using NecBlik.Virtual.GUI.ViewModels;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Virtual.GUI.Examples.Views.Networks
{
    /// <summary>
    /// Interaction logic for VirtualZigBeeNetworkWindow.xaml
    /// </summary>
    public partial class VirtualZigBeeNetworkExampleAWindow : Window
    {
        public VirtualZigBeeNetworkViewModel ViewModel { get; set; }

        public VirtualZigBeeNetworkExampleAWindow()
        {
            InitializeComponent();
        }

        public VirtualZigBeeNetworkExampleAWindow(VirtualZigBeeNetworkViewModel vm):this()
        {
            this.ViewModel = vm;
            this.DataContext = this;
        }
    }
}
