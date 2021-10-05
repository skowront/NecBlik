using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.Factories
{
    public class ZigBeeFactory:IZigBeeFactory
    {
        protected string internalFactoryType { get; set; } = "Default";

        protected List<IZigBeeFactory> OtherFactories { get; set; } = new List<IZigBeeFactory>();

        public ZigBeeFactory()
        {

        }

        public virtual string GetInternalFactoryType()
        {
            return this.internalFactoryType;
        }

        public virtual IZigBeeSource BuildNewSource()
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

        public virtual IZigBeeSource BuildSourceFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetInternalFactoryType())
            {
                return null;
            }
            return new ZigBeeSource();
        }

        public virtual IZigBeeCoordinator BuildCoordinatorFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetInternalFactoryType())
            {
                return null;
            }
            return new ZigBeeCoordinator();
        }

        public virtual ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetInternalFactoryType())
            {
                return null;
            }
            return new ZigBeeNetwork();
        }

        public void AttachOtherFactories(List<IZigBeeFactory> zigBeeFactories)
        {
            this.OtherFactories = zigBeeFactories;
        }
    }
}
