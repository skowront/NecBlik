using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Factories;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ZigBeeNetwork : IVendable
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
                if (this.HasCoordinator && this.ZigBeeCoordinator != null)
                {
                    return this.ZigBeeCoordinator.PanId;
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
        public Collection<FactoryRule> ZigBeesSubtypeFactoryRules {get;set;} = new Collection<FactoryRule>();
        [JsonProperty]
        public FactoryRule ZigBeeCoordinatorSubtypeFactoryRule { get; set; } = new FactoryRule();

        public Action CoordinatorChanged { get; set; } = null;

        public ZigBeeCoordinator ZigBeeCoordinator { get; private set; }

        public Collection<IZigBeeSource> ZigBeeSources { get; set; } = new Collection<IZigBeeSource>();

        public Collection<Tuple<string, string>> ZigBeeConnections { get; set; }

        public IZigBeeFactory ZigBeeFactory { get; set; } = new ZigBeeDefaultFactory();

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

        public ZigBeeNetwork()
        {
            this.internalNetworkType = ZigBeeFactory.GetVendorID();
        }

        public virtual async Task SetCoordinator(ZigBeeCoordinator zigBeeCoordinator)
        {
            this.ZigBeeCoordinator = zigBeeCoordinator;
            if(this.ZigBeeCoordinator!=null)
            {
                this.CoordinatorChanged?.Invoke();
                await this.SyncCoordinator();
                this.HasCoordinator = true;
            }
            else
            {
                this.HasCoordinator = false;
            }
        }

        public virtual async Task SyncCoordinator()
        {
            this.ZigBeeSources = new Collection<IZigBeeSource>((await this.ZigBeeCoordinator.GetDevices(ProgressResponseProvider)).ToList());
        }

        public virtual void AddSource(IZigBeeSource source)
        {
            this.ZigBeeSources.Add(source);
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
                    this.ZigBeeCoordinator.Save(dir);
                }
                File.WriteAllText(dir+"\\"+"Network.json",JsonConvert.SerializeObject(this,Formatting.Indented));
                var sourcesSubDir = dir + Resources.Resources.ZigBeeNetworkSourcesSubdirectory;
                if (!Directory.Exists(sourcesSubDir))
                {
                    Directory.CreateDirectory(sourcesSubDir);
                }
                if (Directory.Exists(sourcesSubDir))
                {
                    foreach (var item in this.ZigBeeSources)
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
    }
}
