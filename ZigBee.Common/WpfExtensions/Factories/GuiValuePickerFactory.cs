using ZigBee.Common.WpfElements.PopupValuePickers;
using ZigBee.Common.WpfElements.PopupValuePickers.ResponseProviders;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfExtensions.Factories
{
    public class GuiValuePickerFactory:ValuePickerFactory
    {
        override public Dictionary<Type, IResponseProvider<object, object>> ListAvailableValuePickers()
        {
            var dict = new Dictionary<Type, IResponseProvider<object, object>>();
            dict[typeof(string)] = new GenericResponseProvider<object, object>(o=>{
                var responseProvider = new StringResponseProvider(new StringValuePicker(null, null));
                return responseProvider.ProvideResponse(o);
            });
            dict[typeof(float)] = new GenericResponseProvider<object, object>(o => {
                var responseProvider = new FloatResponseProvider(new FloatValuePicker(null, null));
                return responseProvider.ProvideResponse(o);
            });
            dict[typeof(DateTime)] = new GenericResponseProvider<object, object>(o => {
                var responseProvider = new DateTimeResponseProvider(new DateTimeValuePicker(null, null));
                return responseProvider.ProvideResponse(o);
            });
            return dict;
        }
    }
}
