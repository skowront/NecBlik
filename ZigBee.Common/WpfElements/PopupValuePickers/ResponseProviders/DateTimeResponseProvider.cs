using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfElements.PopupValuePickers.ResponseProviders
{
    /// <summary>
    /// Response provider that gets an input from user.
    /// </summary>
    public class DateTimeResponseProvider : IResponseProvider<DateTime, object>
    {
        private DateTimeValuePicker Popup = null;

        private DateTime result = DateTime.Now;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public DateTimeResponseProvider(DateTimeValuePicker popup)
        {
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            this.Popup.Dispatcher.Invoke(() =>
            {
                popup.ViewModel.OnConfirm = (s) => { this.result = s; };
                popup.ViewModel.OnCancel = (s) => { this.result = s; };
            });
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Response</returns>
        public DateTime ProvideResponse(object context = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new DateTimeValuePicker(null, null);
            Popup.ViewModel.OnConfirm = (s) => { this.result = s; };
            Popup.ViewModel.OnCancel = (s) => { this.result = DateTime.Now; };
            old.Dispatcher.Invoke(() =>
            {
                Popup?.ShowDialog();
            });
            return result;
        }
    }
}
