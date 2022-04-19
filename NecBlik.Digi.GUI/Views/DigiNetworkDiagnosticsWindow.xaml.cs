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
using NecBlik.Digi.GUI.ViewModels;
using NecBlik.Digi.GUI.Views.Pages;

namespace NecBlik.Digi.GUI.Views
{
    /// <summary>
    /// Interaction logic for DigiNetworkDiagnosticsWindow.xaml
    /// </summary>
    public partial class DigiNetworkDiagnosticsWindow
    {
        public DigiNetworkDiagnosticsViewModel ViewModel { get; set; }

        private ThroughputPage throughputPage;
        private RangePage rangePage;
        public DigiNetworkDiagnosticsWindow()
        {
            InitializeComponent();
        }

        public DigiNetworkDiagnosticsWindow(DigiNetworkDiagnosticsViewModel viewModel) : this()
        {
            this.ViewModel = viewModel;
            this.DataContext = viewModel;
            this.frame.Navigate(new ThroughputPage(this.ViewModel));
        }

        private void RangeClick(object sender, RoutedEventArgs e)
        {
            if (this.rangePage == null)
                this.rangePage = new RangePage(this.ViewModel);
            this.frame.Navigate(this.rangePage);
        }
        

        private void ThroughputClick(object sender, RoutedEventArgs e)
        {
            if (this.throughputPage == null)
                this.throughputPage = new ThroughputPage(this.ViewModel);
            this.frame.Navigate(this.throughputPage);
        }
    }
}
