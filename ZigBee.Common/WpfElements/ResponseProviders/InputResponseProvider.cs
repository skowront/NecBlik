using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Common.WpfElements.ResponseProviders
{
    /// <summary>
    /// Response provider that gets an input from user.
    /// </summary>
    public class InputResponseProvider : IResponseProvider<string, string>
    {
        private SimpleInputPopup Popup = null;

        private string result = string.Empty;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public InputResponseProvider(SimpleInputPopup popup)
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
        public string ProvideResponse(string question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleInputPopup(vm.Message, vm.Title, null, null);
            this.Popup.ViewModel.OnConfirm = (s) => { this.result = s; };
            this.Popup.ViewModel.OnCancel = (s) => { this.result = s; };
            old.Dispatcher.Invoke(() =>
            {
                if (question != null)
                {
                    this.Popup.ViewModel.Message = question;
                }
                this.Popup?.ShowDialog();
            });
            return result;
        }
    }
}
