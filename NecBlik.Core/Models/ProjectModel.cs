using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Factories;
using NecBlik.Core.Models;

namespace NecBlik.Core.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectModel
    {
        [JsonProperty]
        public string ProjectName { get; set; } = Resources.Resources.ProjectModelPojectNameDefault;
        [JsonProperty]
        public DeviceModel ZigBeeTemplate { get; set; } = new DeviceModel();

        public List<Network> Networks { get; set; } = new List<Network>();

        public void Save(string projectFolder, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            updatableResponseProvider?.Init(0,1+this.Networks.Count,0);
            var progress = 1;
            var dir = projectFolder + Resources.Resources.DeviceNetworksDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            updatableResponseProvider?.Update(progress);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!Directory.Exists(dir))
                {
                    return;
                }
                foreach (var network in this.Networks)
                {
                    network.Save(dir);
                    progress++;
                    updatableResponseProvider?.Update(progress);
                }
            }
            updatableResponseProvider?.SealUpdates();
        }

        public async Task Load(string projectFolder, IUpdatableResponseProvider<int, bool, string> updatableResponseProvider)
        {
            var dir = projectFolder + Resources.Resources.DeviceNetworksDirectory;
            if (!Directory.Exists(dir))
            {
                return;
            }
            else
            {
                var networkSubDirs = Directory.EnumerateDirectories(dir);
                foreach(var networkSubDir in networkSubDirs)
                {
                    var network = await DeviceAnyFactory.Instance.BuildNetworkFromDirectory(networkSubDir,updatableResponseProvider);
                    if(network!=null)
                    {
                        this.Networks.Add(network);
                    }
                }
            }
        }
    }
}
