using NecBlik.Common.WpfExtensions;
using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NecBlik.Common.WpfElements
{
    /// <summary>
    /// Interaction logic for SimpleInputPopup.xaml
    /// Universal popup class that represents a progress bar
    /// </summary>
    public partial class SimpleYesNoProgressBarPopup
    {
        public SimpleYesNoProgressBarPopupViewModel ViewModel { get; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        public SimpleYesNoProgressBarPopup()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Ctor with building parameters
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title</param>
        /// <param name="icon">Icon</param>
        /// <param name="OnConfirm">Action to be taken on confirm click</param>
        /// <param name="OnCancel">Action to be taken on cancel click</param>
        /// <param name="min">Minimum progressbar value</param>
        /// <param name="value">Starting progressbar value</param>
        /// <param name="max">Maximum progressbar value</param>
        /// <param name="canClose">True if the box is closable by ok click</param>
        /// <param name="canCloseAbort">True if the box is closable by abort click</param>
        public SimpleYesNoProgressBarPopup(string message, string title, Popups.ZigBeeIcon icon, Action OnConfirm, Action OnCancel, int min,int value, int max, bool canClose = true, bool canCloseAbort = true)
        {
            InitializeComponent();
            this.progressBar.Minimum = min;
            this.progressBar.Maximum = max;
            this.ViewModel = new SimpleYesNoProgressBarPopupViewModel(this, message, title, icon, OnConfirm, OnCancel, min, value, max, canClose, canCloseAbort);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// Safe function that can set a new progressbar value.
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressValue(int value)
        {
            (this.DataContext as SimpleYesNoProgressBarPopupViewModel).Value = value;
        }

        /// <summary>
        /// Safe function that can set a new progressbar value.
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressValueDelta(int value)
        {
            (this.DataContext as SimpleYesNoProgressBarPopupViewModel).Value += value;
        }

        /// <summary>
        /// Safe function that enables window closure on ok click.
        /// </summary>
        /// <param name="value">New value of click bool</param>
        public void SetClosureEnabled(bool value)
        {
            (this.DataContext as SimpleYesNoProgressBarPopupViewModel).CanClose = value;
        }

        /// <summary>
        /// Safe function that enables window closure on cancel click.
        /// </summary>
        /// <param name="value">New value of click bool</param>
        public void SetClosureAbortEnabled(bool value)
        {
            (this.DataContext as SimpleYesNoProgressBarPopupViewModel).CanClose = value;
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public class SimpleYesNoProgressBarPopupViewModel : WindowViewModel
        {
            private int value = 50;
            public int Value
            {
                get { return this.value; }
                set { this.value = value; this.OnPropertyChanged(); }
            }

            private int min = 0;
            public int Min
            {
                get { return this.min; }
                set { this.min = value; this.OnPropertyChanged(); }
            }

            private int max = 100;
            public int Max
            {
                get { return this.max; }
                set { this.max = value; this.OnPropertyChanged(); }
            }

            private string message = string.Empty;
            public string Message
            {
                get { return this.message; }
                set { this.message = value; this.OnPropertyChanged(); }
            }

            private string title = string.Empty;
            public string Title
            {
                get { return this.title; }
                set { this.title = value; this.OnPropertyChanged(); }
            }

            public bool canClose = true;
            public bool CanClose
            {
                get { return this.canClose; }
                set { this.canClose = value; this.OnPropertyChanged(); }
            }

            public bool canCloseAbort = true;
            public bool CanCloseAbort
            {
                get { return this.canCloseAbort; }
                set { this.canCloseAbort = value; this.OnPropertyChanged(); }
            }

            public Popups.ZigBeeIcon Icon { get; set; } = Popups.Icons.InfoIcon;

            public RelayCommand ConfirmCommand { get; set; }
            public RelayCommand CancelCommand { get; set; }

            public Action OnConfirm = null;
            public Action OnCancel = null;

            /// <summary>
            /// Ctor with building parameters
            /// </summary>
            /// <param name="window">Window that the popup is attached to</param>
            /// <param name="message">Message to be displayed</param>
            /// <param name="title">Title</param>
            /// <param name="icon">Icon</param>
            /// <param name="OnConfirm">Action to be taken on confirm click</param>
            /// <param name="OnCancel">Action to be taken on cancel click</param>
            /// <param name="min">Minimum progressbar value</param>
            /// <param name="value">Starting progressbar value</param>
            /// <param name="max">Maximum progressbar value</param>
            /// <param name="canClose">True if the box is closable by ok click</param>
            /// <param name="canCloseAbort">True if the box is closable by abort click</param>
            public SimpleYesNoProgressBarPopupViewModel(Window window, string message, string title, Popups.ZigBeeIcon icon, Action OnConfirm, Action OnCancel, int min, int value, int max, bool canClose, bool canCloseAbort) : base(window)
            {
                this.Message = message;
                this.Title = title;
                window.Title = title;
                this.Icon = icon;
                this.OnConfirm = OnConfirm;
                this.OnCancel = OnCancel;
                this.Value = value;
                this.canClose = canClose;
                this.canCloseAbort = canCloseAbort;
                this.min = min;
                this.max = max;
                this.ConfirmCommand = new RelayCommand(o =>
                {
                    this.OnConfirm?.Invoke();
                    window.Close();
                });
                this.CancelCommand = new RelayCommand(o =>
                {
                    this.OnCancel?.Invoke();
                    window.Close();
                });
            }
        }
    }
}
