using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.Interfaces;
using NecBlik.Core.Models;
using NecBlik.Virtual.GUI.Views;

namespace NecBlik.Virtual.GUI.ViewModels
{
    public class VirtualDeviceViewModel : DeviceViewModel
    {
        protected List<Window> relatedWindows = new List<Window>();

        public VirtualDeviceViewModel(DeviceModel model, NetworkViewModel networkModel) : base(model, networkModel)
        {

        }

        protected override void BuildCommands()
        {
            base.BuildCommands();
            this.EditCommand = new RelayCommand((o) =>
            {
                Window window = new VirtualDeviceWindow(this);
                window.Show();
                this.relatedWindows.Add(window);
            });
        }

        public override void OnDataRecieved(string data, string sourceAddress)
        {
            base.OnDataRecieved(data,sourceAddress);
        }

        public override void Dispose()
        {
            base.Dispose();
            var toRemove = new List<Window>();
            foreach(var window in this.relatedWindows)
            {
                foreach (var item in Application.Current.Windows)
                {
                    if (item == window)
                    {
                        window.Close();
                        toRemove.Add(window);
                    }
                }
            }
            foreach(var item in toRemove)
            {
                this.relatedWindows.Remove(item);
            }
        }
    }
}
