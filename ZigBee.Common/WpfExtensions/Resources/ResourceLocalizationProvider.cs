using ZigBee.Common.Strings;
using ZigBee.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Common.WpfExtensions.Resources
{
    public class ResourceLocalizationProvider:IResponseProvider<string,string>
    {
        private ResourceManager resourceManager;

        public ResourceLocalizationProvider(ResourceManager resourceManager=null)
        {
            if(resourceManager!=null)
            {
                this.resourceManager = resourceManager;
            }
            else
            {
                this.resourceManager = SR.ResourceManager;
            }
        }

        public string ProvideResponse(string context = null)
        {
            return this.resourceManager.GetString(context);
        }
    }
}
