using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Common.WpfElements.PopupValuePickers.ResponseProviders
{
    /// <summary>
    /// Response provider that gets an input from user.
    /// </summary>
    public class FloatResponseProvider : IResponseProvider<float, object>
    {
        private FloatValuePicker Popup = null;

        private float result = 0;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public FloatResponseProvider(FloatValuePicker popup)
        {
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            this.Popup.Dispatcher.Invoke(() =>
            {
                (popup.DataContext as GenericValuePicker<float>).OnConfirm = (s) => { this.result = s; };
                (popup.DataContext as GenericValuePicker<float>).OnCancel = (s) => { this.result = s; };
            });
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question"></param>
        /// <returns>Response</returns>
        public float ProvideResponse(object context = null)
        {
            var old = this.Popup;
            var vm = this.Popup.DataContext as GenericValuePicker<float>;
            this.Popup = new FloatValuePicker(null, null);
            (Popup.DataContext as GenericValuePicker<float>).OnConfirm = (s) => { this.result = s; };
            (Popup.DataContext as GenericValuePicker<float>).OnCancel = (s) => { this.result = 0; };
            old.Dispatcher.Invoke(() =>
            {
                Popup?.ShowDialog();
            });
            return result;
        }
    }
}
