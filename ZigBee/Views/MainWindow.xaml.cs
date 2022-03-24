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
using ZigBee.Common.WpfElements;
using ZigBee.Common.WpfElements.PopupValuePickers;
using ZigBee.Common.WpfElements.ResponseProviders;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI;
using ZigBee.Core.GUI.Interfaces;
using ZigBee.Core.GUI.Models;
using ZigBee.Models;
using ZigBee.ViewModels;
using ZigBee.Views.Controls;

namespace ZigBee.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISelectionSubscriber<ZigBeeViewModel>
    {
        private Point startPoint = new Point();

        private MainWindowViewModel ViewModel;

        private UIElement draggedItem;

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
                if(lv.ItemContainerGenerator.ItemFromContainer(lvi) is ZigBeeViewModel)
                {
                    var vm = (ZigBeeViewModel)lv.ItemContainerGenerator.ItemFromContainer(lvi);
                    var xaml = XamlWriter.Save(ZigBee.Core.GUI.Factories.ZigBeeGuiAnyFactory.Instance.GetZigBeeControl(vm));
                    //DragObject dataObject = new DragObject();
                    //dataObject.Xaml = xaml; 
                    //WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                    //if (panel != null)
                    //{
                    //    // desired size for DesignerCanvas is the stretched Toolbox item size
                    //    double scale = 1.3;
                    //    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                    //}

                    DragDrop.DoDragDrop(lvi, new DataObject(vm.GetType(), vm), DragDropEffects.Copy);
                    //DragDrop.DoDragDrop(lvi, dataObject, DragDropEffects.Copy);
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
            this.ViewModel.NewZigBeeResponseProvider = new GenericResponseProvider<ZigBeeViewModel, ZigBeeViewModel>((o) =>
            {
                bool result = false;
                var window = new ZigBeeEditorWindow(o, new Action(() => result = true), new Action(() => result = false));
                window.ShowDialog();
                if (result == false)
                {
                    return null;
                }
                return o;
            });

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
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = Strings.SR.JsonFilesOpenFileDialodFilter };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.DiagramZigBeesProvider = new GenericResponseProvider<IEnumerable<DiagramZigBee>, object>((o) =>
            {
                return this.designerCanvas.GetDiagramZigBees();
            });

            this.ViewModel.DiagramMapMetadataProvider = new GenericResponseProvider<DiagramItemMetadata, object>((o) =>
            {
                return this.designerCanvas.GetMapMetadata();
            });

            this.ViewModel.DiagramZigBeesLoadProvider = new GenericResponseProvider<object,IEnumerable<DiagramZigBee>>((o) =>
            {
                this.designerCanvas.LoadDiagramZigBees(o,this.ViewModel.AvailableZigBees);
                return null;
            });

            this.ViewModel.ProjectMapPathProvider = new GenericResponseProvider<string, object>(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = Strings.SR.SvgFilesOpenFileDialodFilter };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.ProjectMapLoadedProvider = new GenericResponseProvider<object, Tuple<string, DiagramItemMetadata>>(o =>
            {
                if (o == null)
                {
                    return null;
                }
                if(o.Item1==null)
                {
                    return null;
                }
                if (o.Item1 == string.Empty)
                {
                    return null;
                }
                var mapBackground = new SvgViewbox() { Source = new Uri(o.Item1) };
                this.designerCanvas.SetBackground(mapBackground, o.Item2);
                return null;
            });

            this.ViewModel.NewProjectLoadedProvider = new GenericResponseProvider<object, object>((o) =>
            {
                this.designerCanvas.ClearCanvas();
                this.designerCanvas.DeleteSelection();
                return null;
            });

            this.ViewModel.ZigBeeSelectionSubscriber = this;

            this.ViewModel.ListValueResponseProvider = new ListInputValuePicker();

            this.ViewModel.NewProjectLoadEnsureResponseProvider = new GenericResponseProvider<bool, object>((o) =>
            {
                var popup = new SimpleYesNoPopup(Strings.SR.AreYouSure, "", Popups.ZigBeeIcons.WarningIcon, null, null);
                var rp = new YesNoPopupResponseProvider(popup);
                return rp.ProvideResponse();
            });

            this.ViewModel.SavingProgressBarResponseProvider = new GenericResponseProvider<YesNoProgressBarPopupResponseProvider, object>((o) =>
            {
                var savepopup = new SimpleYesNoProgressBarPopup(Strings.SR.GPSaving+"...", "", Popups.ZigBeeIcons.InfoIcon, null, null, 0, 0, 0, false, false);
                return new YesNoProgressBarPopupResponseProvider(savepopup);
            });

            this.ViewModel.LoadingProgressBarResponseProvider = new GenericResponseProvider<YesNoProgressBarPopupResponseProvider, object>((o) =>
            {
                var savepopup = new SimpleYesNoProgressBarPopup(Strings.SR.GPLoading+"...", "", Popups.ZigBeeIcons.InfoIcon, null, null, 0, 0, 0, false, false);
                return new YesNoProgressBarPopupResponseProvider(savepopup);
            });

            this.ViewModel.ApplicationSettingsResponseProvider = new GenericResponseProvider<Task<ApplicationSettings>, ApplicationSettings>(async (o) =>
            {
                var vm = new ApplicationSettingsViewModel(o);
                var window = new AppSettingsWindow();
                
                return (await window.ProvideResponse(vm)).Model;
            });

            //this.ViewModel.OnProjectSaved = new Action(() =>
            //{
            //    var diagramZigBees = JsonConvert.SerializeObject(this.designerCanvas.GetDiagramZigBees());
            //    File.WriteAllText(this.ViewModel.ZigBeesDiagramJsonFile,this.ViewModel.ZigBeesDiagramJsonFile);
            //    return;
            //});

        }

        public void NotifySelected(ZigBeeViewModel obj)
        {
            this.designerCanvas.AddZigBee(obj,new Point(0,0));
        }
    }
}
