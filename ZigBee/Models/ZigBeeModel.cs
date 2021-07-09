using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZigBeeModel: IDuplicable<ZigBeeModel>
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [JsonProperty]
        public string Name { get; set; } = Resources.Resources.ZigBeeModelNameDefault;
        [JsonProperty]
        public string Version { get; set; } = Resources.Resources.ZigBeeVersionDefault;
        [JsonProperty]
        public List <Guid> ConnectedZigBeGuids { get; set; } = new List<Guid>();

        public ZigBeeModel Duplicate()
        {
            return new ZigBeeModel() { Guid = Guid.NewGuid(), Name = this.Name, Version = this.Version };
        }
    }
}
