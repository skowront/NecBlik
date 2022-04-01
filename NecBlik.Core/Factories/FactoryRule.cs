using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Factories
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactoryRule
    {
        [JsonProperty]
        public string CacheObjectId { get; set; } = string.Empty;
        [JsonProperty]
        public string Property { get; set; } = string.Empty;
        [JsonProperty]
        public string Value { get; set; } = string.Empty;
    }
}
