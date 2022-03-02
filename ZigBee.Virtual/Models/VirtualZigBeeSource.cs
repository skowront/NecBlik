using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ZigBee.Core.Interfaces;

namespace ZigBee.Virtual.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VirtualZigBeeSource : IZigBeeSource
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [JsonProperty]
        public string Address
        {
            get { return this.GetAddress(); }
            set { this.cachedAddress = value; }
        }

        [JsonProperty]
        public string Name { get; set; } = "Virtualbee";

        private string internalSourceType = "Virtual";
        
        private string cachedAddress = string.Empty;

        public virtual string GetAddress()
        {
            if (cachedAddress == string.Empty)
            {
                this.cachedAddress = this.generateAddress64bit();
            }
            return this.cachedAddress;
        }

        public string generateAddress64bit()
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEF";
            var value = new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            while (value == "0000000000000000" || value == "000000000000FFFF" || TakenAddresses.Contains(value))
            {
                value = new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return value;
        }

        public void Save(string folderPath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = this.Guid + "." + this.internalSourceType + ".json";
            if (File.Exists(folderPath + "\\" + file))
            {
                File.WriteAllText(folderPath + "\\" + file, json);
            }
            else
            {
                File.AppendAllText(folderPath + "\\" + file, json);
            }
        }

        public string GetVendorID()
        {
            return this.internalSourceType;
        }

        public string GetName()
        {
            return this.Name;
        }

        public void SetName(string name)
        {
            this.Name = name;
            return;
        }

        public VirtualZigBeeSource()
        {

        }

        private static Collection<string> TakenAddresses = new Collection<string>();
    }
}
