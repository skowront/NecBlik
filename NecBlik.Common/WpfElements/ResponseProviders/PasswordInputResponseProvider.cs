using NecBlik.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Common.WpfElements.ResponseProviders
{
    /// <summary>
    /// Response provider that gets a password from user.
    /// </summary>
    public class PasswordInputResponseProvider : IResponseProvider<string, string>
    {
        private SimpleInpuPasswordPopup Popup = null;

        private string result = string.Empty;

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="popup"></param>
        public PasswordInputResponseProvider(SimpleInpuPasswordPopup popup)
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
        /// <returns>Password</returns>
        public string ProvideResponse(string question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleInpuPasswordPopup(vm.Message, vm.Title, null, null);
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
