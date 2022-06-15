using Microsoft.Win32;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NecBlik.Common.WpfElements;
using NecBlik.Common.WpfElements.PopupValuePickers;
using NecBlik.Common.WpfElements.ResponseProviders;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Core.GUI;
using NecBlik.Core.GUI.Interfaces;
using NecBlik.Core.GUI.Models;
using NecBlik.Core.GUI.ViewModels;
using NecBlik.Models;
using NecBlik.ViewModels;
using NecBlik.Views.Controls;
using MahApps.Metro.Controls;

namespace NecBlik.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, ISelectionSubscriber<DeviceViewModel>
    {
        private Point startPoint = new Point();

        private MainWindowViewModel ViewModel;

        private UIElement draggedItem;

        private FrameworkElement map;

        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = new MainWindowViewModel(this);
            this.DataContext = this.ViewModel;
            this.BuildResponseProviders();
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = this.startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView lv = sender as ListView;
                ListViewItem lvi = this.FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (lvi == null)
                {
                    return;
                }
                if(lv.ItemContainerGenerator.ItemFromContainer(lvi) is DeviceViewModel)
                {
                    var vm = (DeviceViewModel)lv.ItemContainerGenerator.ItemFromContainer(lvi);
                    var xaml = XamlWriter.Save(NecBlik.Core.GUI.Factories.DeviceGuiAnyFactory.Instance.GetDeviceControl(vm));
                    DragDrop.DoDragDrop(lvi, new DataObject(vm.GetType(), vm), DragDropEffects.Copy);
                }

            }
        }

        // Helper to search up the VisualTree
        private T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.Key==Key.Delete)
            {
                this.designerCanvas.DeleteSelection();
            }
        }

        private void BuildResponseProviders()
        {

            this.ViewModel.SaveProjectFilePathProvider = new GenericResponseProvider<string, object>(o =>
            {
                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                if (dialog.ShowDialog(this).GetValueOrDefault())
                {
                    return dialog.SelectedPath;
                }
                return string.Empty;
            });

            this.ViewModel.LoadProjectFilePathProvider = new GenericResponseProvider<string, object>(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = Strings.SR.JsonFilesOpenFileDialogFilter };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.DiagramDevicesProvider = new GenericResponseProvider<IEnumerable<DiagramDevice>, object>((o) =>
            {
                return this.designerCanvas.GetDiagramDevices();
            });

            this.ViewModel.DiagramMapMetadataProvider = new GenericResponseProvider<DiagramItemMetadata, object>((o) =>
            {
                return this.designerCanvas.GetMapMetadata();
            });

            this.ViewModel.DiagramDevicesLoadProvider = new GenericResponseProvider<object,IEnumerable<DiagramDevice>>((o) =>
            {
                this.designerCanvas.LoadDiagramDevices(o,this.ViewModel.AvailableDevices);
                return null;
            });

            this.ViewModel.ProjectMapPathProvider = new GenericResponseProvider<string, object>(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = Strings.SR.SvgFilesOpenFileDialogFilter };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.ProjectMapLoadedProvider = new GenericResponseProvider<object, Tuple<string, DiagramItemMetadata>>(o =>
            {
                if (o == null)
                    return null;
                return this.LoadMap(o.Item1,o.Item2);
            });

            this.ViewModel.ProjectMapRemoveProvider = new GenericResponseProvider<object, object>(o =>
            {
                this.RemoveMap();
                return null;
            });

            this.ViewModel.NewProjectLoadedProvider = new GenericResponseProvider<object, object>((o) =>
            {
                this.designerCanvas.ClearCanvas();
                this.designerCanvas.DeleteSelection();
                return null;
            });

            this.ViewModel.DeviceSelectionSubscriber = this;

            this.ViewModel.ListValueResponseProvider = new ListInputValuePicker();

            this.ViewModel.NewProjectLoadEnsureResponseProvider = new GenericResponseProvider<bool, object>((o) =>
            {
                var popup = new SimpleYesNoPopup(Strings.SR.AreYouSure, "", Popups.Icons.WarningIcon, null, null);
                var rp = new YesNoPopupResponseProvider(popup);
                return rp.ProvideResponse();
            });

            this.ViewModel.ActionEnsureResponseProvider = new GenericResponseProvider<bool, object>((o) =>
            {
                var popup = new SimpleYesNoPopup(Strings.SR.AreYouSure, "", Popups.Icons.WarningIcon, null, null);
                var rp = new YesNoPopupResponseProvider(popup);
                return rp.ProvideResponse();
            });

            this.ViewModel.SavingProgressBarResponseProvider = new GenericResponseProvider<YesNoProgressBarPopupResponseProvider, object>((o) =>
            {
                var savepopup = new SimpleYesNoProgressBarPopup(Strings.SR.GPSaving+"...", "", Popups.Icons.InfoIcon, null, null, 0, 0, 0, false, false);
                return new YesNoProgressBarPopupResponseProvider(savepopup);
            });

            this.ViewModel.LoadingProgressBarResponseProvider = new GenericResponseProvider<YesNoProgressBarPopupResponseProvider, object>((o) =>
            {
                var savepopup = new SimpleYesNoProgressBarPopup(Strings.SR.GPLoading+"...", "", Popups.Icons.InfoIcon, null, null, 0, 0, 0, false, false);
                return new YesNoProgressBarPopupResponseProvider(savepopup);
            });

            this.ViewModel.ApplicationSettingsResponseProvider = new GenericResponseProvider<Task<ApplicationSettings>, ApplicationSettings>(async (o) =>
            {
                var vm = new ApplicationSettingsViewModel(o);
                var window = new AppSettingsWindow();
                
                return (await window.ProvideResponse(vm)).Model;
            });

            this.ViewModel.NetworkRemovedResponseProvider = new GenericResponseProvider<bool, NetworkViewModel>((o) =>
            {
                var vms = o.GetDeviceViewModels();
                foreach (var item in vms)
                {
                    this.designerCanvas.RemoveDevice(item);
                }
                foreach(var item in o.GetSources())
                {
                    this.designerCanvas.RemoveDevice(item.GetCacheId());
                }
                this.designerCanvas.RemoveDevice(o.Coordinator);
                this.designerCanvas.RemoveNetworkGhostDevices(o.Guid);
                return true;
            });
        }

        public void NotifySelected(DeviceViewModel obj)
        {
            this.designerCanvas.AddDevice(obj,new Point(0,0));
        }

        public void NotifyUpdated(DeviceViewModel obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.designerCanvas.UpdateDevice(obj);
            });
        }

        private object RemoveMap()
        {
            this.designerCanvas.RemoveBackground();
            return null;
        }

        private object LoadMap(string path, DiagramItemMetadata metadata)
        {
            if (path == null)
            {
                return null;
            }
            if (path == string.Empty)
            {
                return null;
            }
            if(metadata==null)
            {
                return null;
            }
            else
            {
                if(double.IsNaN(metadata.Size.Width))
                {
                    metadata.Size = new Size() { Width = this.designerCanvas.ActualWidth, Height = metadata.Size.Height };
                }
                if(double.IsNaN(metadata.Size.Height))
                {
                    metadata.Size = new Size() { Width = metadata.Size.Width, Height = this.designerCanvas.ActualHeight};
                }
            }
            var temp = path.Split(".");
            if (temp.Length < 2)
            {
                return null;
            }
            else
            {
                var extension = temp[temp.Length - 1];
                //this.BackgroundCanvas.Children.Remove(this.map);
                switch(extension)
                {
                    case "svg":
                        var mapBackground = new SvgViewbox() { Source = new Uri(path) };
                        this.designerCanvas.SetBackground(mapBackground, metadata);
                        break;
                    default:
                        try
                        {
                            var uri = new Uri("file://" + path);
                            var bitmap = new BitmapImage(uri);
                            var img = new Image() { Source = bitmap };
                            this.designerCanvas.SetBackground(img,metadata);
                        }
                        catch(Exception ex)
                        {

                        }
                        break;
                }
                //this.map.HorizontalAlignment = HorizontalAlignment.Left;
                //this.map.VerticalAlignment = VerticalAlignment.Top;
                //this.BackgroundCanvas.Children.Add(this.map);
                return null;
            }
        }

        private void ResizeMapMenuItemClick(object sender, RoutedEventArgs e)
        {
            if(this.designerCanvas.GetBackground()==null)
            {
                return;
            }
            var vm = new MapResizeViewModel(this.designerCanvas.GetMapMetadata());
            vm.OnDiagramItemMedatadaChanged = new Action<DiagramItemMetadata>((o) =>
            {
                this.designerCanvas.UpdateBackgroundMetadata(o);
            });
            var window = new MapResizeWindow(vm);
            window.Show();
        }

        
    }
}
