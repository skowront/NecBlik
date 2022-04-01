using NecBlik.Common.WpfExtensions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NecBlik.Common.WpfElements.PopupValuePickers
{
    /// <summary>
    /// ViewModel
    /// </summary>
    public class GenericValuePicker<T> : WindowViewModel
    {
        /// <summary>
        /// Captured value holder
        /// </summary>
        private T value;
        public T Value
        {
            get { return this.value; }
            set { this.value = value; this.OnPropertyChanged(); }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public Action<T> OnConfirm = null;

        public Action<T> OnCancel = null;

        /// <summary>
        /// Ctor with building parameters
        /// </summary>
        /// <param name="window">Window for which the controll is attached to</param>
        /// <param name="message">Message</param>
        /// <param name="title">Top title</param>
        /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
        /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
        public GenericValuePicker(Window window, Action<T> onConfirm, Action<T> onCancel) : base(window)
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
