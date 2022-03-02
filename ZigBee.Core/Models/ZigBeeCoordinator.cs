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
        private string internalType { get; set; } = "Default";

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

        public virtual string GetPanID()
        {
            throw new NotImplementedException();
        }

        public virtual void Save(string folderPath)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public void SetName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
