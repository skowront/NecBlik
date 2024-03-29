﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Enums;

namespace NecBlik.Core.Models
{
    public class Coordinator: IDeviceCoordinator
    {
        protected string internalType { get; set; } = Resources.Resources.DefaultFactoryId;
        protected string internalSubType { get; set; } = string.Empty;

        [JsonProperty]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [JsonProperty]
        protected string Name { get; set; } = Resources.Resources.GPDeviceCoordinator;

        protected string panId = Resources.Resources.DeviceCoordinatorPanIdDefault;
        [JsonProperty]
        public string PanId
        {
            get { return this.GetPanID(); }
            set { this.panId = value; }
        }

        public Collection<ISubscriber<RecievedData>> DataRecievedSubscribers = new Collection<ISubscriber<RecievedData>>();

        protected IDeviceSource DeviceSource { get; set; }

        public Coordinator()
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

        public virtual async Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            throw new NotImplementedException();
        }
        public virtual void SetDevices(IEnumerable<IDeviceSource> sources)
        {
            return;
        }

        public virtual string GetPanID()
        {
            return this.panId;
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

        public virtual string GetCacheId()
        {
            return this.Guid.ToString();
        }

        public virtual async Task Discover()
        {

        }

        public virtual void OnDataRecieved(string data, string sourceAddress)
        {
            foreach (var item in this.DataRecievedSubscribers)
            {
                item?.NotifySubscriber(new RecievedData() { Data = data, SourceAddress = sourceAddress});
            }
            return;
        }

        public virtual void Send(string data, string address)
        {
            this.DeviceSource.Send(data, address);
        }

        public virtual void SubscribeToDataRecieved(ISubscriber<RecievedData> subscriber)
        {
            if (!this.DataRecievedSubscribers.Contains(subscriber))
                this.DataRecievedSubscribers.Add(subscriber);
        }

        public virtual void UnsubscribeFromDataRecieved(ISubscriber<RecievedData> subscriber)
        {
            if (this.DataRecievedSubscribers.Contains(subscriber))
                this.DataRecievedSubscribers.Remove(subscriber);
        }

        public virtual bool Open()
        {
            return true;
        }

        public virtual void Close()
        {
            return;
        }

        public bool IsLicensed()
        {
            return false;
        }

        public IEnumerable<string> GetLicensees()
        {
            return new List<string>();
        }

        public virtual void Dispose()
        {

        }

        public virtual void OnDataSent(string data, string sourceAddress)
        {
            this.OnDataRecieved(data, sourceAddress);
        }

        public virtual async Task<PingModel> Ping(long timeout = 0, string payload = "", string remoteAddress="")
        {
            return new PingModel(0.0d, PingModel.PingResult.Ok, payload);
        }

        public virtual async Task<PingModel> PingPacket(long timeout = 0, string payload = "", string remoteAddress = "", bool awaitConfirmation = true)
        {
            return new PingModel(0.0d, PingModel.PingResult.Ok, payload);
        }

        public virtual async Task<string> GetStatusOf(string remoteAddress)
        {
            return NecBlik.Core.Resources.Statuses.Unknown;
        }

        public virtual async Task<(double localStrength, double remoteStrength)> GetSignalStrength(string remoteAddress)
        {
            double l = double.PositiveInfinity, r = double.PositiveInfinity;
            return (l, r);
        }
    }
}
