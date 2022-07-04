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
    public class ThroughputViewModel : BaseViewModel, ISubscriber<RecievedData>, IDisposable
    {
        DigiZigBeeNetworkViewModel networkViewModel;

        public int MinPayloadSize { get; set; } = 0;
        public int MaxPayloadSize { get; set; } = 254;

        private int payloadDesiredSize;
        public int PayloadDesiredSize
        {
            get { return payloadDesiredSize; }
            set { payloadDesiredSize = value; this.PrepareTest(); this.OnPropertyChanged(); }
        }

        private double throughputValue = 0.0;
        public double ThroughputValue
        {
            get { return throughputValue; }
            set
            {
                throughputValue = value;
                this.throughputHistory.Add(value);
                this.Mean = this.throughputHistory.Sum() / (this.throughputHistory.Count == 0 ? 1 : this.throughputHistory.Count);
                this.OnPropertyChanged();
            }
        }

        private List<double> throughputHistory = new List<double>();
        private double mean = 0.0;
        public double Mean
        {
            get { return Math.Round(this.mean, 2); }
            set { mean = value; this.OnPropertyChanged(); }
        }

        private string unit = "bps";
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

        private DateTime lastPoll;
        public DateTime LastPoll {
            get { return lastPoll; }
            set { this.lastPoll = value; this.OnPropertyChanged(); }
        }

        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; this.OnPropertyChanged(); }
        }

        private int sent = 0;
        private int lost = 0;
        public  string SentLost
        {
            get { return $"{this.sent}/{this.lost}"; }
            set { this.OnPropertyChanged(); }
        }

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

        public ThroughputViewModel(DigiZigBeeNetworkViewModel networkViewModel)
        {
            this.networkViewModel = networkViewModel;
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += DoWork;

            this.StartCommand = new RelayCommand((o) =>
            {
                this.IsRunning = true;
                this.PrepareTest();
                this.networkViewModel.Hold(this.CacheId);
                backgroundWorker.RunWorkerAsync();
            });

            this.StopCommand = new RelayCommand((o) =>
            {
                this.cancellationRequested = true;
                backgroundWorker.CancelAsync();
                this.throughputHistory.Clear();
                this.networkViewModel.Model.Coordinator?.UnsubscribeFromDataRecieved(this);
                this.networkViewModel.Unhold(this.CacheId);
                this.IsRunning = false;
            });
        }

        ~ThroughputViewModel()
        {
            this.Dispose();
        }

        private void PrepareTest()
        {
            this.ToSend.Clear();
            this.Sent.Clear();
            this.SendingIterator = 0;
            for (int i = 0; i < PreparedPackets; i++)
            {
                var s = "";
                s += i.ToString();
                for (int j = 0; j < PayloadDesiredSize - i.ToString().Length - 1; j++)
                {
                    s += 'x';
                }
                this.ToSend.Add(s);
            }
            this.networkViewModel.Model.Coordinator?.SubscribeToDataRecieved(this);
            this.PreparationTime = DateTime.Now;
            this.LastUpdateTime = null;
            this.cancellationRequested = false;
            this.ThroughputValue = 0;
            this.throughputHistory.Clear();
            this.sent = 0;
            this.lost = 0;
        }

        public async void DoWork(object sender, DoWorkEventArgs e)
        {
            while (!cancellationRequested)
            {
                if (SendingIterator >= this.ToSend.Count)
                    this.PrepareTest();
                var pm = await this.networkViewModel.Model.Coordinator.PingPacket(0, this.ToSend[SendingIterator], this.deviceAddress, true);
                if(pm.Result == Core.Enums.PingModel.PingResult.NotOk)
                {
                    lost++;
                }
                sent++;
                this.SentLost = String.Empty;
                this.Sent.Add(this.ToSend[SendingIterator]);
                this.NotifySubscriber(new RecievedData() { Data = this.ToSend[SendingIterator], SourceAddress = this.DeviceAddress });
                SendingIterator++;
            }
        }

        public void NotifySubscriber(RecievedData updateInformation)
        {
            const int interval = 1000;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (LastUpdateTime == null)
                {
                    LastUpdateTime = DateTime.Now;
                }
                if (LastUpdateTime == null || (DateTime.Now) > (LastUpdateTime.Value.AddMilliseconds(interval)))
                {
                    var dataRecieved = 0;
                    foreach (var item in this.Sent)
                    {
                        dataRecieved += item.Length;
                    }
                    this.ThroughputValue = dataRecieved;
                    this.Sent.Clear();
                    LastUpdateTime = DateTime.Now;
                    this.LastPoll = LastUpdateTime??DateTime.Now;
                }
            });
        }

        public string GetCacheId()
        {
            return this.CacheId.ToString();
        }

        public void Dispose()
        {
            this.StopCommand.Execute(null);
        }
    }
}
