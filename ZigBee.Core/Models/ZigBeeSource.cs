﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;

namespace ZigBee.Core.Models
{
    public class ZigBeeSource : IZigBeeSource
    {
        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();
        protected string internalType { get; set; } = Resources.Resources.DefaultFactoryId;

        protected string version { get; set; }

        public Collection<ISubscriber<Tuple<string, string>>> DataRecievedSubscribers = new Collection<ISubscriber<Tuple<string, string>>>();

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
            return Resources.Resources.GPZigBee;
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
                item.NotifySubscriber(new Tuple<string,string>(data,sourceAddress));
            }
            return;
        }

        public virtual void Send(string data, string address)
        {
            throw new NotImplementedException();
        }

        public virtual void SubscribeToDataRecieved(ISubscriber<Tuple<string, string>> subscriber)
        {
            if(!this.DataRecievedSubscribers.Contains(subscriber))
                this.DataRecievedSubscribers.Add(subscriber);
        }

        public virtual void UnsubscribeFromDataRecieved(ISubscriber<Tuple<string, string>> subscriber)
        {
            if (this.DataRecievedSubscribers.Contains(subscriber))
                this.DataRecievedSubscribers.Remove(subscriber);
        }
    }
}
