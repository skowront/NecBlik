using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Models;

namespace ZigBee.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ProjectModel model;

        public string ProjectName
        {
            get { return this.model.ProjectName; }
            set { this.model.ProjectName = value; this.OnPropertyChanged(); }
        }

        public string MapFilePath
        {
            get { return this.model.MapFile; }
            set { this.model.MapFile = value; this.OnPropertyChanged(); }
        }

        public BindingList<ZigBeeViewModel> AvailableZigBees { get; set; } = new BindingList<ZigBeeViewModel>();

        public IResponseProvider<ZigBeeViewModel, ZigBeeViewModel> NewZigBeeResponseProvider { get; set; } = new GenericResponseProvider<ZigBeeViewModel, ZigBeeViewModel>(new ZigBeeViewModel());
        public IResponseProvider<string, object> LoadProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<string, object> SaveProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, IEnumerable<DiagramZigBee>> DiagramZigBeesLoadedProvider { get; set; } = new GenericResponseProvider<object, IEnumerable<DiagramZigBee>>(null);
        public IResponseProvider<IEnumerable<DiagramZigBee>, object> DiagramZigBeesProivider { get; set; } = new GenericResponseProvider<IEnumerable<DiagramZigBee>, object>();
        public IResponseProvider<string, object> ProjectMapPathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, string> ProjectMapLoadedProvider { get; set; } = new GenericResponseProvider<object, string>();

        public RelayCommand NewProject { get; set; }
        public RelayCommand SaveProjectCommand { get; set; }
        public RelayCommand LoadProjectCommand { get; set; }
        public RelayCommand AddNewZigBeeCommand { get; set; }
        public RelayCommand LoadProjectMapCommand { get; set; }

        public MainWindowViewModel()
        {
            this.model = new ProjectModel();
            this.SyncFromModel();
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
                if (response == null)
                {
                    return;
                }
                else
                {
                    this.AvailableZigBees.Add(response);
                }
            });

            this.SaveProjectCommand = new RelayCommand(o =>
            {
                var path = this.SaveProjectFilePathProvider.ProvideResponse();
                if (path == null)
                {
                    return;
                }
                if (path == string.Empty)
                {
                    return;
                }
                this.SyncToModel();
                File.WriteAllText(path, JsonConvert.SerializeObject(this.model, Formatting.Indented));
            });

            this.LoadProjectCommand = new RelayCommand(o =>
            {
                var path = this.LoadProjectFilePathProvider.ProvideResponse();
                if (path == null)
                {
                    return;
                }
                if (path == string.Empty)
                {
                    return;
                }
                this.model = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(path));
                this.SyncFromModel();
                this.ProjectMapLoadedProvider.ProvideResponse(this.MapFilePath);
            });

            this.LoadProjectMapCommand = new RelayCommand(o =>
            {
                var path = this.ProjectMapPathProvider.ProvideResponse();
                if (path == null)
                {
                    return;
                }
                if (path == string.Empty)
                {
                    return;
                }
                this.MapFilePath = path;
                this.ProjectMapLoadedProvider.ProvideResponse(this.MapFilePath);
            });
        }

        private void SyncToModel()
        {
            this.model.DiagramZigBees = this.DiagramZigBeesProivider.ProvideResponse();
            this.model.AvailableZigBees.Clear();
            foreach (var item in this.AvailableZigBees)
            {
                this.model.AvailableZigBees.Add(item.Model);
            }
        }

        private void SyncFromModel()
        {
            this.AvailableZigBees.Clear();
            foreach (var item in this.model.AvailableZigBees)
            {
                this.AvailableZigBees.Add(new ZigBeeViewModel(item));
            }
            this.DiagramZigBeesLoadedProvider.ProvideResponse(this.model.DiagramZigBees);
            this.Refresh();
        }

        private void Refresh()
        {
            this.OnPropertyChanged(nameof(this.ProjectName));
        }
    }
}
