using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Core.GUI.Models;

namespace ZigBee.ViewModels
{
    public class MapResizeViewModel : BaseViewModel
    {
        private const double SLIDER_MAX = 3000;

        public DiagramItemMetadata model;

        public double Width
        {
            get
            {
                return model.Size.Width;
            }
            set
            {
                if (maintainAspectRatio == false)
                {
                    model.Size = new Size() { Height = model.Size.Height, Width = value };

                }
                else
                {
                    var ratio = value / model.Size.Width;
                    model.Size = new Size() { Height = model.Size.Height * ratio, Width = value };
                }
                if (value > this.MaxWidth)
                {
                    this.MaxWidth = value;
                }
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Height));
            }
        }

        public double Height
        {
            get
            {
                return model.Size.Height;
            }
            set
            {
                if (maintainAspectRatio == false)
                {
                    model.Size = new Size() { Height = value, Width = model.Size.Width };
                }
                else
                {
                    var ratio = value / model.Size.Height;
                    model.Size = new Size() { Height = value, Width = model.Size.Width * ratio };
                }
                if(value>this.MaxHeight)
                {
                    this.MaxHeight = value; 
                }
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Width));
            }
        }

        private bool maintainAspectRatio = false;
        public bool MaintainAspectRatio
        {
            get { return maintainAspectRatio; }
            set { maintainAspectRatio = value; this.OnPropertyChanged(); }
        }

        private double maxWidth = SLIDER_MAX;
        public double MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; this.OnPropertyChanged(); }
        }

        private double maxHeight = SLIDER_MAX;
        public double MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; this.OnPropertyChanged(); }
        }

        public Action<DiagramItemMetadata> OnDiagramItemMedatadaChanged;

        public MapResizeViewModel(DiagramItemMetadata model)
        {
            this.model = model;
            if (this.model.Size.Width > this.MaxWidth)
            {
                this.MaxWidth = this.model.Size.Width;
            }
            if (this.model.Size.Height > this.MaxHeight)
            {
                this.MaxHeight = this.model.Size.Height;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string property = null)
        {
            base.OnPropertyChanged(property);
            this.OnDiagramItemMedatadaChanged?.Invoke(this.model);
        }
    }
}
