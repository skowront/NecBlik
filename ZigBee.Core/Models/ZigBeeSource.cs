﻿using Newtonsoft.Json;
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
        protected string internalType { get; set; } = "Default";

        public virtual string GetVendorID()
        {
            return internalType;
        }

        public virtual string GetAddress()
        {
            return "??";
        }

        public virtual void Save(string folderPath)
        {
            return;
        }

        public virtual string GetName()
        {
            return "ZigBee";
        }

        public virtual void SetName(string name)
        {
            return;
        }

        public virtual Guid GetGuid()
        {
            return this.Guid;
        }

        public virtual void SetGuid(Guid guid)
        {
            this.Guid = guid;
        }
    }
}
