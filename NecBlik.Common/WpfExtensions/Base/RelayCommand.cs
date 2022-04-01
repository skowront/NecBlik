using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace NecBlik.Common.WpfExtensions.Base
{ 
    /// <summary>
    /// Base RelayCommand implementation
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Default action to be taken. Just an empty function.
        /// </summary>
        public static Action<object> Default = o => { };

        /// <summary>
        /// Action that will be called on executing.
        /// </summary>
        private Action<object> execute;
        
        /// <summary>
        /// Function that defines if the action may execute or not.
        /// </summary>
        private Func<object, bool> canExecute;

        /// <summary>
        /// CanExecute Eventhandler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="execute">Action to be taken when executing</param>
        /// <param name="canExecute">Function that defines if the action may execute</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Checks if action can execute
        /// </summary>
        /// <param name="parameter">Context to pass to action</param>
        /// <returns>True if action can execute</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Executes the given callback
        /// </summary>
        /// <param name="parameter">Parameter to be passed to callback</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
