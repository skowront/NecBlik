using NecBlik.Core.GUI.ViewModels;
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

namespace NecBlik.Core.GUI.Views
{
    /// <summary>
    /// Interaction logic for PingCoordinatorWindow.xaml
    /// </summary>
    public partial class PingCoordinatorWindow
    {
        public PingCoordinatorWindow()
        {
            InitializeComponent();
        }

        public PingCoordinatorWindow(PingViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
