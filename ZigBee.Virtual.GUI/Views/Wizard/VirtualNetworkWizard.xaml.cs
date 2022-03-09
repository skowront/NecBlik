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
using ZigBee.Virtual.GUI.ViewModels.Wizard;

namespace ZigBee.Virtual.GUI.Views.Wizard
{
    /// <summary>
    /// Interaction logic for VirtualNetworkWizard.xaml
    /// </summary>
    public partial class VirtualNetworkWizard : Window
    {
        public VirtualNetworkWizard()
        {
            InitializeComponent();
        }

        public VirtualNetworkWizard(VirtualNetworkWizardViewModel virtualNetworkWizardViewModel)
        {
            InitializeComponent();
            this.DataContext = virtualNetworkWizardViewModel;
        }
    }
}
