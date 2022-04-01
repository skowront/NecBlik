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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Interfaces;

namespace NecBlik.Virtual.GUI.Views.Controls
{
    /// <summary>
    /// Interaction logic for VirtualCoordinatorUserControl.xaml
    /// </summary>
    public partial class VirtualCoordinatorUserControl : UserControl,IDevicePresenter
    {
        public VirtualCoordinatorUserControl()
        {
            InitializeComponent();
        }

        public VirtualCoordinatorUserControl(DeviceViewModel deviceViewModel)
        {
            InitializeComponent();
            this.DataContext = deviceViewModel;
        }

        public DeviceViewModel GetDeviceViewModel()
        {
            return this.DataContext as DeviceViewModel;
        }
    }
}
