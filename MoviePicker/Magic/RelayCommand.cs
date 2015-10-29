using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MoviePicker.Magic
{
    [DebuggerStepThrough]
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> executeAction)
            : this(executeAction, null)
        { }
        public RelayCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc)
        {
            this.executeAction = executeAction;
            this.canExecuteFunc = canExecuteFunc;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067
        private readonly Action<object> executeAction;
        public readonly Func<object, bool> canExecuteFunc;

        public bool CanExecute(object parameter)
        {
            return this.canExecuteFunc?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            this.executeAction(parameter);
        }
    }
}