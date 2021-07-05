using ZigBee.Common.WpfExtensions;
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
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

namespace ZigBee.Common.WpfElements
{
    /// <summary>
    /// Interaction logic for SimpleInputPopup.xaml
    /// Universal popup class
    /// </summary>
    public partial class SimpleYesPopup : Window
    {
        public SimpleYesPopupViewModel ViewModel { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public SimpleYesPopup()
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
        /// <param name="OnConfirm">Action to be taken when user clicks ok</param>
        public SimpleYesPopup(string message,string title, Popups.ZigBeeIcon icon, Action OnConfirm=null)
        {
            InitializeComponent();
            this.ViewModel = new SimpleYesPopupViewModel(this, message, title, icon, OnConfirm);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public class SimpleYesPopupViewModel : WindowViewModel
        {
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

            public Popups.ZigBeeIcon Icon { get; set; } = Popups.ZigBeeIcons.InfoIcon;

            public RelayCommand ConfirmCommand { get; set; }

            public Action OnConfirm = null;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="window">Window that the popup is attached to</param>
            /// <param name="message">Message</param>
            /// <param name="title">Title</param>
            /// <param name="icon">Icon</param>
            /// <param name="OnConfirm">Action to be taken when user clicks ok</param>
            public SimpleYesPopupViewModel(Window window,string message,string title, Popups.ZigBeeIcon icon, Action OnConfirm) : base(window)
            {
                this.Message = message;
                this.Title = title;
                window.Title = title;
                this.Icon = icon;
                this.OnConfirm = OnConfirm;
                this.ConfirmCommand = new RelayCommand(o =>
                {
                    this.OnConfirm?.Invoke();
                    window.Close();
                });
            }
        }
    }
}
