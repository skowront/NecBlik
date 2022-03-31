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

        private string name = Resources.Resources.ZigBeeModelNameDefault;
        public string Name 
        {
            get
            {
                if (this.ZigBeeSource != null)
                {
                    return this.ZigBeeSource.GetName();
                }
                else return name;
            }
            set 
            {
                if (this.ZigBeeSource != null)
                {
                    this.ZigBeeSource.SetName(value);
                }
                else this.name = value;
            }  
        } 
        [JsonProperty]
        public string InternalFactoryType { get; set; }

        [JsonProperty]
        public string InternalFactorySubType { get; set; }

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

        private string cacheId { get; set; } = null;
        [JsonProperty]
        public string CacheId
        {
            get
            {
                if (this.cacheId != null)
                {
                    return this.cacheId;
                }
                if (this.ZigBeeSource == null)
                {
                    return string.Empty;
                }
                this.cacheId = this.ZigBeeSource?.GetCacheId();
                return this.cacheId;
            }
        }

        private string version= Resources.Resources.ZigBeeVersionDefault;
        [JsonProperty]
        public string Version
        {
            get 
            {
                if(this.ZigBeeSource!=null)
                {
                    return ZigBeeSource.GetVersion();
                }
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
        [JsonProperty]
        public List <Guid> ConnectedZigBeGuids { get; set; } = new List<Guid>();

        public IZigBeeSource ZigBeeSource { get; set; }

        public ZigBeeModel(IZigBeeSource source=null)
        {
            this.Guid = source?.GetGuid() ?? Guid.NewGuid();
            this.ZigBeeSource = source;
            this.Name = this.ZigBeeSource?.GetName();
        }

        public ZigBeeModel Duplicate()
        {
            return new ZigBeeModel() { Guid = Guid.NewGuid(), Name = this.Name, Version = this.Version, addressName = this.AddressName };
        }
    }
}
