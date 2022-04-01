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
using System.Collections.ObjectModel;

namespace NecBlik.Common.WpfElements
{
    /// <summary>
    /// Interaction logic for SimpleInputPopup.xaml
    /// Universal popup class
    /// </summary>
    public partial class SimpleListInputPopup : Window
    {
        public SimpleListInputViewModel ViewModel { get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public SimpleListInputPopup()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Ctor with building parameters
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Top title</param>
        /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
        /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
        public SimpleListInputPopup(string message, string title, IEnumerable<string> availableValues , Action<string> OnConfirm, Action<string> OnCancel)
        {
            InitializeComponent();
            this.ViewModel = new SimpleListInputViewModel(this, message, title, availableValues,  OnConfirm, OnCancel);
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public class SimpleListInputViewModel : WindowViewModel
        {
            /// <summary>
            /// Captured value holder
            /// </summary>
            private string value = string.Empty;
            public string Value
            {
                get { return this.value; }
                set { this.value = value; this.OnPropertyChanged(); }
            }

            public ObservableCollection<string> Values { get; set; } = new ObservableCollection<string>();

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

            public RelayCommand ConfirmCommand { get; set; }
            public RelayCommand CancelCommand { get; set; }

            public Action<string> OnConfirm = null;

            public Action<string> OnCancel = null;

            /// <summary>
            /// Ctor with building parameters
            /// </summary>
            /// <param name="window">Window for which the controll is attached to</param>
            /// <param name="message">Message</param>
            /// <param name="title">Top title</param>
            /// <param name="OnConfirm">Action (callback) to be taken when confirmed</param>
            /// <param name="OnCancel">Action (callback) to be taken when canceled</param>
            public SimpleListInputViewModel(Window window, string message, string title, IEnumerable<string> availableValues, Action<string> OnConfirm, Action<string> OnCancel) : base(window)
            {
                this.Message = message;
                this.Title = title;
                this.OnConfirm = OnConfirm;
                this.OnCancel = OnCancel;
                foreach(var item in availableValues)
                {
                    this.Values.Add(item);
                }
                this.ConfirmCommand = new RelayCommand(o =>
                {
                    this.OnConfirm?.Invoke(this.value);
                    window.Close();
                });
                this.CancelCommand = new RelayCommand(o =>
                {
                    this.OnCancel?.Invoke(this.value);
                    window.Close();
                });
            }
        }
    }
}
