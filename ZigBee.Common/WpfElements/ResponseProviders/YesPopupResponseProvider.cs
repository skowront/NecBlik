using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfElements.ResponseProviders
{
    /// <summary>
    /// IResponseProvider implementation
    /// </summary>
    public class YesPopupResponseProvider : IResponseProvider<bool, string>
    {
        private SimpleYesPopup Popup = null;

        private bool result = true;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup to be shown</param>
        public YesPopupResponseProvider(SimpleYesPopup popup)
        {
            if (popup == null)
            {
                return;
            }
            this.Popup = popup;
            this.Popup.Dispatcher.Invoke(() =>
            {
                this.Popup.ViewModel.OnConfirm = () => { this.result = true; };
            });
        }

        /// <summary>
        /// Shows a proper popup to user and gets a response
        /// </summary>
        /// <param name="question">Question context</param>
        /// <returns>Result from user</returns>
        public bool ProvideResponse(string question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleYesPopup(vm.Message, vm.Title, vm.Icon);
            this.Popup.ViewModel.OnConfirm = () => { this.result = true; };
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
