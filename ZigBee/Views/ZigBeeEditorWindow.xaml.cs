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
using ZigBee.Common.WpfExtensions.Base;
using ZigBee.Common.WpfExtensions.Interfaces;
using ZigBee.Core.GUI;
using ZigBee.ViewModels;

namespace ZigBee.Views
{
    /// <summary>
    /// Interaction logic for ZigBeeEditorWindow.xaml
    /// </summary>
    public partial class ZigBeeEditorWindow : Window
    {
        public ZigBeeViewModel ViewModel { get; set; }

        public ZigBeeEditorWindow()
        {
            InitializeComponent();
        }

        private Action OnConfirm;
        private Action OnCancel;

        public RelayCommand OnConfirmCommand { get; set; }
        public RelayCommand OnCancelCommand { get; set; }

        public ZigBeeEditorWindow(ZigBeeViewModel zigBeeViewModel, Action onConfirm = null, Action onCancel = null)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ViewModel = zigBeeViewModel;
            this.OnConfirm = onConfirm;
            this.OnCancel = onCancel;
            this.buildCommands();
        }

        private void buildCommands()
        {
            this.OnConfirmCommand = new RelayCommand(o => { this.OnConfirm?.Invoke(); this.Close(); });
            this.OnCancelCommand = new RelayCommand(o => { this.OnCancel?.Invoke(); this.Close(); });
        }
    }
}
