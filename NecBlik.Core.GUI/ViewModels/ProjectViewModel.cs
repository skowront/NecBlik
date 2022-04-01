using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.Models;

namespace NecBlik.Core.GUI.ViewModels
{
    public class ProjectViewModel: BaseViewModel
    {
        private ProjectModel model;

        public string Name
        {
            get
            {
                return this.model.ProjectName;
            }
            set
            {
                this.model.ProjectName = value; OnPropertyChanged();
            }
        }

        public ProjectViewModel(ProjectModel model)
        {
            this.model = model;
        }

    }
}
