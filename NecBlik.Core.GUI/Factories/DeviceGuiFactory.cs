﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.GUI.Views.Controls;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.Factories
{
    public abstract class DeviceGuiFactory : IDeviceGuiFactory
    {
        protected List<IDeviceGuiFactory> otherFactories;

        protected List<IDeviceGuiFactory> subFactories;

        protected string internalFactoryType { get; set; } = "Abstract";

        public DeviceGuiFactory()
        {
            this.internalFactoryType = "Abstract";
        }

        public virtual void AttachOtherFactories(List<IDeviceGuiFactory> deviceFactories)
        {
            this.otherFactories = deviceFactories;
        }

        public virtual DataTemplate GetNetworkBriefDataTemplate(Network network)
        {
            return null;
        }

        public virtual DataTemplate GetNetworkDataTemplate(Network network)
        {
            return null;
        }

        public virtual NetworkViewModel GetNetworkViewModel(Network network)
        {
            return new NetworkViewModel(network);
        }

        public virtual string GetVendorID()
        {
            return this.internalFactoryType;
        }

        public virtual UIElement GetDeviceControl(DeviceViewModel deviceViewModel)
        {
            return new DeviceControl(deviceViewModel);
        }

        public virtual async Task<NetworkViewModel> NetworkViewModelFromWizard(Network network)
        {
            return new NetworkViewModel(network);
        }

        public virtual void Initalize(object args = null)
        {
            var dir = string.Format(NecBlik.Core.Resources.Resources.LibraryFolderFormattableString, this.internalFactoryType);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }


        public bool IsLicensed()
        {
            return false;
        }

        public IEnumerable<string> GetLicensees()
        {
            return new List<string>();
        }
    }
}
