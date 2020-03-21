/**
 * @file    LoginViewModel.cs
 * @author  Trent Thompson & Drew Hoffer
 * 
 */

using System;
using System.Windows.Input;

namespace FAST_Converter.ViewModel
{
    /// <summary>
    /// Executes commands depending on what parameters it is given.  Essentially just 
    /// ButtonCommand.cs without the need for validation.
    /// </summary>
    /// <typeparam name="T">Type of command to be executed</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;
        private readonly Func<T, bool> _canExecute = null;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null) {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true);
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        public bool CanExecute(object parameter) => _canExecute((T)parameter);


        public void Execute(object parameter) => _execute((T)parameter);

        
    }

    public class RelayCommand:RelayCommand<object>
    {
        public RelayCommand(Action execute):base (_=>execute()) { }

        public RelayCommand(Action execute, Func<bool> canExecute):base(_ => execute(), _ => canExecute()) { }
    }
}