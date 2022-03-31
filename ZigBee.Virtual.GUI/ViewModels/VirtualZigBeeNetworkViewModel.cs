using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;
using ZigBee.Virtual.GUI.Factories;
using ZigBee.Virtual.GUI.Views;
using ZigBee.Virtual.Models;

namespace ZigBee.Virtual.GUI.ViewModels
{
    public class VirtualZigBeeNetworkViewModel : ZigBeeNetworkViewModel
    {
        public ObservableCollection<VirtualZigBeeViewModel> ZigBees { get; set; } = new ObservableCollection<VirtualZigBeeViewModel>();

        public VirtualZigBeeNetworkViewModel(ZigBeeNetwork network) : base(network)
        {

            this.EditResponseProvider = new GenericResponseProvider<string, ZigBeeNetworkViewModel>((q) => {
                Window window = new VirtualZigBeeNetworkWindow(this);
                window.Show();
                return null;
            });

            this.SyncFromModel();
        }

        public virtual void SyncFromModel()
        {
            this.ZigBees.Clear();
           
            foreach (var device in this.model.ZigBeeSources)
            {
                var factory = new VirtualZigBeeGuiFactory();
                var vm = factory.ZigBeeViewModelFromRules(new ZigBeeModel(device), this, this.model.ZigBeesSubtypeFactoryRules.ToList());
                vm.PullSelectionSubscriber = this.ZigBeeSelectionSubscriber;
                this.ZigBees.Add(vm);
            }
            this.GetZigBeeCoordinatorViewModel();

            this.OnPropertyChanged();
        }

        public override ZigBeeViewModel GetZigBeeCoordinatorViewModel()
        {
            if (this.zigBeeCoorinator == null)
            {
                var zvm = new ZigBeeModel(this.Model.ZigBeeCoordinator);
                var factory = new VirtualZigBeeGuiFactory();
                var vm = factory.ZigBeeViewModelFromRule(zvm, this, this.model.ZigBeeCoordinatorSubtypeFactoryRule);
                this.zigBeeCoorinator = vm;
            }
            this.zigBeeCoorinator.PullSelectionSubscriber = ZigBeeSelectionSubscriber;
            this.ZigBeeSelectionSubscriber?.NotifyUpdated(this.zigBeeCoorinator);
            return this.zigBeeCoorinator;
        }

        public override IEnumerable<ZigBeeViewModel> GetZigBeeViewModels()
        {
            return this.ZigBees;
        }

        public override void Sync()
        {
            base.Sync();
            this.SyncFromModel();
        }
    }
}
