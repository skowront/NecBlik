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

namespace NecBlik.Virtual.GUI.Views
{
    /// <summary>
    /// Interaction logic for VirtualZigBeeNetworkWindow.xaml
    /// </summary>
    public partial class VirtualZigBeeNetworkWindow : Window
    {
        public VirtualZigBeeNetworkViewModel ViewModel { get; set; }

        public VirtualZigBeeNetworkWindow()
        {
            InitializeComponent();
        }

        public VirtualZigBeeNetworkWindow(VirtualZigBeeNetworkViewModel vm):this()
        {
            this.ViewModel = vm;
            this.DataContext = this;
        }
    }
}
