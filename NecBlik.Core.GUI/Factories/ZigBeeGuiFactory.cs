using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.GUI.Views.Controls;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.Factories
{
    public abstract class ZigBeeGuiFactory : IZigBeeGuiFactory
    {
        private List<IZigBeeGuiFactory> otherFactories;

        protected string internalFactoryType { get; set; } = "Abstract";

        public ZigBeeGuiFactory()
        {
            this.internalFactoryType = "Abstract";
        }

        public virtual void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories)
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

        public virtual UIElement GetZigBeeControl(ZigBeeViewModel zigBeeViewModel)
        {
            return new ZigBeeControl(zigBeeViewModel);
        }

        public virtual async Task<ZigBeeNetworkViewModel> NetworkViewModelFromWizard(ZigBeeNetwork zigBeeNetwork)
        {
            return new ZigBeeNetworkViewModel(zigBeeNetwork);
        }

        public virtual void Initalize(object args = null)
        {
            
        }
    }
}
