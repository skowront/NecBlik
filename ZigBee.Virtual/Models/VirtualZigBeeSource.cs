using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Factories;

namespace ZigBee.Virtual.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VirtualZigBeeSource : ZigBeeSource
    {
        [JsonProperty]
        public string Address
        {
            get { return this.GetAddress(); }
            set { this.cachedAddress = value; }
        }

        [JsonProperty]
        public string Name { get; set; } = Resources.Resources.DefaultVirtualZigBeeName;
        
        private string cachedAddress = string.Empty;

        public override string GetAddress()
        {
            if (cachedAddress == string.Empty)
            {
                this.cachedAddress = VirtualZigBeeNetwork.generateAddress64bit();
            }
            return this.cachedAddress;
        }

        public override void Save(string folderPath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = this.Guid + "." + this.internalType + ".json";
            if (File.Exists(folderPath + "\\" + file))
            {
                File.WriteAllText(folderPath + "\\" + file, json);
            }
            else
            {
                File.AppendAllText(folderPath + "\\" + file, json);
            }
        }

        public override string GetVendorID()
        {
            return this.internalType;
        }

        public override string GetName()
        {
            return this.Name;
        }

        public override void SetName(string name)
        {
            this.Name = name;
            return;
        }

        public VirtualZigBeeSource()
        {
            this.Name = Resources.Resources.DefaultVirtualZigBeeName;
            this.internalType = (new VirtualZigBeeFactory()).GetVendorID();
        }

        public override void Send(string data, string address)
        {
            
        }
    }
}
