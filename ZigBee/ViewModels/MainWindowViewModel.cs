﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.ViewModels;
using ZigBee.Core.GUI.Models;
using ZigBee.Core.Models;
using ZigBee.Core.GUI.Factories;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Strings;
using ZigBee.Common.WpfElements;
using ZigBee.Common.WpfElements.ResponseProviders;
using ZigBee.Core.GUI.Views;
using System.Threading;
using ZigBee.Models;
using System.Threading.Tasks;

namespace ZigBee.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties
        private ProjectModel model;

        private ProjectGuiModel guiModel;

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

        public Collection<ZigBeeViewModel> AvailableZigBees 
        {
            get
            {
                var all = new Collection<ZigBeeViewModel>();
                foreach (var net in this.ZigBeeNetworks)
                {
                    var zbs = net.GetZigBeeViewModels();
                    foreach(var item in zbs)
                    {
                        all.Add(item);
                    }
                    var coordinator = net.GetZigBeeCoordinatorViewModel();
                    if(coordinator!=null)
                    {
                        all.Add(coordinator);
                    }
                }
                return all;
            }
        }
        public ObservableCollection<ZigBeeNetworkViewModel> ZigBeeNetworks { get; set; } = new ObservableCollection<ZigBeeNetworkViewModel>();
        #endregion

        #region ResponseProviders
        public IResponseProvider<ZigBeeViewModel, ZigBeeViewModel> NewZigBeeResponseProvider { get; set; } = new GenericResponseProvider<ZigBeeViewModel, ZigBeeViewModel>(new ZigBeeViewModel());
        public IResponseProvider<string, object> LoadProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<string, object> SaveProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, IEnumerable<DiagramZigBee>> DiagramZigBeesLoadProvider { get; set; } = new GenericResponseProvider<object, IEnumerable<DiagramZigBee>>(null);
        public IResponseProvider<IEnumerable<DiagramZigBee>, object> DiagramZigBeesProvider { get; set; } = new GenericResponseProvider<IEnumerable<DiagramZigBee>, object>();
        public IResponseProvider<DiagramItemMetadata, object> DiagramMapMetadataProvider { get; set; } = new GenericResponseProvider<DiagramItemMetadata, object>(new DiagramItemMetadata());
        public IResponseProvider<string, object> ProjectMapPathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, Tuple<string, DiagramItemMetadata>> ProjectMapLoadedProvider { get; set; } = new GenericResponseProvider<object, Tuple<string,DiagramItemMetadata>>();
        public IResponseProvider<object, object> NewProjectLoadedProvider { get; set; } = new GenericResponseProvider<object, object>();
        public IResponseProvider<bool, object> NewProjectLoadEnsureResponseProvider { get; set; } = new GenericResponseProvider<bool, object>(true);
        public IResponseProvider<bool, object> ActionEnsureResponseProvider { get; set; } = new GenericResponseProvider<bool, object>(true);
        public IResponseProvider<bool, ZigBeeNetworkViewModel> NetworkRemovedResponseProvider { get; set; } = new GenericResponseProvider<bool, ZigBeeNetworkViewModel>(true);
        
        public IResponseProvider<string, Tuple<string, IEnumerable<string>>> ListValueResponseProvider = new GenericResponseProvider<string, Tuple<string, IEnumerable<string>>>(string.Empty);

        public IResponseProvider<YesNoProgressBarPopupResponseProvider, object> SavingProgressBarResponseProvider = null;

        public IResponseProvider<YesNoProgressBarPopupResponseProvider, object> LoadingProgressBarResponseProvider = null;

        public IResponseProvider<Task<ApplicationSettings>, ApplicationSettings> ApplicationSettingsResponseProvider = new GenericResponseProvider<Task<ApplicationSettings>, ApplicationSettings>();
        #endregion
        public ISelectionSubscriber<ZigBeeViewModel> ZigBeeSelectionSubscriber { get; set; }

        #region Commands
        public RelayCommand NewProjectCommand { get; set; }
        public RelayCommand SaveProjectCommand { get; set; }
        public RelayCommand LoadProjectCommand { get; set; }
        public RelayCommand AddNewZigBeeCommand { get; set; }
        public RelayCommand AddNetworkCommand { get; set; }
        public RelayCommand LoadProjectMapCommand { get; set; }
        public RelayCommand EditProjectCommand { get; set; }
        public RelayCommand ApplicationSettingsCommand { get; set; }
        public RelayCommand RemoveNetworkCommand { get; set; }

        #endregion

        public MainWindowViewModel(ISelectionSubscriber<ZigBeeViewModel> zigBeeSelectionSubscriber)
        {
            this.ZigBeeSelectionSubscriber = zigBeeSelectionSubscriber; 
            this.model = new ProjectModel();
            this.guiModel = new ProjectGuiModel();
            this.SyncFromModel();
            this.buildCommands();
        }

        private void buildCommands()
        {
            this.NewProjectCommand = new RelayCommand(o =>
            {
                if(this.NewProjectLoadEnsureResponseProvider.ProvideResponse() == false)
                {
                    return;
                }

                this.model = new ProjectModel();
                this.guiModel = new ProjectGuiModel();
                this.SyncFromModel();
                this.NewProjectLoadedProvider?.ProvideResponse();
            });

            this.AddNetworkCommand = new RelayCommand(async (o) =>
            {
                var ip = this.ListValueResponseProvider.ProvideResponse(new Tuple<string, IEnumerable<string>>(Strings.SR.SelectLibrary, ZigBeeGuiAnyFactory.Instance.GetFactoryIds()));
                if(ip != string.Empty)
                {
                    var vm = await ZigBeeGuiAnyFactory.Instance.NetworkViewModelFromWizard(null,ip);
                    if(vm!=null)
                    {
                        var network = vm;
                        if (network == null)
                            return;
                        this.ZigBeeNetworks.Add(network);
                        this.SyncToModel();
                        this.SyncFromModel();
                    }
                }
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
                }
            });

            this.SaveProjectCommand = new RelayCommand(o =>
            {
                this.SaveProject();
            });

            this.LoadProjectCommand = new RelayCommand(o =>
            {
                if (this.NewProjectLoadEnsureResponseProvider.ProvideResponse() == false)
                {
                    return;
                }
                this.LoadProject();
            });

            this.LoadProjectMapCommand = new RelayCommand(o =>
            {
                var path = this.ProjectMapPathProvider.ProvideResponse();
                this.LoadProjectMap(path);
            });

            this.EditProjectCommand = new RelayCommand(o =>
            {
                var vm = new ProjectViewModel(this.model);
                var window = new ProjectWindow(vm);
                window.ShowDialog();
                this.Refresh();
            });

            this.ApplicationSettingsCommand = new RelayCommand(async o =>
            {
                var res = await this.ApplicationSettingsResponseProvider.ProvideResponse(((App)App.Current).ApplicationSettings);
                if(res!=null)
                {
                    ((App)App.Current).ApplicationSettings = res;
                }
            });

            this.RemoveNetworkCommand = new RelayCommand(o => { var vm = o as ZigBeeNetworkViewModel; this.RemoveNetwork(o as ZigBeeNetworkViewModel); });
        }

        private void LoadProjectMap(string path)
        {
            if (path == null)
            {
                return;
            }
            if (path == string.Empty)
            {
                return;
            }
            this.MapFilePath = path;
            this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.guiModel.mapDiagramMetadata));
        }

        private async void LoadProject()
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
            this.NewProjectLoadedProvider?.ProvideResponse();
            var file = path;
            var dir = Path.GetDirectoryName(path);
            if (File.Exists(file))
            {
                var projectStr = File.ReadAllText(file);
                this.model = JsonConvert.DeserializeObject<ProjectModel>(projectStr);
            }
            else
            {
                
                this.model.Save(path, this.SavingProgressBarResponseProvider.ProvideResponse());
                this.SaveProjectGui(path);
            }
            await this.model.Load(dir, this.LoadingProgressBarResponseProvider.ProvideResponse());
            this.LoadProjectGui(path);
            
            this.SyncFromModel();
        }

        private void LoadProjectGui(string path)
        {
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
            file = file.Replace(".json", ".Gui.json");
            if (File.Exists(file))
            {
                var projectStr = File.ReadAllText(file);
                this.guiModel = JsonConvert.DeserializeObject<ProjectGuiModel>(projectStr);
            }
            else
            {
                this.guiModel.Save(path);
            }
        }

        private void SaveProject()
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
                    Directory.Delete(dir,true);
                    Directory.CreateDirectory(dir);
                    File.AppendAllText(file, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                }
            }
            this.model.Save(dir, this.SavingProgressBarResponseProvider.ProvideResponse());
            this.SaveProjectGui(dir);
        }

        public void SaveProjectGui(string path)
        {
            if (path == null)
            {
                return;
            }
            if (path == string.Empty)
            {
                return;
            }
            this.SyncToModel();
            var file = path + "\\" + this.ProjectName + ".Gui.json";
            var dir = path;
            if (File.Exists(file))
            {
                File.WriteAllText(file, JsonConvert.SerializeObject(this.guiModel, Formatting.Indented));
            }
            else
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (Directory.Exists(dir))
                {
                    File.AppendAllText(file, JsonConvert.SerializeObject(this.guiModel, Formatting.Indented));
                }
            }
            this.guiModel.Save(dir);
        }

        private void SyncToModel()
        {
            this.model.ZigBeeNetworks.Clear();
            foreach(var item in this.ZigBeeNetworks)
            {
                this.model.ZigBeeNetworks.Add(item.Model);
            }
            //this.model.DiagramZigBees = this.DiagramZigBeesProivider.ProvideResponse();
            this.guiModel.mapDiagramMetadata = this.DiagramMapMetadataProvider.ProvideResponse();
            this.guiModel.mapItemsMetadata = new Collection<DiagramZigBee>(new List<DiagramZigBee>(this.DiagramZigBeesProvider?.ProvideResponse()));
            //this.model.AvailableZigBees.Clear();
            //foreach (var item in this.AvailableZigBees)
            //{
            //    this.model.AvailableZigBees.Add(item.Model);
            //}
        }

        private void SyncFromModel()
        {
            var zbn = this.model.ZigBeeNetworks;
            this.ZigBeeNetworks.Clear();
            foreach (var item in zbn)
            {
                var nvm = ZigBeeGuiAnyFactory.Instance.GetNetworkViewModel(item);
                nvm.ZigBeeSelectionSubscriber = this.ZigBeeSelectionSubscriber;
                nvm.Sync();
                this.ZigBeeNetworks.Add(nvm);
            }
            //TODO
            //this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.guiModel.mapDiagramMetadata));
            this.DiagramZigBeesLoadProvider.ProvideResponse(this.guiModel.mapItemsMetadata);
            //this.AvailableZigBees.Clear();
            //foreach (var item in this.model.AvailableZigBees)
            //{
            //    this.AvailableZigBees.Add(new ZigBeeViewModel(item));
            //}
            //this.DiagramZigBeesLoadedProvider.ProvideResponse(this.model.DiagramZigBees);
            this.LoadProjectMap(this.model.MapFile);
            this.Refresh();
        }

        private void RemoveNetwork(ZigBeeNetworkViewModel networkViewModel)
        {
            var resp = this.ActionEnsureResponseProvider?.ProvideResponse();
            if (resp == null)
                return;
            if (resp == false)
                return;
            this.NetworkRemovedResponseProvider?.ProvideResponse(networkViewModel);
            this.ZigBeeNetworks.Remove(networkViewModel);
            this.Refresh();
        }

        private void Refresh()
        {
            this.OnPropertyChanged(nameof(this.ProjectName));
        }
    }
}
