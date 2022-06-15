using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Core.GUI.Models;
using NecBlik.Core.Models;
using NecBlik.Core.GUI.Factories;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Strings;
using NecBlik.Common.WpfElements;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Core.GUI.Views;
using System.Threading;
using NecBlik.Models;
using System.Threading.Tasks;
using NecBlik.Views;

namespace NecBlik.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties
        private ProjectModel model;

        private ProjectGuiModel guiModel;

        public string ProjectName
        {
            get { return this.model.ProjectName; }
            set { this.model.ProjectName = value; this.ProjectDirectory = string.Empty; this.OnPropertyChanged(); }
        }

        public string MapFilePath
        {
            get { return this.guiModel.MapFile; }
            set { this.guiModel.MapFile = value; this.OnPropertyChanged(); }
        }

        public string ProjectDirectory
        {
            get; set;
        } = string.Empty;

        public Collection<DeviceViewModel> AvailableDevices 
        {
            get
            {
                var all = new Collection<DeviceViewModel>();
                foreach (var net in this.Networks)
                {
                    var zbs = net.GetDeviceViewModels();
                    foreach(var item in zbs)
                    {
                        all.Add(item);
                    }
                    var coordinator = net.GetCoordinatorViewModel();
                    if(coordinator!=null)
                    {
                        all.Add(coordinator);
                    }
                }
                return all;
            }
        }

        public ObservableCollection<NetworkViewModel> Networks { get; set; } = new ObservableCollection<NetworkViewModel>();
        #endregion

        #region ResponseProviders
 
        public IResponseProvider<string, object> LoadProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<string, object> SaveProjectFilePathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, IEnumerable<DiagramDevice>> DiagramDevicesLoadProvider { get; set; } = new GenericResponseProvider<object, IEnumerable<DiagramDevice>>(null);
        public IResponseProvider<IEnumerable<DiagramDevice>, object> DiagramDevicesProvider { get; set; } = new GenericResponseProvider<IEnumerable<DiagramDevice>, object>();
        public IResponseProvider<DiagramItemMetadata, object> DiagramMapMetadataProvider { get; set; } = new GenericResponseProvider<DiagramItemMetadata, object>(new DiagramItemMetadata());
        public IResponseProvider<string, object> ProjectMapPathProvider { get; set; } = new GenericResponseProvider<string, object>(string.Empty);
        public IResponseProvider<object, Tuple<string, DiagramItemMetadata>> ProjectMapLoadedProvider { get; set; } = new GenericResponseProvider<object, Tuple<string,DiagramItemMetadata>>();
        public IResponseProvider<object, object> ProjectMapRemoveProvider { get; set; } = new GenericResponseProvider<object, object>();
        public IResponseProvider<object, object> NewProjectLoadedProvider { get; set; } = new GenericResponseProvider<object, object>();
        public IResponseProvider<bool, object> NewProjectLoadEnsureResponseProvider { get; set; } = new GenericResponseProvider<bool, object>(true);
        public IResponseProvider<bool, object> ActionEnsureResponseProvider { get; set; } = new GenericResponseProvider<bool, object>(true);
        public IResponseProvider<bool, NetworkViewModel> NetworkRemovedResponseProvider { get; set; } = new GenericResponseProvider<bool, NetworkViewModel>(true);
        
        public IResponseProvider<string, Tuple<string, IEnumerable<string>>> ListValueResponseProvider = new GenericResponseProvider<string, Tuple<string, IEnumerable<string>>>(string.Empty);

        public IResponseProvider<YesNoProgressBarPopupResponseProvider, object> SavingProgressBarResponseProvider = null;

        public IResponseProvider<YesNoProgressBarPopupResponseProvider, object> LoadingProgressBarResponseProvider = null;

        public IResponseProvider<Task<ApplicationSettings>, ApplicationSettings> ApplicationSettingsResponseProvider = new GenericResponseProvider<Task<ApplicationSettings>, ApplicationSettings>();
        #endregion
        public ISelectionSubscriber<DeviceViewModel> DeviceSelectionSubscriber { get; set; }

        #region Commands
        public RelayCommand NewProjectCommand { get; set; }
        public RelayCommand SaveProjectCommand { get; set; }
        public RelayCommand SaveProjectAsCommand { get; set; }
        public RelayCommand LoadProjectCommand { get; set; }
        public RelayCommand AddNetworkCommand { get; set; }
        public RelayCommand LoadProjectMapCommand { get; set; }
        public RelayCommand RemoveProjectMapCommand { get; set; }
        public RelayCommand EditProjectCommand { get; set; }
        public RelayCommand ApplicationSettingsCommand { get; set; }
        public RelayCommand RemoveNetworkCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }

        #endregion

        public MainWindowViewModel(ISelectionSubscriber<DeviceViewModel> deviceSelectionSubscriber)
        {
            this.DeviceSelectionSubscriber = deviceSelectionSubscriber; 
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

                foreach(var nwk in this.Networks)
                {
                    nwk.Dispose();
                }

                this.model = new ProjectModel();
                this.guiModel = new ProjectGuiModel();
                this.SyncFromModel();
                this.NewProjectLoadedProvider?.ProvideResponse();
            });

            this.AddNetworkCommand = new RelayCommand(async (o) =>
            {
                var ip = this.ListValueResponseProvider.ProvideResponse(new Tuple<string, IEnumerable<string>>(Strings.SR.SelectLibrary, DeviceGuiAnyFactory.Instance.GetFactoryIds()));
                if(ip != string.Empty)
                {
                    var vm = await DeviceGuiAnyFactory.Instance.NetworkViewModelFromWizard(null,ip);
                    
                    if(vm!=null)
                    {
                        var network = vm;
                        if (network == null)
                            return;
                        vm.DeviceSelectionSubscriber = this.DeviceSelectionSubscriber;
                        this.Networks.Add(network);
                        this.SyncToModel();
                        this.SyncFromModel();
                    }
                }
            });

            this.SaveProjectCommand = new RelayCommand(o =>
            {
                this.SaveProject();
            });

            this.SaveProjectAsCommand = new RelayCommand(o =>
            {
                this.SaveProjectAs();
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

            this.RemoveProjectMapCommand = new RelayCommand((o) =>
            {
                this.RemoveProjectMap();
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

            this.RemoveNetworkCommand = new RelayCommand(o => 
            {   
                this.RemoveNetwork(o as NetworkViewModel); 
            });

            this.AboutCommand = new RelayCommand((o) =>
            {
                var window = new AboutWindow();
                window.Show();
            });
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
            if (this.ProjectDirectory != string.Empty)
            { 
                if(path != this.ProjectDirectory + "\\" + Path.GetFileName(path))
                    File.Copy(path, this.ProjectDirectory + "\\" + Path.GetFileName(path), true);
                this.MapFilePath = this.ProjectDirectory + "\\" + Path.GetFileName(path);
                this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.guiModel.mapDiagramMetadata));
            }
            else
            {
                this.MapFilePath = path;
                this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.guiModel.mapDiagramMetadata));
            }
        }

        private void RemoveProjectMap()
        {
            this.ProjectMapRemoveProvider.ProvideResponse();
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
            this.ProjectDirectory = Path.GetDirectoryName(path);
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
            var path = this.ProjectDirectory != string.Empty ? this.ProjectDirectory : this.SaveProjectFilePathProvider?.ProvideResponse();
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
                    if(Directory.Exists(dir + NecBlik.Core.Resources.Resources.DeviceNetworksDirectory))
                        Directory.Delete(dir + NecBlik.Core.Resources.Resources.DeviceNetworksDirectory, true);
                    File.AppendAllText(file, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                }
            }
            this.ProjectDirectory = dir;
            this.model.Save(dir, this.SavingProgressBarResponseProvider.ProvideResponse());
            this.SaveProjectGui(dir);
        }

        private void SaveProjectAs()
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
                    if (Directory.Exists(dir + NecBlik.Core.Resources.Resources.DeviceNetworksDirectory))
                        Directory.Delete(dir + NecBlik.Core.Resources.Resources.DeviceNetworksDirectory, true);
                    Directory.CreateDirectory(dir);
                    File.AppendAllText(file, JsonConvert.SerializeObject(this.model, Formatting.Indented));
                }
            }
            this.ProjectDirectory = dir;
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
                if (!File.Exists(this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath)))
                {
                    var nmf = this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath);
                    File.Copy(this.MapFilePath, nmf);
                    this.MapFilePath = Path.GetFileName(nmf);
                }
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
                    if(this.MapFilePath !=null && this.MapFilePath!=String.Empty && this.ProjectDirectory!=string.Empty)
                        if (!File.Exists(this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath)))
                        {
                            var nmf = this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath);
                            File.Copy(this.MapFilePath, nmf);
                            this.MapFilePath = Path.GetFileName(nmf);
                        }
                    File.AppendAllText(file, JsonConvert.SerializeObject(this.guiModel, Formatting.Indented));
                }
            }
            
            this.guiModel.Save(dir);
        }

        private void SyncToModel()
        {
            this.model.Networks.Clear();
            foreach(var item in this.Networks)
            {
                this.model.Networks.Add(item.Model);
            }
            //this.model.DiagramZigBees = this.DiagramZigBeesProivider.ProvideResponse();
            this.guiModel.mapDiagramMetadata = this.DiagramMapMetadataProvider.ProvideResponse();
            this.guiModel.mapItemsMetadata = new Collection<DiagramDevice>(new List<DiagramDevice>(this.DiagramDevicesProvider?.ProvideResponse()));
            //this.model.AvailableZigBees.Clear();
            //foreach (var item in this.AvailableZigBees)
            //{
            //    this.model.AvailableZigBees.Add(item.Model);
            //}
        }

        private void SyncFromModel()
        {
            var zbn = this.model.Networks;
            this.Networks.Clear();
            foreach (var item in zbn)
            {
                var nvm = DeviceGuiAnyFactory.Instance.GetNetworkViewModel(item);
                nvm.DeviceSelectionSubscriber = this.DeviceSelectionSubscriber;
                nvm.Sync();
                this.Networks.Add(nvm);
            }
            //TODO
            //this.ProjectMapLoadedProvider.ProvideResponse(new Tuple<string, DiagramItemMetadata>(this.MapFilePath, this.guiModel.mapDiagramMetadata));
            this.DiagramDevicesLoadProvider.ProvideResponse(this.guiModel.mapItemsMetadata);
            //this.AvailableZigBees.Clear();
            //foreach (var item in this.model.AvailableZigBees)
            //{
            //    this.AvailableZigBees.Add(new ZigBeeViewModel(item));
            //}
            //this.DiagramZigBeesLoadedProvider.ProvideResponse(this.model.DiagramZigBees);
            if (this.ProjectDirectory != string.Empty && this.MapFilePath!=null)
                this.LoadProjectMap(this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath));
            else if(this.MapFilePath!=null)
                this.LoadProjectMap(this.MapFilePath);
            this.Refresh();
        }

        private void RemoveNetwork(NetworkViewModel networkViewModel)
        {
            var resp = this.ActionEnsureResponseProvider?.ProvideResponse();
            if (resp == null)
                return;
            if (resp == false)
                return;
            this.NetworkRemovedResponseProvider?.ProvideResponse(networkViewModel);
            networkViewModel.Dispose();
            this.Networks.Remove(networkViewModel);
            this.Refresh();
        }

        private void Refresh()
        {
            if(this.MapFilePath!=null&&this.ProjectDirectory!=string.Empty)
                this.MapFilePath = Path.GetFullPath(this.ProjectDirectory + "\\" + Path.GetFileName(this.MapFilePath));
            this.ProjectDirectory = string.Empty;
            this.OnPropertyChanged(nameof(this.ProjectName));
        }
    }
}
