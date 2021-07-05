using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Common.WpfElements.PopupValuePickers.ResponseProviders
{
    public class StringResponseProvider : IResponseProvider<string, object>
    {
        private StringValuePicker Popup = null;

        private string result = string.Empty;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public StringResponseProvider(StringValuePicker popup)
        {
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            this.Popup.Dispatcher.Invoke(() =>
            {
                (popup.DataContext as GenericValuePicker<string>).OnConfirm = (s) => { this.result = s; };
                (popup.DataContext as GenericValuePicker<string>).OnCancel = (s) => { this.result = s; };
            });
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Response</returns>
        public string ProvideResponse(object context = null)
        {
            var old = this.Popup;
            var vm = this.Popup.DataContext as GenericValuePicker<float>;
            this.Popup = new StringValuePicker(null, null);
            (Popup.DataContext as GenericValuePicker<string>).OnConfirm = (s) => { this.result = s; };
            (Popup.DataContext as GenericValuePicker<string>).OnCancel = (s) => { this.result = string.Empty; };
            old.Dispatcher.Invoke(() =>
            {
                Popup?.ShowDialog();
            });
            return result;
        }
    }
}
