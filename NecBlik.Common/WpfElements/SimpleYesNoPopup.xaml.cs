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
    /// Universal popup class
    /// </summary>
    public partial class SimpleYesNoPopup : Window
    {
        public SimpleYesNoPopupViewModel ViewModel { get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public SimpleYesNoPopup()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Ctor with building parameters
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title</param>
        /// <param name="icon">Icon</param>
        /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
        /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
        public SimpleYesNoPopup(string message,string title, Popups.ZigBeeIcon icon, Action OnConfirm=null, Action OnCancel=null)
        {
            InitializeComponent();
            this.ViewModel = new SimpleYesNoPopupViewModel(this, message, title, icon, OnConfirm, OnCancel);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// Viewmodel
        /// </summary>
        public class SimpleYesNoPopupViewModel : WindowViewModel
        {
            /// <summary>
            /// Captured value
            /// </summary>
            private string value = string.Empty;
            public string Value
            {
                get { return this.value; }
                set { this.value = value; this.OnPropertyChanged(); }
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

            public Popups.ZigBeeIcon Icon { get; set; } = Popups.Icons.InfoIcon;

            public RelayCommand ConfirmCommand { get; set; }
            public RelayCommand CancelCommand { get; set; }

            public Action OnConfirm = null;
            public Action OnCancel = null;

            /// <summary>
            /// Ctor with building parameters
            /// </summary>
            /// <param name="window">Window for which the controll is attached to</param>
            /// <param name="message">Message</param>
            /// <param name="title">Top title</param>
            /// <param name="icon">Icon to be displayed</param>
            /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
            /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
            public SimpleYesNoPopupViewModel(Window window,string message,string title, Popups.ZigBeeIcon icon, Action OnConfirm, Action OnCancel) : base(window)
            {
                this.Message = message;
                this.Title = title;
                window.Title = title;
                this.Icon = icon;
                this.OnConfirm = OnConfirm;
                this.OnCancel = OnCancel;
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
