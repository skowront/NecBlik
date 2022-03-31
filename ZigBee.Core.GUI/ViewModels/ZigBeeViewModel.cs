using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI
{
    public class ZigBeeViewModel:BaseViewModel,IDuplicable<ZigBeeViewModel>,ICachable,IVendable, ISubscriber<Tuple<string, string>>
    {
        public ZigBeeModel Model;

        public ZigBeeNetworkViewModel Network;

        public Guid Guid
        {
            get { return this.Model.Guid; }
        }

        public string Name
        {
            get { return this.Model.Name; }
            set { this.Model.Name = value; this.OnPropertyChanged(); }
        }

        public string Version
        {
            get { return this.Model.Version; }
            set { this.Model.Version = value; this.OnPropertyChanged(); }
        }

        public string InternalFactoryType
        {
            get 
            {
                if (this.Model.ZigBeeSource != null)
                    return this.Model.ZigBeeSource.GetVendorID();
                return this.Model.InternalFactoryType; 
            }
            set { this.Model.InternalFactoryType = value; this.OnPropertyChanged(); }
        }

        public string Address
        {
            get { return this.Model.AddressName; }
        }

        public string CacheId
        {
            get { return this.Model.CacheId; }
        }

        private string outputBuffer = string.Empty;
        public string OutputBuffer
        {
            get { return this.outputBuffer; }
            set { this.outputBuffer = value; this.OnPropertyChanged(); }
        }

        private string ioHistoryBuffer;
        public string IOHistoryBuffer
        {
            get { return this.ioHistoryBuffer; }
            set { this.ioHistoryBuffer = value; this.OnPropertyChanged(); }
        }

        private string selectedDestinationAddress=string.Empty;
        public string SelectedDestinationAddress
        {
            get { return this.selectedDestinationAddress; }
            set { this.selectedDestinationAddress = value; this.OnPropertyChanged(); }
        }

        public IEnumerable<string> AvailableDestinationAddresses
        {
            get { return this.GetAvailableDestinationAdresses(); }
        }

        public ISelectionSubscriber<ZigBeeViewModel> PullSelectionSubscriber { get; set; }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }
        public RelayCommand SendCommand { get; set; }

        public ZigBeeViewModel(ZigBeeModel model = null, ZigBeeNetworkViewModel networkModel = null)
        {
            this.Network = networkModel;
            this.Model = model;
            if(this.Model == null)
            {
                this.Model = new ZigBeeModel();
                return;
            }
            this.Model.ZigBeeSource?.SubscribeToDataRecieved(this);
            this.BuildCommands();
        }

        public ZigBeeViewModel Duplicate()
        {
            var zb = new ZigBeeViewModel(this.Model.Duplicate(),this.Network);
            zb.SelectCommand = this.SelectCommand = new RelayCommand((o) => {
                this.PullSelectionSubscriber?.NotifySelected(zb);
            });
            return zb;
        }

        protected virtual void BuildCommands()
        {
            this.EditCommand = new RelayCommand((o) => { });
            this.SelectCommand = new RelayCommand((o) => { 
                this.PullSelectionSubscriber?.NotifySelected(this); 
            });
            this.SendCommand = new RelayCommand((o) => { 
                this.Send(); 
            });
        }

        public virtual string GetCacheId()
        {
            return this.Guid.ToString();
        }

        public virtual string GetVendorID()
        {
            return this.InternalFactoryType;
        }

        public virtual IEnumerable<string> GetAvailableDestinationAdresses()
        {
            var ret = new List<string>(this.Network.Model.ZigBeeSources.Select(x => { return x.GetAddress(); }));
            if(this.Network.Model.HasCoordinator)
            {
                ret.Add(this.Network.Model.ZigBeeCoordinator.GetAddress());
            }
            ret.Add(Strings.SR.Broadcast);
            return ret;
        }

        public virtual void OnDataRecieved(string data, string sourceAddress)
        {
            this.AddIncomingHistoryBufferEntry(data, sourceAddress);
        }

        public virtual void Send()
        {
            
            var sources = this.Network.GetZigBeeViewModels();
            if (this.SelectedDestinationAddress==Strings.SR.Broadcast)
            {
                foreach (var source in sources)
                {
                    source.Model.ZigBeeSource.OnDataRecieved(this.OutputBuffer,this.Address);
                    this.AddOutgoingHistoryBufferEntry(this.outputBuffer, source.Address);
                }
                this.Network.ZigBeeCoordinator.OnDataRecieved(this.outputBuffer, this.Address);
            }
            else
            {
                bool sent = false;
                foreach (var source in sources)
                {
                    if (source.Address == this.SelectedDestinationAddress)
                    {
                        source.Model.ZigBeeSource.OnDataRecieved(this.OutputBuffer, this.Address);
                        this.AddOutgoingHistoryBufferEntry(this.outputBuffer, source.Address);
                        sent = true;
                        break;
                    }
                }
                if(sent == false && this.Network.Model.HasCoordinator)
                {
                    if(this.Network.Model.ZigBeeCoordinator.GetAddress()==this.SelectedDestinationAddress)
                    {
                        this.Network.ZigBeeCoordinator.Model.ZigBeeSource.OnDataRecieved(this.OutputBuffer, this.Address);
                        this.AddOutgoingHistoryBufferEntry(this.outputBuffer, this.Address);
                    }
                }
            }
            this.OutputBuffer = string.Empty;
        }

        protected virtual void AddIncomingHistoryBufferEntry(string dataRecieved, string sourceAddress)
        {
            this.IOHistoryBuffer += Strings.SR.GPFrom + ": " + sourceAddress + " " + Strings.SR.GPRecieved + ":" + dataRecieved + "\n";
        }

        protected virtual void AddOutgoingHistoryBufferEntry(string dataSent, string destinationAddress)
        {
            this.IOHistoryBuffer += Strings.SR.GPTo + ": " + destinationAddress + " " + Strings.SR.GPSent + ":" + dataSent + "\n";
        }

        public virtual void NotifySubscriber(Tuple<string, string> updateInformation)
        {
            this.OnDataRecieved(updateInformation.Item1, updateInformation.Item2);
        }
    }
}