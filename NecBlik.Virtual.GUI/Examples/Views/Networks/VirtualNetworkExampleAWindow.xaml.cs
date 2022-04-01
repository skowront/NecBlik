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
    /// Interaction logic for VirtualDeviceNetworkWindow.xaml
    /// </summary>
    public partial class VirtualNetworkExampleAWindow : Window
    {
        public VirtualNetworkViewModel ViewModel { get; set; }

        public VirtualNetworkExampleAWindow()
        {
            InitializeComponent();
        }

        public VirtualNetworkExampleAWindow(VirtualNetworkViewModel vm):this()
        {
            this.ViewModel = vm;
            this.DataContext = this;
        }
    }
}
