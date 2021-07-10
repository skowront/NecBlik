using DiagramDesigner;
using Microsoft.Win32;
using Newtonsoft.Json;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Models;
using ZigBee.ViewModels;
using ZigBee.Views.Controls;

namespace ZigBee.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint = new Point();

        private MainWindowViewModel ViewModel;

        private UIElement draggedItem;

        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = new MainWindowViewModel();
            this.DataContext = this.ViewModel;
            this.buildResponseProviders();
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
                var vm = (ZigBeeViewModel)lv.ItemContainerGenerator.ItemFromContainer(lvi);
                var xaml = XamlWriter.Save(new ZigBeeControl(vm));
                //DragObject dataObject = new DragObject();
                //dataObject.Xaml = xaml; 
                //WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                //if (panel != null)
                //{
                //    // desired size for DesignerCanvas is the stretched Toolbox item size
                //    double scale = 1.3;
                //    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                //}

                DragDrop.DoDragDrop(lvi, new DataObject(vm.GetType(),vm), DragDropEffects.Copy);
                //DragDrop.DoDragDrop(lvi, dataObject, DragDropEffects.Copy);
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
                this.designerCanvas.ClearSelection();
            }
        }

        private void buildResponseProviders()
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
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = false, Filter = "Text files (*.json)|*.json" };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.LoadProjectFilePathProvider = new GenericResponseProvider<string, object>(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = "Text files (*.json)|*.json" };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.DiagramZigBeesProivider = new GenericResponseProvider<IEnumerable<DiagramZigBee>, object>((o) =>
            {
                return this.designerCanvas.GetDiagramZigBees();
            });

            this.ViewModel.DiagramZigBeesLoadedProvider = new GenericResponseProvider<object,IEnumerable<DiagramZigBee>>((o) =>
            {
                //this.designerCanvas.LoadDiagramZigBees(o,this.ViewModel.AvailableZigBees);
                return null;
            });

            this.ViewModel.ProjectMapPathProvider = new GenericResponseProvider<string, object>(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = true, Filter = "Text files (*.svg)|*.svg" };
                openFileDialog.ShowDialog();
                return openFileDialog.FileName;
            });

            this.ViewModel.ProjectMapLoadedProvider = new GenericResponseProvider<object, string>(o =>
            {
                if (o == null)
                {
                    return null;
                }
                if (o == string.Empty)
                {
                    return null;
                }
                var mapBackground = new SvgViewbox() { Source = new Uri(o) };
                this.designerCanvas.SetBackground(mapBackground);
                return null;
            });

            //this.ViewModel.OnProjectSaved = new Action(() =>
            //{
            //    var diagramZigBees = JsonConvert.SerializeObject(this.designerCanvas.GetDiagramZigBees());
            //    File.WriteAllText(this.ViewModel.ZigBeesDiagramJsonFile,this.ViewModel.ZigBeesDiagramJsonFile);
            //    return;
            //});

        }
    }
}
