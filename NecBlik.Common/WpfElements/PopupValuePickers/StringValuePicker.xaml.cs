using NecBlik.Common.WpfExtensions.Interfaces;
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

namespace NecBlik.Common.WpfElements.PopupValuePickers
{
    /// <summary>
    /// Interaction logic for StringValuePicker.xaml
    /// </summary>
    public partial class StringValuePicker : Window
    {
        public StringValuePicker(Action<string> onConfirm, Action<string> onCancel)
        {
            InitializeComponent();
            this.DataContext = new GenericValuePicker<string>(this, onConfirm, onCancel);
        }
    }
}
