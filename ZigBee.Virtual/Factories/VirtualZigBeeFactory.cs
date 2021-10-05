using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Factories;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;
using ZigBee.Virtual.Models;

namespace ZigBee.Virtual.Factories
{
    public class VirtualZigBeeFactory : ZigBeeFactory
    {
        public override string GetInternalFactoryType()
        {
            return "Virtual";
        }

        public override IZigBeeSource Build()
        {
            return new VirtualZigBeeSource();
        }

        public override IZigBeeCoordinator BuildCoordinator()
        {
            return new VirtualZigBeeCoordinator(this);
        }

        public override ZigBeeNetwork BuildNetwork()
        {
            return new VirtualZigBeeNetwork();
        }
    }
}
