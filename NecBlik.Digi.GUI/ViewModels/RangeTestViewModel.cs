using NecBlik.Common.WpfExtensions.Base;
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
    public class RangeTestViewModel : BaseViewModel, IDisposable
    {
        DigiZigBeeNetworkViewModel networkViewModel;

        private double localRange = 0.0;
        public double LocalRange
        {
            get { return localRange; }
            set
            {
                localRange = value;
                this.OnPropertyChanged();
            }
        }

        private double remoteRange = 0.0;
        public double RemoteRange
        {
            get { return remoteRange; }
            set
            {
                remoteRange = value;
                this.OnPropertyChanged();
            }
        }

        private string unit = "dBm";
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

        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; this.OnPropertyChanged(); }
        }

        private DateTime lastPoll;
        public DateTime LastPoll
        {
            get { return lastPoll; }
            set { this.lastPoll = value; this.OnPropertyChanged(); }
        }


        public RelayCommand StartCommand { get; set; }
        public RelayCommand StopCommand { get; set; }

        public BackgroundWorker backgroundWorker { get; set; }

        private DateTime PreparationTime;
        private DateTime? LastUpdateTime = null;
        private bool cancellationRequested = false;

        public RangeTestViewModel(DigiZigBeeNetworkViewModel networkViewModel)
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
                this.networkViewModel.Unhold(this.CacheId);
                this.IsRunning = false;
            });
        }

        ~RangeTestViewModel()
        {
            this.Dispose();
        }

        private void PrepareTest()
        {
            this.PreparationTime = DateTime.Now;
            this.LastUpdateTime = null;
            this.cancellationRequested = false;
            this.LocalRange = 0;
            this.RemoteRange = 0;
        }

        public async void DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime last = DateTime.Now;
            while (!cancellationRequested)
            {
                if(last.AddMilliseconds(1000)<DateTime.Now)
                {
                    var pm = await this.networkViewModel.Model.Coordinator.GetSignalStrength(this.DeviceAddress);
                    this.RangeValueRecieved(pm.localStrength, pm.remoteStrength);
                    last = DateTime.Now;
                    this.LastPoll = last;
                }
            }
        }

        public void RangeValueRecieved(double localValue, double remoteValue)
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
                    this.LocalRange = localValue;
                    this.RemoteRange = remoteValue;
                    LastUpdateTime = DateTime.Now;
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
