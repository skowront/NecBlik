using NecBlik.Common.WpfExtensions.Base;
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
using System.Reflection;

namespace NecBlik.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
        }

        public class AboutViewModel: BaseViewModel
        {
            public string Version
            {   
                get {
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
            }
        }
    }
}
