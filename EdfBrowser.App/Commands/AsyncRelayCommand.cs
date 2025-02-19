using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;

        private bool _isExecuting;

        internal AsyncRelayCommand(Func<object, Task> execute)
        {
            _execute = execute ?? throw new ArgumentNullException($"{nameof(execute)}");
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting;
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            await _execute(parameter);

            _isExecuting = false;
        }
    }
}
