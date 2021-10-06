using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Core.GUI.Factories
{
    public class ZigBeeGuiDefaultFactory: ZigBeeGuiFactory
    {
        private List<IZigBeeGuiFactory> otherFactories;

        protected string internalFactoryType { get; set; } = "Abstract";

        public void AttachOtherFactories(List<IZigBeeGuiFactory> zigBeeFactories)
        {
            this.otherFactories = zigBeeFactories;
        }

        public DataTemplate GetNetworkBriefDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public DataTemplate GetNetworkDataTemplate(ZigBeeNetwork zigBeeNetwork)
        {
            return null;
        }

        public virtual string GetVendorID()
        {
            return this.internalFactoryType;
        }
    }
}
