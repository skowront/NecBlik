using NecBlik.Common.WpfExtensions.Interfaces;
using System;
using System.Windows;

namespace NecBlik.Common.WpfElements.PopupValuePickers
{
    /// <summary>
    /// Interaction logic for FloatValuePicker.xaml
    /// </summary>
    public partial class FloatValuePicker
    {
        public FloatValuePicker(Action<float> onConfirm, Action<float> onCancel)
        {
            InitializeComponent();
            this.DataContext = new GenericValuePicker<float>(this, onConfirm, onCancel);
        }
    }
}
