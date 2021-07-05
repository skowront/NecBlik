using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfExtensions.UI
{
    //https://brianlagunas.com/localize-enum-descriptions-in-wpf/
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        ResourceManager _resourceManager;
        string _resourceKey;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceManager = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string description = _resourceManager.GetString(_resourceKey);
                return string.IsNullOrWhiteSpace(description) ? string.Format("[[{0}]]", _resourceKey) : description;
            }
        }
    }
}
