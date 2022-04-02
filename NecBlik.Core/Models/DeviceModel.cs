using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;

namespace NecBlik.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceModel: IDuplicable<DeviceModel>
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        private string name = Resources.Resources.DeviceModelNameDefault;
        public string Name 
        {
            get
            {
                if (this.DeviceSource != null)
                {
                    return this.DeviceSource.GetName();
                }
                else return name;
            }
            set 
            {
                if (this.DeviceSource != null)
                {
                    this.DeviceSource.SetName(value);
                }
                else this.name = value;
            }  
        } 
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
                if(this.DeviceSource == null)
                {
                    return Resources.Resources.DeviceUnableToRetrieveAddress;
                }
                this.addressName = this.DeviceSource?.GetAddress();
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
                if (this.DeviceSource == null)
                {
                    return string.Empty;
                }
                this.cacheId = this.DeviceSource?.GetCacheId();
                return this.cacheId;
            }
        }

        private string version= Resources.Resources.DeviceVersionDefault;
        [JsonProperty]
        public string Version
        {
            get 
            {
                if(this.DeviceSource!=null)
                {
                    return DeviceSource.GetVersion();
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

        public IDeviceSource DeviceSource { get; set; }

        public DeviceModel(IDeviceSource source=null)
        {
            this.Guid = source?.GetGuid() ?? Guid.NewGuid();
            this.DeviceSource = source;
            this.Name = this.DeviceSource?.GetName();
        }

        public DeviceModel Duplicate()
        {
            return new DeviceModel() { Guid = Guid.NewGuid(), Name = this.Name, Version = this.Version, addressName = this.AddressName };
        }

        public bool IsLicensed()
        {
            return this.DeviceSource.IsLicensed();
        }

        public IEnumerable<string> GetLicensees()
        {
            return this.DeviceSource.GetLicensees();
        }
    }
}
