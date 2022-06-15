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
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.ViewModels;

namespace NecBlik.Views
{
    /// <summary>
    /// Interaction logic for AppSettingsWindow.xaml
    /// </summary>
    public partial class AppSettingsWindow : IResponseProvider<Task<ApplicationSettingsViewModel>,ApplicationSettingsViewModel>
    {
        public ApplicationSettingsViewModel ViewModel { get; set; }

        public AppSettingsWindow()
        {
            InitializeComponent();
        }

        public async Task<ApplicationSettingsViewModel> ProvideResponse(ApplicationSettingsViewModel context = null)
        {
            if(context!=null)
            {
                this.ViewModel = context;
                this.DataContext = this.ViewModel;
            }
            var t = Task.Run(() => {
                Application.Current.Dispatcher.Invoke(() => { this.ShowDialog(); });
            });

            await t;
            return this.ViewModel;
        }
    }
}
