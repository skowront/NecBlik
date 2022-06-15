using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.Enums;
using NecBlik.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Core.GUI.ViewModels
{
    public class PingViewModel:BaseViewModel
    {
        private PingModel model;

        private DateTime sendingTime = DateTime.MinValue;
        public DateTime SendingTime
        {
            get {  return this.sendingTime; }
            set { this.sendingTime = value; this.OnPropertyChanged(); }
        }

        public double ResponseTime
        {
            get { return model.ResponseTime; }
            set { model.ResponseTime = value; this.OnPropertyChanged(); }
        }

        public PingModel.PingResult Result
        {
            get { return model.Result; }
            set { model.Result = value; this.OnPropertyChanged(); }
        }

        public string ReturnedPayload
        {
            get { return model.Payload; }
            set { model.Payload = value; this.OnPropertyChanged(); }
        }

        public string Message
        {
            get { return model.Message; }
            set { model.Message = value; this.OnPropertyChanged(); }
        }

        IDeviceCoordinator coordinator = null;

        private int timeout = 0;
        public int Timeout {
           get { return timeout; }
           set { timeout = value; this.OnPropertyChanged(); }    
        }

        private int payloadSize = 0;
        public int PayloadSize
        {
            get { return payloadSize; } 
            set { payloadSize = value; this.OnPropertyChanged(); }
        }

        private string payload = string.Empty;
        public string Payload
        {
            get { return payload; }
            set { payload = value; this.OnPropertyChanged(); }
        }

        public string SelectedRemoteAddresss { get; set; }

        public ObservableCollection<string> AvailableAdresses { get; set; } = new ObservableCollection<string>();

        public RelayCommand RunCommand { get; set; }
        public RelayCommand RunPacketCommand { get; set; }

        public PingViewModel(IDeviceCoordinator coordinator, List<string> availableAddresses, PingModel model=null)
        {
            this.SelectedRemoteAddresss = availableAddresses.FirstOrDefault();
            if(model != null)
                this.model = model;
            else
                this.model = new PingModel();

            this.coordinator = coordinator;

            foreach(var item in availableAddresses)
            {
                this.AvailableAdresses.Add(item);
            }

            this.RunCommand = new RelayCommand(async (o) =>
            {
                this.SendingTime = DateTime.Now;
                PingModel result;
                if (this.PayloadSize > 0)
                {
                    result = await coordinator.Ping(this.timeout, RandomString(this.payloadSize), this.SelectedRemoteAddresss);
                }
                else
                {
                    result = await coordinator.Ping(this.timeout, this.Payload, this.SelectedRemoteAddresss);
                }
                this.ReturnedPayload = result.Payload;
                this.Message = result.Message;
                this.ResponseTime = result.ResponseTime;
                this.Result = result.Result;
            });

            this.RunPacketCommand = new RelayCommand(async (o) =>
            {
                this.SendingTime = DateTime.Now;
                PingModel result;
                if (this.PayloadSize > 0)
                {
                    result = await coordinator.PingPacket(this.timeout, RandomString(this.payloadSize), this.SelectedRemoteAddresss);
                }
                else
                {
                    result = await coordinator.PingPacket(this.timeout, this.Payload, this.SelectedRemoteAddresss);
                }
                this.ReturnedPayload = result.Payload;
                this.Message = result.Message;
                this.ResponseTime = result.ResponseTime;
                this.Result = result.Result;
            });
        }

        #region stackoverflow
        /// <summary>
        /// Author: https://stackoverflow.com/users/1364007/wai-ha-lee
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings"/>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Author: https://stackoverflow.com/users/1364007/wai-ha-lee
        /// <see cref="https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings"/>
        /// </summary>
        private static Random random = new Random();
        #endregion
    }
}
