using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Common.WpfElements.PopupValuePickers.ResponseProviders
{
    public class NumericResponseProvider<T>: IResponseProvider<T,object>
    {
        private NumericValuePicker Popup = null;

        private T result;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public NumericResponseProvider(NumericValuePicker popup)
        {
            popup.Initialize(
                new Action<T>((s) => { this.result = s; }),
                new Action<T>( (s) => { this.result = s; }));
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            //this.Popup.Dispatcher.Invoke(() =>
            //{
            //    (popup.DataContext as GenericValuePicker<T>).OnConfirm = 
            //    (popup.DataContext as GenericValuePicker<T>).OnCancel = ;
            //});
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Response</returns>
        public T ProvideResponse(object context = null)
        {
            var old = this.Popup;
            var vm = this.Popup.DataContext as GenericValuePicker<T>;
            this.Popup = new NumericValuePicker();
            this.Popup.Initialize<T>(null, null);
            (Popup.DataContext as GenericValuePicker<T>).OnConfirm = (s) => { this.result = s; };
            (Popup.DataContext as GenericValuePicker<T>).OnCancel = (s) => {  };
            old.Dispatcher.Invoke(() =>
            {
                Popup?.ShowDialog();
            });
            return result;
        }
    }
}
