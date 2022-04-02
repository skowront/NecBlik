using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using NecBlik.Common.WpfExtensions;
using Fasetto.Word;

namespace NecBlik.Common.WpfExtensions.Base
{
    /// <summary>
    /// Author: https://github.com/angelsix/fasetto-word/tree/69f144a73638b5c789d8df155cd023a6ad68f5e7
    /// Some changes by: Tomasz Skowron https://github.com/skowront
    /// </summary>
    public class WindowViewModel:BaseViewModel
    {
        protected Window window;

        public int MinimumWindowWidth { get; set; } = 400;
        public int MinimumWindowHeight { get; set; } = 400;

        public int InnerContentPadding { get; set; } = 10;
        public Thickness InnerContentPaddingThickness
        {
            get { return new Thickness(this.InnerContentPadding); }
        }

        public int ResizeBorder { get; set; } = 6;

        public Thickness ResizeBorderThickness
        {
            get { return new Thickness(ResizeBorder + OuterMarginSize); }
        }

        private int outerMarginSize = 10;
        public int OuterMarginSize { 
            get
            {
                return window.WindowState == WindowState.Maximized ? 0 : outerMarginSize;
            }
            set
            {
                this.outerMarginSize = value; this.OnPropertyChanged();
            }
        }
        public Thickness OuterMarginSizeThickness
        {
            get { return new Thickness(OuterMarginSize); }
        }

        private int windowRadius = 10;
        public int WindowRadius
        {
            get
            {
                return window.WindowState == WindowState.Maximized ? 0 : windowRadius;
            }
            set
            {
                this.windowRadius = value; this.OnPropertyChanged();
            }
        }

        private int cornerRadius = 10;

        public int CornerRadiusInt
        {
            get { return this.cornerRadius; }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return new CornerRadius(this.cornerRadius);
            }
        }

        public int TitleHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength { get { return new GridLength(this.TitleHeight+this.ResizeBorder); } }

        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand OpenMenuWindowCommand { get; set; }

        public WindowViewModel(Window window)
        {
            this.window = window;
            window.StateChanged += (sender, e) =>
            {
                this.OnPropertyChanged(nameof(ResizeBorderThickness));
                this.OnPropertyChanged(nameof(OuterMarginSize));
                this.OnPropertyChanged(nameof(OuterMarginSizeThickness));
                this.OnPropertyChanged(nameof(WindowRadius));
                this.OnPropertyChanged(nameof(CornerRadius));
            };
            MinimizeWindowCommand = new RelayCommand((o)=> { window.WindowState = WindowState.Minimized; });
            MaximizeWindowCommand = new RelayCommand((o)=> { window.WindowState ^= WindowState.Maximized; });
            CloseWindowCommand = new RelayCommand((o)=> { window.Close(); });
            OpenMenuWindowCommand = new RelayCommand((o)=>  
                { 
                    SystemCommands.ShowSystemMenu(window, this.GetMousePosition());
                },
                (o) => 
                { 
                    return true;
                }
            );
            var Resizer = new WindowResizer(this.window);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        private Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
