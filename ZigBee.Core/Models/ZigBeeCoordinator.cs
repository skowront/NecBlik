using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    public class ZigBeeCoordinator: IZigBeeCoordinator
    {
        protected string internalType { get; set; } = "Default";

        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [JsonProperty]
        protected string Name { get; set; } = "ZigBee Coordingator";

        public ZigBeeCoordinator()
        {

        }

        public virtual string GetVendorID()
        {
            return this.internalType;
        }

        public virtual string GetAddress()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Tuple<string, string>> GetConnections()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IZigBeeSource> GetDevices()
        {
            throw new NotImplementedException();
        }
        public virtual void SetDevices(IEnumerable<IZigBeeSource> sources)
        {
            return;
        }

        public virtual string GetPanID()
        {
            throw new NotImplementedException();
        }

        public virtual void Save(string folderPath)
        {
            throw new NotImplementedException();
        }

        public virtual string GetName()
        {
            return this.Name;
        }

        public virtual void SetName(string name)
        {
            this.Name = name;
            return;
        }

        public virtual Guid GetGuid()
        {
            return this.Guid;
        }

        public virtual void SetGuid(Guid guid)
        {
            this.Guid = guid;
            return;
        }

        public virtual string GetVersion()
        {
            return "??";
        }
    }
}
