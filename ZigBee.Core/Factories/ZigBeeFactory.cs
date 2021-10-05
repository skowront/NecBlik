using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.Factories
{
    public class ZigBeeFactory:IZigBeeFactory
    {
        private string internalFactoryType { get; set; } = "Default";

        public ZigBeeFactory()
        {

        }

        public virtual string GetInternalFactoryType()
        {
            return this.internalFactoryType;
        }

        public virtual IZigBeeSource Build()
        {
            return new ZigBeeSource();
        }

        public virtual IZigBeeCoordinator BuildCoordinator()
        {
            return new ZigBeeCoordinator();
        }

        public virtual ZigBeeNetwork BuildNetwork()
        {
            return new ZigBeeNetwork();
        }
    }
}
