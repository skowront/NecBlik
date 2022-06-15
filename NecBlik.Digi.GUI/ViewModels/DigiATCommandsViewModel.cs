using NecBlik.Common.WpfExtensions.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiATCommandsViewModel : BaseViewModel
    {
        public DigiZigBeeNetworkViewModel Network { get; set; }

        public DigiATCommandViewModel ATCommandViewModel {get;set;}

        public BindingList<string> IOHistory { get; set; } = new BindingList<string>();

        public string Address
        {
            get { return this.ATCommandViewModel.Address; }
            set { this.ATCommandViewModel.Address = value; }
        }

        public string Command
        {
            get { return this.ATCommandViewModel.Command; }
            set { this.ATCommandViewModel.Command = value; }
        }

        public string Parameter
        {
            get { return this.ATCommandViewModel.Parameter; }
            set { this.ATCommandViewModel.Parameter = value; }
        }

        public IEnumerable<string> AvailableAddresses
        {
            get { return this.Network.Coordinator?.AvailableDestinationAddresses.Where((e) => { return e != Core.GUI.Strings.SR.Broadcast; }) ?? new List<string>(); }
        }

        public RelayCommand SendCommand { get; set; }

        public DigiATCommandsViewModel(DigiZigBeeNetworkViewModel Network)
        {
            this.Network = Network;
            this.ATCommandViewModel = new DigiATCommandViewModel();

            this.SendCommand = new RelayCommand((o) =>
            {
                if(this.Network.Model.HasCoordinator)
                if(this.Network.Coordinator is DigiZigBeeCoordinatorViewModel)
                {
                    this.IOHistory.Add((this.Network.Coordinator as DigiZigBeeCoordinatorViewModel)?.SendAtCommand(this.ATCommandViewModel));
                }
            });
        }
    }
}
