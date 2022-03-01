using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.Factories
{
    public abstract class ZigBeeGuiFactory : IZigBeeGuiFactory
    {
        private List<IZigBeeGuiFactory> otherFactories;

        protected string internalFactoryType { get; set; } = "Abstract";

        public void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories)
        {
            this.otherFactories = zigBeeFactories;
        }

        public virtual DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public virtual DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public virtual ZigBeeNetworkViewModel GetNetworkViewModel(ZigBeeNetwork zigBeeNetwork)
        {
            return new ZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public virtual string GetVendorID()
        {
            return this.internalFactoryType;
        }
    }
}
