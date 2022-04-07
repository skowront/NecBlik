using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.Interfaces
{
    public interface IDeviceSource: IVendable, ICachable, IDisposable
    {
        public string GetAddress();
        public string GetName();
        public void SetName(string name);
        public void Save(string folderPath);
        public Guid GetGuid();
        public void SetGuid(Guid guid);
        public string GetVersion();
        public void OnDataRecieved(string data, string sourceAddress);
        public void OnDataSent(string data, string sourceAddress);
        public void SubscribeToDataRecieved(ISubscriber<RecievedData> subscriber);
        public void UnsubscribeFromDataRecieved(ISubscriber<RecievedData> subscriber);
        public void Send(string data, string address);
        public bool Open();
        public void Close();
    }
}
