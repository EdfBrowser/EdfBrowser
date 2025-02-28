using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal abstract class AsyncBaseCommand : ICommand
    {
        private bool _isExecuting;

        protected AsyncBaseCommand() { }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting;
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            await ExecuteAsync(parameter);

            _isExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
