using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeSource: IVendable
    {
        public string GetAddress();
        public string GetName();
        public void SetName(string name);
        public void Save(string folderPath);
        public Guid GetGuid();
        public void SetGuid(Guid guid);
    }
}
