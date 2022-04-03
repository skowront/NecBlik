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
using System.Windows.Shapes;

namespace NecBlik.Digi.GUI.Views
{
    /// <summary>
    /// Interaction logic for DigiNetworkWindow.xaml
    /// </summary>
    public partial class DigiNetworkWindow
    {
        public DigiZigBeeNetworkViewModel ViewModel { get; set; }

        public DigiNetworkWindow()
        {
            InitializeComponent();
        }

        public DigiNetworkWindow(DigiZigBeeNetworkViewModel vm) : this()
        {
            this.ViewModel = vm;
            this.DataContext = this;
        }
    }
}
