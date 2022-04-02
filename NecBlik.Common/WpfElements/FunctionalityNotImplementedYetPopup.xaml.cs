using NecBlik.Common.WpfExtensions.Base;
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
    /// Interaction logic for FunctionalityNotImplementedYetPopup.xaml
    /// </summary>
    public partial class FunctionalityNotImplementedYetPopup : Window
    {
        public FunctionalityNotImplementedYetPopup()
        {
            InitializeComponent();
            this.DataContext = new FunctionalityNotImplementedYetPopupViewModel(this);
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public class FunctionalityNotImplementedYetPopupViewModel : WindowViewModel
        {
            public Popups.ZigBeeIcon Icon { get; set; } = Popups.Icons.InfoIcon;

            public RelayCommand ConfirmCommand { get; set; }

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="window">Window that the popup is attached to</param>
            public FunctionalityNotImplementedYetPopupViewModel(Window window) : base(window)
            {
                this.ConfirmCommand = new RelayCommand(o =>
                {
                    window.Close();
                });
            }
        }
    }
}
