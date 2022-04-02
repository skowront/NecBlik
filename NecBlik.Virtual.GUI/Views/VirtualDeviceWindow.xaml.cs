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

namespace NecBlik.Virtual.GUI.Views
{
    /// <summary>
    /// Interaction logic for VirtualZigBeeWindow.xaml
    /// </summary>
    public partial class VirtualDeviceWindow
    {
        public VirtualDeviceWindow()
        {
            InitializeComponent();
        }

        public VirtualDeviceWindow(VirtualDeviceViewModel virtualDeviceViewModel) : this()
        {
            this.DataContext = virtualDeviceViewModel;
        }

        private void ComboBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.AvComboBox.ItemsSource = (this.DataContext as VirtualDeviceViewModel).AvailableDestinationAddresses;
        }
    }
}
