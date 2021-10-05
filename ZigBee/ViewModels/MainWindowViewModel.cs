using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI;
using ZigBee.Core.Models;
using ZigBee.Models;
using ZigBee.Virtual.Models;

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

        //public DiagramItemMetadata MapMetadata
        //{
        //    get { return this.model.DiagramMapMetadata; }
        //    set { this.model.DiagramMapMetadata = value; this.OnPropertyChanged(); }
        //}

        public ObservableCollection<ZigBeeViewModel> AvailableZigBees { get; set; } = new ObservableCollection<ZigBeeViewModel>();

        public ObservableCollection<ZigBeeNetwork> ZigBeeNetworks { get; set; } = new ObservableCollection<ZigBeeNetwork>();

        public IResponseProvider<ZigBeeViewModel, ZigBeeViewModel> NewZigBeeResponseProvider { get; set; } = new GenericResponseProvider<ZigBeeViewModel, ZigBeeViewModel>(new ZigBeeViewModel());
        public IResponseProvider<string, object> LoadProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<string, object> SaveProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, IEnumerable<DiagramZigBee>> DiagramZigBeesLoadedProvider { get; set; } = new GenericResponseProvider<object, IEnumerable<DiagramZigBee>>(null);
        public IResponseProvider<IEnumerable<DiagramZigBee>, object> DiagramZigBeesProivider { get; set; } = new GenericResponseProvider<IEnumerable<DiagramZigBee>, object>();
        public IResponseProvider<DiagramItemMetadata, object> DiagramMapMetadataProvider { get; set; } = new GenericResponseProvider<DiagramItemMetadata, object>(new DiagramItemMetadata());
        public IResponseProvider<string, object> ProjectMapPathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, Tuple<string, DiagramItemMetadata>> ProjectMapLoadedProvider { get; set; } = new GenericResponseProvider<object, Tuple<string,DiagramItemMetadata>>();
        public IResponseProvider<object, object> NewProjectLoadedProvider { get; set; } = new GenericResponseProvider<object, object>();

        public RelayCommand NewProjectCommand { get; set; }
        public RelayCommand SaveProjectCommand { get; set; }
        public RelayCommand LoadProjectCommand { get; set; }
        public RelayCommand AddNewZigBeeCommand { get; set; }
        public RelayCommand LoadProjectMapCommand { get; set; }

        public MainWindowViewModel()
        {
            this.model = new ProjectModel();
            this.model.ZigBeeNetworks.Add(new VirtualZigBeeNetwork(true));
            this.SyncFromModel();
            this.buildCommands();
        }

        private void buildCommands()
        {
            this.NewProjectCommand = new RelayCommand(o =>
            {
                this.model = new ProjectModel();
                this.SyncFromModel();
                this.NewProjectLoadedProvider?.ProvideResponse();
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
                var file = path + "\\" + this.ProjectName + "\\" + this.ProjectName + ".json";
                var dir = path + "\\" + this.ProjectName;
                if (File.Exists(file))
                {
                    File.WriteAllText(file, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                }
                else
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (Directory.Exists(dir))
                    {
                        File.AppendAllText(file, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                    }
                }
                this.model.Save(dir);
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
                var file = path;
                var dir = Path.GetDirectoryName(path);
                if (File.Exists(file))
                {
                    var projectStr = File.ReadAllText(file);
                    this.model = JsonConvert.DeserializeObject<ProjectModel>(projectStr);
                }
                else
                {
                    this.model.Save(path);
                }
                this.model.Load(dir);
                this.SyncFromModel();
                //this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath,this.MapMetadata));
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
                //this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.MapMetadata));
            });
        }

        private void SyncToModel()
        {
            //this.model.DiagramZigBees = this.DiagramZigBeesProivider.ProvideResponse();
            //this.model.DiagramMapMetadata = this.DiagramMapMetadataProvider.ProvideResponse();
            //this.model.AvailableZigBees.Clear();
            //foreach (var item in this.AvailableZigBees)
            //{
            //    this.model.AvailableZigBees.Add(item.Model);
            //}
        }

        private void SyncFromModel()
        {
            //this.AvailableZigBees.Clear();
            //foreach (var item in this.model.AvailableZigBees)
            //{
            //    this.AvailableZigBees.Add(new ZigBeeViewModel(item));
            //}
            //this.DiagramZigBeesLoadedProvider.ProvideResponse(this.model.DiagramZigBees);
            this.Refresh();
        }

        private void Refresh()
        {
            this.OnPropertyChanged(nameof(this.ProjectName));
        }
    }
}
