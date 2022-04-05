using NecBlik.Common.WpfExtensions.Base;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Factories;
using NecBlik.Core.GUI.Factories.ViewModels;
using NecBlik.Core.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace NecBlik.Core.GUI.Views
{
    /// <summary>
    /// Interaction logic for NetworkFactoryRulesEditor.xaml
    /// </summary>
    public partial class FactoryRulesEditor
    {
        public NetworkViewModel ViewModel = null;

        Action OnClose = null;

        public FactoryRulesEditor()
        {
            InitializeComponent();
        }

        public FactoryRulesEditor(IResponseProvider<ObservableCollection<FactoryRuleViewModel>, object> responseProvider,
            IResponseProvider<List<string>, FactoryRule> AvailableCacheObjectIDsProvider = null,
            IResponseProvider<List<string>, FactoryRule> AvailablePropertyProvider = null,
            IResponseProvider<List<string>, FactoryRule> AvailableValueProvider = null,
            Action OnClose = null)
        {
            InitializeComponent();
            var vm = new FactoryRulesEditorViewModel(responseProvider);
            vm.AvailablePropertyProvider = AvailablePropertyProvider;
            vm.AvailableValueProvider = AvailableValueProvider;
            vm.AvailableCacheObjectIDsProvider = AvailableCacheObjectIDsProvider;
            this.OnClose = OnClose;
            this.DataContext = vm;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.OnClose?.Invoke();
        }
    }
}
