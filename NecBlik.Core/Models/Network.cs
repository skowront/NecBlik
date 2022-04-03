using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Factories;
using NecBlik.Core.Interfaces;

namespace NecBlik.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Network : IVendable, IDisposable
    {
        [JsonProperty]
        public string Name { get; set; } = Resources.Resources.GPNetwork;
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [JsonProperty]
        public bool HasCoordinator { get; private set; } = false;
        [JsonProperty]
        private string panId { get; set; } = Guid.NewGuid().ToString();
        public string PanId
        {
            get
            {
                if (this.HasCoordinator && this.Coordinator != null)
                {
                    return this.Coordinator.PanId;
                }
                return panId;
            }
            set
            {
                return;
            }
        }

        [JsonProperty]
        public string InternalSubType { get; set; } = string.Empty;
        [JsonProperty]
        public Collection<FactoryRule> DeviceSubtypeFactoryRules {get;set;} = new Collection<FactoryRule>();
        [JsonProperty]
        public FactoryRule DeviceCoordinatorSubtypeFactoryRule { get; set; } = new FactoryRule();

        public Action CoordinatorChanged { get; set; } = null;

        public Coordinator Coordinator { get; private set; }

        public Collection<IDeviceSource> DeviceSources { get; set; } = new Collection<IDeviceSource>();

        public Collection<Tuple<string, string>> Connections { get; set; }

        public IDeviceFactory DeviceFactory { get; set; } = new DeviceDefaultFactory();

        protected string internalNetworkType { get; set; } = string.Empty;

        private IUpdatableResponseProvider<int, bool, string> progressResponseProvider;
        public IUpdatableResponseProvider<int, bool, string> ProgressResponseProvider
        {
            get { return progressResponseProvider; }
            set
            {
                this.progressResponseProvider = value;
            }
        }

        public Network()
        {
            this.internalNetworkType = DeviceFactory.GetVendorID();
        }

        public virtual async Task SetCoordinator(Coordinator coordinator)
        {
            this.Coordinator = coordinator;
            if(this.Coordinator!=null)
            {
                this.CoordinatorChanged?.Invoke();
                //await this.SyncCoordinator();
                this.HasCoordinator = true;
            }
            else
            {
                this.HasCoordinator = false;
            }
        }

        public virtual async Task SyncCoordinator()
        {
            this.DeviceSources = new Collection<IDeviceSource>((await this.Coordinator.GetDevices(ProgressResponseProvider)).ToList());
        }

        public virtual void AddSource(IDeviceSource source)
        {
            this.DeviceSources.Add(source);
        }

        public virtual void Save(string folderName)
        {
            var dir = folderName + "\\" + this.Guid + "." + this.internalNetworkType;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (Directory.Exists(dir))
            {
                if(this.HasCoordinator)
                {
                    this.Coordinator.Save(dir);
                }
                File.WriteAllText(dir+"\\"+"Network.json",JsonConvert.SerializeObject(this,Formatting.Indented));
                var sourcesSubDir = dir + Resources.Resources.DeviceNetworkSourcesSubdirectory;
                if (!Directory.Exists(sourcesSubDir))
                {
                    Directory.CreateDirectory(sourcesSubDir);
                }
                if (Directory.Exists(sourcesSubDir))
                {
                    foreach (var item in this.DeviceSources)
                    {
                        item.Save(sourcesSubDir);
                    }
                }
            }
        }

        public string GetVendorID()
        {
            return this.internalNetworkType;
        }

        public bool IsLicensed()
        {
            return false;
        }

        public IEnumerable<string> GetLicensees()
        {
            return new List<string>();
        }

        public virtual void Dispose()
        {

        }
    }
}
