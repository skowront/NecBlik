using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    public class ZigBeeSource : IZigBeeSource
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        private string internalType { get; set; } = "Default";

        public virtual string GetVendorID()
        {
            return internalType;
        }

        public string GetAddress()
        {
            return "??";
        }

        public void Save(string folderPath)
        {
            return;
        }

        public string GetName()
        {
            return "ZigBee";
        }

        public void SetName(string name)
        {
            return;
        }

        public Guid GetGuid()
        {
            return this.Guid;
        }

        public void SetGuid(Guid guid)
        {
            this.Guid = guid;
        }
    }
}
