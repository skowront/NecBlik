using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Models;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeFactory: IVendable
    {
        void AttachOtherFactories(List<IZigBeeFactory> zigBeeFactories);

        IZigBeeSource BuildNewSource();

        IZigBeeSource BuildSourceFromJsonFile(string pathToFile);

        IZigBeeCoordinator BuildCoordinator();

        IZigBeeCoordinator BuildCoordinatorFromJsonFile(string pathToFile);

        ZigBeeNetwork BuildNetwork();

        ZigBeeNetwork BuildNetworkFromDirectory(string pathToDirectory);
    }
}
