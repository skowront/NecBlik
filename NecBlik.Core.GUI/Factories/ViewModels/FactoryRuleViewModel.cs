using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Common.WpfElements.PopupValuePickers;
using NecBlik.Core.Factories;
using NecBlik.Common.WpfElements.PopupValuePickers.ResponseProviders;

namespace NecBlik.Core.GUI.Factories.ViewModels
{
    public class FactoryRuleViewModel : BaseViewModel
    {
        public FactoryRule Model { get; set; }

        public string CacheObjectId
        {
            get { return Model.CacheObjectId; }
            set { Model.CacheObjectId = value; this.OnPropertyChanged(); }
        }

        public string Property
        {
            get { return this.Model.Property; }
            set { this.Model.Property = value; this.OnPropertyChanged(); }
        }

        public string Value
        {
            get { return this.Model.Value; }
            set { this.Model.Value = value; this.OnPropertyChanged(); }
        }

        public RelayCommand SelectCacheObjectIDCommand { get; set; }
        public RelayCommand SelectPropertyCommand { get; set; }
        public RelayCommand SelectValueCommand { get; set; }

        public IResponseProvider<List<string>, FactoryRule> AvailableCacheObjectIDsProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailablePropertyProvider { get; set; }
        public IResponseProvider<List<string>, FactoryRule> AvailableValueProvider { get; set; }

        public FactoryRuleViewModel(FactoryRule factoryRule)
        {
            this.Model = factoryRule;
            this.buildCommands();
        }

        private void buildCommands()
        {

            this.SelectCacheObjectIDCommand = new RelayCommand((o) =>
            {
                var av = this.AvailableCacheObjectIDsProvider?.ProvideResponse(this.Model);
                if (av == null)
                {
                    var popup = new StringValuePicker(null, null);
                    var rp = new StringResponseProvider(popup);
                    string response = rp.ProvideResponse();
                    if (response == null)
                    {
                        return;
                    }
                    else
                    {
                        this.CacheObjectId = response;
                    }
                }
                else
                {
                    var arp = new ListInputValuePicker(true);
                    var availableItems = this.AvailableCacheObjectIDsProvider.ProvideResponse(this.Model);
                    this.CacheObjectId = arp.ProvideResponse(new Tuple<string, IEnumerable<string>>(string.Empty, availableItems));
                }
            });

            this.SelectPropertyCommand = new RelayCommand((o) =>
            {
                var av = this.AvailablePropertyProvider?.ProvideResponse(this.Model);
                if (av == null)
                {
                    var popup = new StringValuePicker(null, null);
                    var rp = new StringResponseProvider(popup);
                    string response = rp.ProvideResponse();
                    if (response == null)
                    {
                        return;
                    }
                    else
                    {
                        this.Property = response;
                    }
                }
                else
                {
                    var arp = new ListInputValuePicker(false);
                    var availableItems = this.AvailablePropertyProvider.ProvideResponse(this.Model);
                    this.Property = arp.ProvideResponse(new Tuple<string, IEnumerable<string>>(string.Empty, availableItems));
                }
            });

            this.SelectValueCommand = new RelayCommand((o) =>
            {
                var av = this.AvailableValueProvider?.ProvideResponse(this.Model);
                if (av == null)
                {
                    var popup = new StringValuePicker(null, null);
                    var rp = new StringResponseProvider(popup);
                    string response = rp.ProvideResponse();
                    if (response == null)
                    {
                        return;
                    }
                    else
                    {
                        this.Value = response;
                    }
                }
                else
                {
                    var arp = new ListInputValuePicker(false);
                    var availableItems = this.AvailableValueProvider.ProvideResponse(this.Model);
                    this.Value = arp.ProvideResponse(new Tuple<string, IEnumerable<string>>(string.Empty, availableItems));
                }
            });
        }
    }
}
