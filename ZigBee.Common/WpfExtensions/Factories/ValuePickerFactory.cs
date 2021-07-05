using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using ZigBee.Common.Strings;

namespace ZigBee.Common.WpfExtensions.Factories
{
    public class ValuePickerFactory
    {
        public ValuePickerFactory()
        {

        }

        virtual public Dictionary<Type, IResponseProvider<object, object>> ListAvailableValuePickers()
        {
            var dict = new Dictionary<Type, IResponseProvider<object, object>>();
            dict[typeof(string)] = new GenericResponseProvider<object, object>(string.Empty);
            dict[typeof(float)] = new GenericResponseProvider<object, object>(0.0f);
            dict[typeof(DateTime)] = new GenericResponseProvider<object, object>(DateTime.Now);
            return dict;
        }

        virtual public Dictionary<Type, IResponseProvider<object, object>> ListDefaultValuePickers()
        {
            var dict = new Dictionary<Type, IResponseProvider<object, object>>();
            dict[typeof(string)] = new GenericResponseProvider<object, object>(string.Empty);
            dict[typeof(float)] = new GenericResponseProvider<object, object>(0.0f);
            dict[typeof(DateTime)] = new GenericResponseProvider<object, object>(DateTime.Now);
            return dict;
        }
        
        virtual public Dictionary<Type, Func<string, object>> ListAvailableFromStringConverters()
        {
            var dict = new Dictionary<Type, Func<string, object>>();
            dict[typeof(string)] = o => { return o; };
            dict[typeof(float)] = o => { return float.Parse(o); };
            dict[typeof(DateTime)] = o => { return DateTime.Parse(o); };
            return dict;
        }

        virtual public Dictionary<Type, Func<object, string>> ListAvailableToStringConverters()
        {
            var dict = new Dictionary<Type, Func<object, string>>();
            dict[typeof(string)] = o => { return (string)o; };
            dict[typeof(float)] = o => { return o.ToString(); };
            dict[typeof(DateTime)] = o => { return o.ToString(); };
            return dict;
        }

        virtual public Dictionary<Type, Dictionary<string, Func<object, object, bool>>> ListAvailableOperators()
        {
            var dict = new Dictionary<Type, Dictionary<string, Func<object, object, bool>>>();
            dict[typeof(float)] = new Dictionary<string, Func<object, object, bool>>();
            dict[typeof(float)][SR.GPEquals] = (o1, o2) => { return ((float)o1) == ((float)o2); };
            dict[typeof(float)][SR.GPNotEquals] = (o1, o2) => { return ((float)o1) != ((float)o2); };
            dict[typeof(float)][SR.GPGreaterThan] = (o1, o2) => { return ((float)o1) > ((float)o2); };
            dict[typeof(float)][SR.GPLowerThan] = (o1, o2) => { return ((float)o1) < ((float)o2); };
            dict[typeof(DateTime)] = new Dictionary<string, Func<object, object, bool>>();
            dict[typeof(DateTime)][SR.GPEquals] = (o1, o2) => { return ((DateTime)o1) == ((DateTime)o2); };
            dict[typeof(DateTime)][SR.GPNotEquals] = (o1, o2) => { return ((DateTime)o1) != ((DateTime)o2); };
            dict[typeof(DateTime)][SR.GPGreaterThan] = (o1, o2) => { return ((DateTime)o1) > ((DateTime)o2); };
            dict[typeof(DateTime)][SR.GPLowerThan] = (o1, o2) => { return ((DateTime)o1) < ((DateTime)o2); };
            dict[typeof(string)] = new Dictionary<string, Func<object, object, bool>>();
            dict[typeof(string)][SR.GPContains] = (o1, o2) => { return ((string)o1).ToLower().Contains(((string)o2).ToLower()); };
            return dict;
        }
    }
}
