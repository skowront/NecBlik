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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZigBee.Virtual.GUI.ViewModels;

namespace ZigBee.Virtual.GUI.Examples.Views.Sources
{
    /// <summary>
    /// Interaction logic for VirtualZigBeeWindow.xaml
    /// </summary>
    public partial class VirtualZigBeeExampleAWindow : Window
    {
        public VirtualZigBeeExampleAWindow()
        {
            InitializeComponent();
        }

        public VirtualZigBeeExampleAWindow(VirtualZigBeeViewModel virtualZigBeeViewModel) : this()
        {
            this.DataContext = virtualZigBeeViewModel;
        }
    }
}
