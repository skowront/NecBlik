using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Factories
{
    public abstract class ZigBeeFactory:IZigBeeFactory
    {
        private string internalFactoryType { get; set; } = string.Empty;

        public ZigBeeFactory()
        {

        }

        public abstract string GetInternalFactoryType();

        public abstract IZigBeeSource Build();
    }
}
