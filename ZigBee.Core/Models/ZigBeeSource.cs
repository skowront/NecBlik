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
    }
}
