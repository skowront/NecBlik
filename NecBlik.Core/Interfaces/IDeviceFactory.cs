using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Models;

namespace NecBlik.Core.Interfaces
{
    public interface IDeviceFactory: IVendable, IInitializable
    {
        void AttachOtherFactories(List<IDeviceFactory> deviceFactories);

        IDeviceSource BuildNewSource();

        IDeviceSource BuildSourceFromJsonFile(string pathToFile);

        Coordinator BuildCoordinator();

        Coordinator BuildCoordinatorFromJsonFile(string pathToFile);

        Network BuildNetwork();

        Task<Network> BuildNetworkFromDirectory(string pathToDirectory, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider);
    }
}
