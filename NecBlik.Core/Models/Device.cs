using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Enums;

namespace NecBlik.Core.Models
{
    public class Device : IDeviceSource
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        protected string internalType { get; set; } = Resources.Resources.DefaultFactoryId;

        protected string version { get; set; }

        public Collection<ISubscriber<RecievedData>> DataRecievedSubscribers = new Collection<ISubscriber<RecievedData>>();

        public virtual string GetVendorID()
        {
            return internalType;
        }

        public virtual string GetAddress()
        {
            return "??";
        }

        public virtual void Save(string folderPath)
        {
            return;
        }

        public virtual string GetName()
        {
            return Resources.Resources.GPDevice;
        }

        public virtual void SetName(string name)
        {
            return;
        }

        public virtual Guid GetGuid()
        {
            return this.Guid;
        }

        public virtual void SetGuid(Guid guid)
        {
            this.Guid = guid;
        }

        public virtual string GetVersion()
        {
            return this.version;
        }

        public virtual string GetCacheId()
        {
            return this.Guid.ToString();
        }

        public virtual void OnDataRecieved(string data, string sourceAddress)
        {
            foreach(var item in this.DataRecievedSubscribers)
            {
                item.NotifySubscriber(new RecievedData() { Data = data, SourceAddress = sourceAddress });
            }
            return;
        }

        public virtual void Send(string data, string address)
        {
            throw new NotImplementedException();
        }

        public virtual void SubscribeToDataRecieved(ISubscriber<RecievedData> subscriber)
        {
            if(!this.DataRecievedSubscribers.Contains(subscriber) && !this.DataRecievedSubscribers.Any((s) => { return s.GetCacheId() == subscriber.GetCacheId(); }))
                this.DataRecievedSubscribers.Add(subscriber);
        }

        public virtual void UnsubscribeFromDataRecieved(ISubscriber<RecievedData> subscriber)
        {
            var toRemove = new List<ISubscriber<RecievedData>>();
            for(int i = 0; i<this.DataRecievedSubscribers.Count; i++)
            {
                if (this.DataRecievedSubscribers[i] == subscriber || this.DataRecievedSubscribers[i].GetCacheId() == subscriber.GetCacheId())
                    toRemove.Add(this.DataRecievedSubscribers[i]);
            }
            while(toRemove.Count()>0)
                this.DataRecievedSubscribers.Remove(toRemove.First());
        }

        public virtual bool Open()
        {
            return true;
        }

        public virtual void Close()
        {
            return;
        }

        public virtual bool IsLicensed()
        {
            return false;
        }

        public virtual IEnumerable<string> GetLicensees()
        {
            return new List<string>();
        }

        public virtual void Dispose()
        {

        }

        public virtual void OnDataSent(string data, string sourceAddress)
        {
            this.OnDataRecieved(data,sourceAddress);
        }
    }
}
