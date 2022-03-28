using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeFactory: IVendable, IInitializable
    {
        void AttachOtherFactories(List<IZigBeeFactory> zigBeeFactories);

        IZigBeeSource BuildNewSource();

        IZigBeeSource BuildSourceFromJsonFile(string pathToFile);

        ZigBeeCoordinator BuildCoordinator();

        ZigBeeCoordinator BuildCoordinatorFromJsonFile(string pathToFile);

        ZigBeeNetwork BuildNetwork();

        Task<ZigBeeNetwork> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider);
    }
}
