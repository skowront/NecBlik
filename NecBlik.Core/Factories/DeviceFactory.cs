using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;

namespace NecBlik.Core.Factories
{
    public abstract class DeviceFactory:IDeviceFactory
    {
        protected string internalFactoryType { get; set; } = Resources.Resources.AbstractFactoryId;

        protected List<IDeviceFactory> OtherFactories { get; set; } = new List<IDeviceFactory>();

        public DeviceFactory()
        {

        }

        public virtual string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public virtual IDeviceSource BuildNewSource()
        {
            return new Device();
        }

        public virtual Coordinator BuildCoordinator()
        {
            return new Coordinator();
        }

        public virtual Network BuildNetwork()
        {
            return new Network();
        }

        public virtual IDeviceSource BuildSourceFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            return new Device();
        }

        public virtual Coordinator BuildCoordinatorFromJsonFile(string pathToFile)
        {
            var path = Path.GetDirectoryName(pathToFile);
            var fileName = Path.GetFileName(pathToFile);
            var type = fileName.Split('.')[1];
            if (type != this.GetVendorID())
            {
                return null;
            }
            return new Coordinator();
        }

        public virtual async Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            if (pathToDirectory.Split('.').LastOrDefault() != this.GetVendorID())
            {
                return null;
            }
            return new Network();
        }

        public void AttachOtherFactories(List<IDeviceFactory> deviceFactories)
        {
            this.OtherFactories = deviceFactories;
        }

        public virtual void Initalize(object args = null)
        {
            
        }
    }
}
