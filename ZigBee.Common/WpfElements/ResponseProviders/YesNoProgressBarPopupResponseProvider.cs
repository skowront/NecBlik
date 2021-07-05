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
    public class YesNoProgressBarPopupResponseProvider : IUpdatableResponseProvider<int,bool,string>
    {
        private SimpleYesNoProgressBarPopup Popup = null;

        private bool result = true;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup to be shown</param>
        public YesNoProgressBarPopupResponseProvider(SimpleYesNoProgressBarPopup popup)
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
        /// Function that provides a response from user.
        /// </summary>
        /// <param name="question">Question context</param>
        /// <returns>Result from user</returns>
        public bool ProvideResponse(string question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleYesNoProgressBarPopup(vm.Message, vm.Title, vm.Icon, null, null, vm.Min, vm.Value, vm.Max, vm.canClose, vm.canCloseAbort);
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
            return result;
        }

        public void Update(int newValue)
        {
            this.Popup.Dispatcher.Invoke(() => { this.Popup.SetProgressValue(newValue); });
        }

        public void SealUpdates()
        {
            this.Popup.Dispatcher.Invoke(() => { this.Popup.SetClosureEnabled(true); });
        }

        public void SetLimit(int limit)
        {
            this.Popup.Dispatcher.Invoke(() => { this.Popup.ViewModel.Max = limit; });
        }
    }
}
