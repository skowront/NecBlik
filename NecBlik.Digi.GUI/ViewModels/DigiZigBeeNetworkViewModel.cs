using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Core.GUI;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.ViewModels;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiZigBeeNetworkViewModel : VirtualZigBeeNetworkViewModel
    {
        public DigiZigBeeNetworkViewModel(ZigBeeNetwork network) : base(network)
        {
        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            if (this.zigBeeCoorinator == null)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                this.zigBeeCoorinator = new DigiZigBeeCoordinatorViewModel(zvm, this);
                this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
                this.ZigBeeSelectionSubscriber?.NotifyUpdated(this.zigBeeCoorinator);
            }
            return this.zigBeeCoorinator;
        }

        public override void Close()
        {
            this.zigBeeCoorinator.Model.ZigBeeSource.Close();
        }

        public override bool Open()
        {
            return this.zigBeeCoorinator.Model.ZigBeeSource.Open();
        }

        public override void SyncFromModel()
        {
            this.ZigBees.Clear();

            foreach (var device in this.model.ZigBeeSources)
            {
                var vm = new DigiZigBeeViewModel(new ZigBeeModel(device), this);
                vm.PullSelectionSubscriber = this.ZigBeeSelectionSubscriber;
                this.ZigBeeSelectionSubscriber?.NotifyUpdated(vm);
                this.ZigBees.Add(vm);
            }

            if(this.model.HasCoordinator)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                this.zigBeeCoorinator = new DigiZigBeeCoordinatorViewModel(zvm, this);
                this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
                this.ZigBeeSelectionSubscriber?.NotifyUpdated(this.zigBeeCoorinator);
            }
        }

        public override async Task Discover()
        {
            await this.model.SyncCoordinator();
            this.SyncFromModel();
        }
    }
}
