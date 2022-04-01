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
    /// Interaction logic for VirtualZigBeeCoordinatorUserControl.xaml
    /// </summary>
    public partial class VirtualZigBeeCoordinatorUserControl : UserControl,IZigBeePresenter
    {
        public VirtualZigBeeCoordinatorUserControl()
        {
            InitializeComponent();
        }

        public VirtualZigBeeCoordinatorUserControl(ZigBeeViewModel zigBeeViewModel)
        {
            InitializeComponent();
            this.DataContext = zigBeeViewModel;
        }

        public ZigBeeViewModel GetZigBeeViewModel()
        {
            return this.DataContext as ZigBeeViewModel;
        }
    }
}
