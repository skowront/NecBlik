using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.Factories;

namespace NecBlik.Virtual.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VirtualDevice : Device
    {
        [JsonProperty]
        public string Address
        {
            get { return this.GetAddress(); }
            set { this.cachedAddress = value; }
        }

        [JsonProperty]
        public string Name { get; set; } = Resources.Resources.DefaultVirtualDeviceName;
        
        public string cachedAddress { get; protected set; } = string.Empty;

        public override string GetAddress()
        {
            if (cachedAddress == string.Empty)
            {
                this.cachedAddress = VirtualNetwork.generateAddress64bit();
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

        public VirtualDevice()
        {
            this.Name = Resources.Resources.DefaultVirtualDeviceName;
            this.internalType = (new VirtualDeviceFactory()).GetVendorID();
        }

        public VirtualDevice(bool generateAddress)
        {
            this.Name = Resources.Resources.DefaultVirtualDeviceName;
            this.internalType = (new VirtualDeviceFactory()).GetVendorID();
            if(generateAddress)
            {
                this.cachedAddress = string.Empty;
            }
        }

        public override void Send(string data, string address)
        {
            
        }

        public override bool IsLicensed()
        {
            return true;
        }

        public override IEnumerable<string> GetLicensees()
        {
            VirtualDeviceFactory f = new VirtualDeviceFactory();
            return new List<string>() { f.GetVendorID() };
        }
    }
}
