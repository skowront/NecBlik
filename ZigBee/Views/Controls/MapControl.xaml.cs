using SharpVectors.Converters;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZigBee.Core.GUI;
using ZigBee.Models;
using ZigBee.ViewModels;
using ZigBee.Views.Controls;

namespace ZigBee.Views.Controls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class MapControl : UserControl, INotifyPropertyChanged
    {
        private SvgViewbox mapBackground;

        //private Map map;

        //Notify Property Changed Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private bool isZoomEnabled = true;
        public bool IsZoomEnabled
        {
            get { return this.isZoomEnabled; }
            set { this.isZoomEnabled = value; this.OnPropertyChanged(); }
        }

        public MapControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void LoadMapFromSvg(string path)
        {
            if (path == null)
            {
                return;
            }
            if (path == string.Empty)
            {
                return;
            }
            mapBackground = new SvgViewbox() { Source = new Uri(path) };
            if (this.mapBackground!=null)
            {
                this.mapCanvas.Children.Remove(mapBackground);
            }
            this.mapCanvas.Children.Add(mapBackground);
        }

        //public void LoadMap(Map map)
        //{
        //    this.map = map;
        //    if(this.map==null)
        //    {
        //        return;
        //    }
        //    this.ClearCanvas();
        //    this.LoadMapFromSvg(this.map.MapPath);
        //}

        private void ClearCanvas()
        {
            this.mapCanvas.Children.Clear();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetData(typeof(ZigBeeViewModel)) is ZigBeeViewModel)
            {

            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.Key == Key.Tab)
            {
                this.IsZoomEnabled = true;
            }
        }


    }
}
