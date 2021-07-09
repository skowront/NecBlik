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
using ZigBee.ViewModels;

namespace ZigBee.Views.Controls
{
    /// <summary>
    /// Interaction logic for ZigBeeControl.xaml
    /// </summary>
    public partial class ZigBeeControl : UserControl
    {
        public ZigBeeControl()
        {
            InitializeComponent();
        }

        public ZigBeeControl(ZigBeeViewModel zigBeeViewModel)
        {
            InitializeComponent();
            this.DataContext = zigBeeViewModel;
        }
    }
}
