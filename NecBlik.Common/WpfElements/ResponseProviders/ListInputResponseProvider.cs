using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Common.WpfElements.ResponseProviders
{
    /// <summary>
    /// Response provider that gets an input from user.
    /// </summary>
    public class ListInputResponseProvider : IResponseProvider<string, Tuple<string,IEnumerable<string>>>
    {
        private SimpleListInputPopup Popup = null;

        private string result = string.Empty;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="popup">Popup</param>
        public ListInputResponseProvider(SimpleListInputPopup popup)
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
        public string ProvideResponse(Tuple<string,IEnumerable<string>> question = null)
        {
            var old = this.Popup;
            var vm = this.Popup.ViewModel;
            this.Popup = new SimpleListInputPopup(vm.Message, vm.Title, question.Item2, null, null,vm.Editable);
            this.Popup.ViewModel.OnConfirm = (s) => { this.result = s; };
            this.Popup.ViewModel.OnCancel = (s) => { this.result = s; };
            old.Dispatcher.Invoke(() =>
            {
                if (question != null)
                {
                    this.Popup.ViewModel.Message = question.Item1;
                }
                this.Popup?.ShowDialog();
            });
            return result;
        }
    }
}
