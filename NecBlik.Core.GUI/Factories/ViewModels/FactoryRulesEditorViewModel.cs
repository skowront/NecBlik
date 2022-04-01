using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;

namespace NecBlik.Core.GUI.Factories.ViewModels
{
    public class FactoryRulesEditorViewModel : BaseViewModel
    {
        public IResponseProvider<List<string>, FactoryRule> AvailableCacheObjectIDsProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailablePropertyProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailableValueProvider { get; set; }

        public ObservableCollection<FactoryRuleViewModel> FactoryRules { get; set; }

        public RelayCommand RemoveRuleCommand { get; set; }
        public RelayCommand AddRuleCommand { get; set; }
        public RelayCommand ClearRulesCommand { get; set; }

        public FactoryRulesEditorViewModel(IResponseProvider<ObservableCollection<FactoryRuleViewModel>,object> responseProvider)
        {
            this.FactoryRules = responseProvider.ProvideResponse();

            this.RemoveRuleCommand = new RelayCommand((o) =>
            {
                this.FactoryRules.Remove(o as FactoryRuleViewModel);
            });

            this.AddRuleCommand = new RelayCommand((o) =>
            {
                var vm = new FactoryRuleViewModel(new FactoryRule());
                vm.AvailablePropertyProvider = this.AvailablePropertyProvider;
                vm.AvailableValueProvider = this.AvailableValueProvider;
                vm.AvailableCacheObjectIDsProvider = this.AvailableCacheObjectIDsProvider; 
                this.FactoryRules.Add(vm);
            });

            this.ClearRulesCommand = new RelayCommand((o) =>
            {
                this.FactoryRules.Clear();
            });
        }
    }
}
