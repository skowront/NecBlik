using NecBlik.Common.WpfExtensions.Interfaces;
using System;
using System.Windows;

namespace NecBlik.Common.WpfElements.PopupValuePickers
{
    /// <summary>
    /// Interaction logic for FloatValuePicker.xaml
    /// </summary>
    public partial class NumericValuePicker : Window
    {
        public NumericValuePicker()
        {
            InitializeComponent();
        }

        public void Initialize<T>(Action<T> onConfirm, Action<T> onCancel)
        {
            this.DataContext = new GenericValuePicker<T>(this, onConfirm, onCancel);
        }
    }
}
