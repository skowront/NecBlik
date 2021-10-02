using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZigBeeModel: IDuplicable<ZigBeeModel>
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [JsonProperty]
        public string Name { get; set; } = Resources.Resources.ZigBeeModelNameDefault;
        [JsonProperty]
        public string InternalFactoryType { get; set; }

        private string addressName { get; set; } = null;
        [JsonProperty]
        public string AddressName 
        { 
            get
            {
                if(this.addressName != null)
                {
                    return this.addressName;
                }
                if(this.ZigBeeSource == null)
                {
                    return Resources.Resources.ZigBeeUnableToRetrieveAddress;
                }
                this.addressName = this.ZigBeeSource?.GetAddress();
                return this.addressName;
            }
        }
        [JsonProperty]
        public string Version { get; set; } = Resources.Resources.ZigBeeVersionDefault;
        [JsonProperty]
        public List <Guid> ConnectedZigBeGuids { get; set; } = new List<Guid>();

        public IZigBeeSource ZigBeeSource { get; set; }

        public ZigBeeModel Duplicate()
        {
            return new ZigBeeModel() { Guid = Guid.NewGuid(), Name = this.Name, Version = this.Version, addressName = this.AddressName };
        }
    }
}
