using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Common.WpfElements.ResponseProviders
{
    /// <summary>
    /// Response provider implementation
    /// </summary>
    public class YesNoPopupResponseProvider : IResponseProvider<bool, string>
    {
        private SimpleYesNoPopup Popup = null;

        private bool result = true;

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="popup">Popup to be displayed</param>
        public YesNoPopupResponseProvider(SimpleYesNoPopup popup)
        {
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            this.Popup.Dispatcher.Invoke(() =>
            {
                popup.ViewModel.OnConfirm = () => { this.result = true; };
                popup.ViewModel.OnCancel = () => { this.result = false; };
            });
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question">Question context</param>
        /// <returns>Response</returns>
        public bool ProvideResponse(string question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleYesNoPopup(vm.Message, vm.Title, vm.Icon);
            this.Popup.ViewModel.OnConfirm = () => { this.result = true; };
            this.Popup.ViewModel.OnCancel = () => { this.result = false; };
            old.Dispatcher.Invoke(() =>
            {
                if (question != null)
                {
                    this.Popup.ViewModel.Message = question;
                }
                this.Popup?.ShowDialog();
            });
            old.Close();
            return result;
        }
    }
}
