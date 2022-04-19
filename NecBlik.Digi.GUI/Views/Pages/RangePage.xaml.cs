using NecBlik.Digi.GUI.ViewModels;
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

namespace NecBlik.Digi.GUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for RangePage.xaml
    /// </summary>
    public partial class RangePage : Page
    {
        public RangePage()
        {
            InitializeComponent();
        }

        public RangePage(DigiNetworkDiagnosticsViewModel viewModel) : this()
        {
            this.DataContext = viewModel;
        }
    }
}
