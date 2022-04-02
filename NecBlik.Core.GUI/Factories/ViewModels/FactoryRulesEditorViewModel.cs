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
using System.Windows.Data;

namespace NecBlik.Core.GUI.Factories.ViewModels
{
    public class FactoryRulesEditorViewModel : BaseViewModel
    {
        public IResponseProvider<List<string>, FactoryRule> AvailableCacheObjectIDsProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailablePropertyProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailableValueProvider { get; set; }

        public ListCollectionView FactoryRulesView { get; set; }

        public RelayCommand RemoveRuleCommand { get; set; }
        public RelayCommand AddRuleCommand { get; set; }
        public RelayCommand ClearRulesCommand { get; set; }
        public RelayCommand ClearFiltersCommand { get; set; }

        protected string cacheFilterString = string.Empty;
        public string CacheFilterString
        {
            get { return cacheFilterString; }
            set
            {
                cacheFilterString = value;
                this.OnPropertyChanged();

                if (String.IsNullOrEmpty(value))
                    FactoryRulesView.Filter = null;
                else
                    FactoryRulesView.Filter = new Predicate<object>(o =>
                    ((FactoryRuleViewModel)o).CacheObjectId.ToLower().Contains(value.ToLower()) &&
                    ((FactoryRuleViewModel)o).Property.ToLower().Contains(this.PropertyFilterString.ToLower()) &&
                    ((FactoryRuleViewModel)o).Value.ToLower().Contains(this.ValueFilterString.ToLower())
                    );
            }
        }

        protected string propertyFilterString = string.Empty;
        public string PropertyFilterString
        {
            get { return propertyFilterString; }
            set
            {
                propertyFilterString = value;
                this.OnPropertyChanged();

                if (String.IsNullOrEmpty(value))
                    FactoryRulesView.Filter = null;
                else
                    FactoryRulesView.Filter = new Predicate<object>(o =>
                    ((FactoryRuleViewModel)o).CacheObjectId.ToLower().Contains(this.CacheFilterString.ToLower()) &&
                    ((FactoryRuleViewModel)o).Property.ToLower().Contains(value.ToLower()) &&
                    ((FactoryRuleViewModel)o).Value.ToLower().Contains(this.ValueFilterString.ToLower())
                    );
            }
        }

        protected string valueFilterString = string.Empty;
        public string ValueFilterString
        {
            get { return valueFilterString; }
            set
            {
                valueFilterString = value;
                this.OnPropertyChanged();

                if (String.IsNullOrEmpty(value))
                    FactoryRulesView.Filter = null;
                else
                    FactoryRulesView.Filter = new Predicate<object>(o =>
                    ((FactoryRuleViewModel)o).CacheObjectId.ToLower().Contains(this.CacheFilterString.ToLower()) &&
                    ((FactoryRuleViewModel)o).Property.ToLower().Contains(this.PropertyFilterString.ToLower()) &&
                    ((FactoryRuleViewModel)o).Value.ToLower().Contains(value.ToLower())
                    );
            }
        }

        public FactoryRulesEditorViewModel(IResponseProvider<ObservableCollection<FactoryRuleViewModel>,object> responseProvider)
        {
            this.FactoryRulesView = new ListCollectionView(responseProvider.ProvideResponse());

            this.RemoveRuleCommand = new RelayCommand((o) =>
            {
                this.FactoryRulesView.Remove(o as FactoryRuleViewModel);
            });

            this.AddRuleCommand = new RelayCommand((o) =>
            {
                var vm = new FactoryRuleViewModel(new FactoryRule());
                vm.AvailablePropertyProvider = this.AvailablePropertyProvider;
                vm.AvailableValueProvider = this.AvailableValueProvider;
                vm.AvailableCacheObjectIDsProvider = this.AvailableCacheObjectIDsProvider; 
                this.FactoryRulesView.AddNewItem(vm);
                this.FactoryRulesView.CommitNew();
            });

            this.ClearRulesCommand = new RelayCommand((o) =>
            {
                while(this.FactoryRulesView.Count>0)
                    this.FactoryRulesView.Remove(this.FactoryRulesView.GetItemAt(0));
            });

            this.ClearFiltersCommand = new RelayCommand((o) =>
            {
                this.CacheFilterString = string.Empty;   
                this.PropertyFilterString = string.Empty;   
                this.ValueFilterString = string.Empty;   
            });
        }
    }
}
