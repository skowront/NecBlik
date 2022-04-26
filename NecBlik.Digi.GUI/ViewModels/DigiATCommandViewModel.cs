using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Digi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Digi.GUI.ViewModels
{
    public class DigiATCommandViewModel:BaseViewModel
    {
        public DigiATCommand Model { get; set; }

        public string Address
        {
            get { return Model.Address; }
            set { Model.Address = value; this.OnPropertyChanged(); }
        }

        public string Command 
        {
            get { return this.Model.Command; }
            set { this.Model.Command = value; this.OnPropertyChanged(); }
        }

        public string Parameter
        {
            get { return this.Model.Parameter; }
            set { this.Model.Parameter = value; this.OnPropertyChanged(); }
        }

        public DigiATCommandViewModel(DigiATCommand model = null)
        {
            this.Model = model ?? new();
        }
    }
}
