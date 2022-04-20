using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiNetworkDiagnosticsViewModel: BaseViewModel
    {
        public List<string> AvailableDevices
        {
            get { return new List<string>(this.NetworkViewModel.Devices.Select((x) => { return x.Address; })); }
        }

        public DigiZigBeeNetworkViewModel NetworkViewModel
        {
            get;set;
        }

        public ThroughputViewModel ThroughputVM
        {
            get; set;
        }

        public DigiNetworkDiagnosticsViewModel(DigiZigBeeNetworkViewModel networkViewModel)
        {
            this.NetworkViewModel = networkViewModel;
            this.ThroughputVM = new ThroughputViewModel(networkViewModel);
        }

        public class ThroughputViewModel : BaseViewModel, ISubscriber<NecBlik.Core.Models.RecievedData>
        {
            DigiZigBeeNetworkViewModel networkViewModel;

            public int MinPayloadSize {get;set;} = 7;
            public int MaxPayloadSize {get;set;} = 94;

            private int payloadDesiredSize;
            public int PayloadDesiredSize
            {
                get { return payloadDesiredSize;}
                set { payloadDesiredSize = value; this.OnPropertyChanged(); }
            }

            private double throughputValue = 0.0;
            public double ThroughputValue
            {
                get { return throughputValue; }
                set { throughputValue = value; this.OnPropertyChanged(); }
            }

            private string unit = "kbps";
            public string Unit
            {
                get { return unit; }
                set { unit = value; this.OnPropertyChanged(); }
            }

            private string deviceAddress = string.Empty;

            public string DeviceAddress
            {
                get { return deviceAddress; }
                set { deviceAddress = value; this.OnPropertyChanged(); }
            }

            private Guid CacheId = Guid.NewGuid();

            public RelayCommand StartCommand { get; set; }
            public RelayCommand StopCommand { get; set; }

            public BackgroundWorker backgroundWorker { get; set; }

            private DateTime PreparationTime;
            private DateTime? LastUpdateTime = null;
            private const int PreparedPackets = 1000;
            private int SendingIterator = 0;
            private bool cancellationRequested = false;
            public List<string> ToSend = new List<string>();
            public List<string> Sent = new List<string>();
            public List<string> Recieved = new List<string>();

            public ThroughputViewModel(DigiZigBeeNetworkViewModel networkViewModel)
            {
                this.networkViewModel = networkViewModel;
                this.backgroundWorker = new BackgroundWorker();
                this.backgroundWorker.WorkerSupportsCancellation = true;
                this.backgroundWorker.DoWork += DoWork;

                this.StartCommand = new RelayCommand((o) =>
                {
                    this.PrepareTest();
                    this.networkViewModel.Hold();
                    backgroundWorker.RunWorkerAsync();
                });

                this.StopCommand = new RelayCommand((o) =>
                {
                    this.cancellationRequested = true;
                    backgroundWorker.CancelAsync();
                    this.networkViewModel.Model.Coordinator?.UnsubscribeFromDataRecieved(this);
                    this.networkViewModel.Unhold();
                });
            }

            private void PrepareTest()
            {
                const int XBeeMaxPayload = 94;
                this.ToSend.Clear();
                this.Sent.Clear();
                this.Recieved.Clear();
                this.SendingIterator = 0;
                for (int i = 0; i < PreparedPackets; i++)
                {
                    var s = "Echo";
                    s += i.ToString();
                    for(int j = 0; j<XBeeMaxPayload-i.ToString().Length-"Echo".Length;j++)
                    {
                        s += ':';
                    }
                    this.ToSend.Add(s);
                }
                this.networkViewModel.Model.Coordinator?.SubscribeToDataRecieved(this);
                this.PreparationTime = DateTime.Now;
                this.LastUpdateTime = null;
                this.cancellationRequested = false;   
            }

            public void DoWork(object sender, DoWorkEventArgs e)
            {
                while(!cancellationRequested)
                {
                    if (SendingIterator >= this.ToSend.Count)
                        this.PrepareTest();
                    this.networkViewModel.Model.Coordinator?.Send(this.ToSend[SendingIterator], this.deviceAddress);
                    this.Sent.Add(this.ToSend[SendingIterator]);
                    SendingIterator++;
                }
            }

            public void NotifySubscriber(RecievedData updateInformation)
            {
                const int interval = 1000;
                this.Recieved.Add(updateInformation.Data);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if(LastUpdateTime == null)
                    {
                        LastUpdateTime = DateTime.Now;
                    }
                    if(LastUpdateTime == null||(DateTime.Now) > (LastUpdateTime.Value.AddMilliseconds(interval)))
                    {
                        var dataRecieved = 0;
                        foreach(var item in this.Recieved)
                        {
                            if(this.Sent.Contains(item))
                            {
                                dataRecieved += item.Length;
                            }
                        }
                        this.ThroughputValue = dataRecieved/1000;
                        this.Recieved.Clear();
                        LastUpdateTime = DateTime.Now;
                    }
                });
            }

            public string GetCacheId()
            {
                return this.CacheId.ToString();   
            }
        }
    }
}
