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
using NecBlik.ViewModels;

namespace NecBlik.Views
{
    /// <summary>
    /// Interaction logic for MapResizeWindow.xaml
    /// </summary>
    public partial class MapResizeWindow : Window
    {
        public MapResizeWindow()
        {
            InitializeComponent();
        }

        public MapResizeWindow(MapResizeViewModel mapResizeViewModel):this()
        {
            this.DataContext = mapResizeViewModel;
        }
    }
}
