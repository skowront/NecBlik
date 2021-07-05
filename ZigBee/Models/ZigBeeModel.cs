using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Models
{
    public class ZigBeeModel
    {
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; } = Resources.Resources.ZigBeeModelNameDefault;
        public string Version { get; set; } = Resources.Resources.ZigBeeVersionDefault;
    }
}
