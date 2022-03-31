using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.Factories;

namespace ZigBee.Core.GUI.Factories.ViewModels
{
    public class FactoryRuleViewModel:BaseViewModel
    {
        public FactoryRule Model { get; set; }

        public string CacheObjectId
        {
            get { return Model.CacheObjectId; }
            set { Model.CacheObjectId = value; this.OnPropertyChanged(); }
        }

        public string Value
        {
            get { return this.Model.Value; }
            set { this.Model.Value = value; this.OnPropertyChanged(); }
        }

        public string Property
        {
            get { return this.Model.Property; }
            set { this.Model.Property = value; this.OnPropertyChanged(); }
        }

        public FactoryRuleViewModel(FactoryRule factoryRule)
        {
            this.Model = factoryRule;
        }
    }
}
