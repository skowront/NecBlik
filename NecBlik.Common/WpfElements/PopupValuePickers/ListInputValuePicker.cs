using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Common.WpfElements.PopupValuePickers
{
    public class ListInputValuePicker : IResponseProvider<string, Tuple<string, IEnumerable<string>>>
    {
        private bool isEditable = false;
        public ListInputValuePicker(bool isEditable = false)
        {
            this.isEditable = isEditable;
        }

        public string ProvideResponse(Tuple<string, IEnumerable<string>> context = null)
        {
            string response = null;
            var popup = new SimpleListInputPopup(context.Item1, string.Empty, context.Item2,new Action<string>((o) => { response = o; }), new Action<string>((o) => { response = null; }),this.isEditable);
            var lvrp = new ListInputResponseProvider(popup);
            response = lvrp.ProvideResponse(context);
            return response;
        }
    }
}
