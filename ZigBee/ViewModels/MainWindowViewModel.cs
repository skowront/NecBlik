using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
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

        public RelayCommand NewProject { get; set; }

        public MainWindowViewModel()
        {
            this.model = new ProjectModel();
            this.buildCommands();
        }

        private void buildCommands()
        {
            this.NewProject = new RelayCommand(o =>
            {
                this.model = new ProjectModel();
            });
        }
    }
}
