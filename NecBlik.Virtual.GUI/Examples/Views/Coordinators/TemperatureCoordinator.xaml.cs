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
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Virtual.GUI.Examples.Views.Coordinators
{
    /// <summary>
    /// Interaction logic for VirtualDeviceWindow.xaml
    /// </summary>
    public partial class VirtualCoordinatorExampleAWindow : Window
    {
        public VirtualCoordinatorExampleAWindow()
        {
            InitializeComponent();
        }

        public VirtualCoordinatorExampleAWindow(VirtualDeviceViewModel virtualDeviceViewModel) : this()
        {
            this.DataContext = virtualDeviceViewModel;
        }
    }
}
