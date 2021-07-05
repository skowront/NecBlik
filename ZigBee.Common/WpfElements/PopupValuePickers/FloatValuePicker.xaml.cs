using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Windows;

namespace ZigBee.Common.WpfElements.PopupValuePickers
{
    /// <summary>
    /// Interaction logic for FloatValuePicker.xaml
    /// </summary>
    public partial class FloatValuePicker : Window
    {
        public FloatValuePicker(Action<float> onConfirm, Action<float> onCancel)
        {
            InitializeComponent();
            this.DataContext = new GenericValuePicker<float>(this, onConfirm, onCancel);
        }

        
    }
}
