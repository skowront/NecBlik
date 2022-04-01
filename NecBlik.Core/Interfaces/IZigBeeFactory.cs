using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Models;

namespace NecBlik.Core.Interfaces
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
