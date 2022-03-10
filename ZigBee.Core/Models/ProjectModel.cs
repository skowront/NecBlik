using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.Factories;
using ZigBee.Core.Models;

namespace ZigBee.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectModel
    {
        [JsonProperty]
        public string ProjectName { get; set; } = Resources.Resources.ProjectModelPojectNameDefault;
        [JsonProperty]
        public ZigBeeModel ZigBeeTemplate { get; set; } = new ZigBeeModel();
        [JsonProperty]
        public string MapFile { get; set; }

        public List<ZigBeeNetwork> ZigBeeNetworks { get; set; } = new List<ZigBeeNetwork>();

        public void Save(string projectFolder, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            updatableResponseProvider?.Init(0,1+this.ZigBeeNetworks.Count,0);
            var progress = 1;
            var dir = projectFolder + Resources.Resources.ZigBeeNetworksDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            updatableResponseProvider?.Update(progress);
            if (Directory.Exists(dir))
            {
                foreach (var ZigBeeNetwork in this.ZigBeeNetworks)
                {
                    ZigBeeNetwork.Save(dir);
                    progress++;
                    updatableResponseProvider?.Update(progress);
                }
            }
            updatableResponseProvider?.SealUpdates();
        }

        public async Task Load(string projectFolder, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            var dir = projectFolder + Resources.Resources.ZigBeeNetworksDirectory;
            if (!Directory.Exists(dir))
            {
                return;
            }
            else
            {
                var networkSubDirs = Directory.EnumerateDirectories(dir);
                foreach(var networkSubDir in networkSubDirs)
                {
                    var network = await ZigBeeAnyFactory.Instance.BuildNetworkFromDirectory(networkSubDir,updatableResponseProvider);
                    if(network!=null)
                    {
                        this.ZigBeeNetworks.Add(network);
                    }
                }
            }
        }
    }
}
