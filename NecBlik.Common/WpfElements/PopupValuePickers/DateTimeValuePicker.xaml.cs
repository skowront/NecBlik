using NecBlik.Common.WpfExtensions.Base;
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
    /// Interaction logic for DateTimeValuePicker.xaml
    /// </summary>
    public partial class DateTimeValuePicker : Window
    {
        public DateTimeValuePickerViewModel ViewModel;

        public DateTimeValuePicker(Action<DateTime> onConfirm, Action<DateTime> onCancel)
        {
            InitializeComponent();
            this.ViewModel = new DateTimeValuePickerViewModel(this, onConfirm, onCancel);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public class DateTimeValuePickerViewModel : WindowViewModel
        {
            /// <summary>
            /// Captured value holder
            /// </summary>
            private DateTime value = DateTime.Now;
            public DateTime Value
            {
                get { return this.value; }
                set { this.value = value; this.OnPropertyChanged(); }
            }

            public RelayCommand ConfirmCommand { get; set; }
            public RelayCommand CancelCommand { get; set; }

            public Action<DateTime> OnConfirm = null;

            public Action<DateTime> OnCancel = null;

            /// <summary>
            /// Ctor with building parameters
            /// </summary>
            /// <param name="window">Window for which the controll is attached to</param>
            /// <param name="message">Message</param>
            /// <param name="title">Top title</param>
            /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
            /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
            public DateTimeValuePickerViewModel(Window window, Action<DateTime> onConfirm, Action<DateTime> onCancel) : base(window)
            {
                this.OnConfirm = onConfirm;
                this.OnCancel = onCancel;
                this.ConfirmCommand = new RelayCommand(o =>
                {
                    this.OnConfirm?.Invoke(this.value);
                    window.Close();
                });
                this.CancelCommand = new RelayCommand(o =>
                {
                    this.OnCancel?.Invoke(this.value);
                    window.Close();
                });
            }
        }
    }
}
