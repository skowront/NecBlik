using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Models;

namespace ZigBee.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        private ProjectModel model;

        public string ProjectName
        {
            get { return model.ProjectName; }
            set { this.model.ProjectName = value; this.OnPropertyChanged(); }
        }

        public BindingList<ZigBeeViewModel> AvailableZigBees { get; set; } = new BindingList<ZigBeeViewModel>();

        public IResponseProvider<ZigBeeViewModel, ZigBeeViewModel> NewZigBeeResponseProvider { get; set; } = new GenericResponseProvider<ZigBeeViewModel, ZigBeeViewModel>(new ZigBeeViewModel());

        public RelayCommand NewProject { get; set; }
        public RelayCommand AddNewZigBeeCommand { get; set; }

        public MainWindowViewModel()
        {
            this.model = new ProjectModel();
            foreach(var item in this.model.AvailableZigBees)
            {
                this.AvailableZigBees.Add(new ZigBeeViewModel(item));
            }
            this.buildCommands();
        }

        private void buildCommands()
        {
            this.NewProject = new RelayCommand(o =>
            {
                this.model = new ProjectModel();
            });

            this.AddNewZigBeeCommand = new RelayCommand(o =>
            {
                var response = this.NewZigBeeResponseProvider.ProvideResponse(new ZigBeeViewModel(this.model.ZigBeeTemplate.Duplicate()));
                if (response==null)
                {
                    return;
                }
                else
                {
                    this.AvailableZigBees.Add(response);
                }
            });
        }
    }
}
