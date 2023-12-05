using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Super_project.Core
{
    internal class RelayCommand: ICommand
    {
        private Action<object> _execute;
        private Func<object,bool> _canExecuteFunc;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecuteFunc)
        {
            _execute = execute;
            _canExecuteFunc = canExecuteFunc;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc==null||_canExecuteFunc(parameter);
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
