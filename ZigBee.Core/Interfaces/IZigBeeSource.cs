using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Core.Interfaces
{
    public interface IZigBeeSource: IVendable, ICachable
    {
        public string GetAddress();
        public string GetName();
        public void SetName(string name);
        public void Save(string folderPath);
        public Guid GetGuid();
        public void SetGuid(Guid guid);
        public string GetVersion();
        public void OnDataRecieved(string data, string sourceAddress);
        public void SubscribeToDataRecieved(ISubscriber<Tuple<string,string>> subscriber);
        public void UnsubscribeFromDataRecieved(ISubscriber<Tuple<string, string>> subscriber);
        public void Send(string data, string address);
    }
}
