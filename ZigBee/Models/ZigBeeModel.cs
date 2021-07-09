using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;

namespace ZigBee.Models
{
    public class ZigBeeModel: IDuplicable<ZigBeeModel>
    {
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; } = Resources.Resources.ZigBeeModelNameDefault;
        public string Version { get; set; } = Resources.Resources.ZigBeeVersionDefault;

        public ZigBeeModel Duplicate()
        {
            return new ZigBeeModel() { Guid = new Guid(), Name = this.Name, Version = this.Version };
        }
    }
}
