using System;
using System.Threading.Tasks;

namespace EdfBrowser.App
{
    internal class AsyncRelayCommand : AsyncBaseCommand
    {
        private readonly Func<object, Task> _callback;

        internal AsyncRelayCommand(Func<object, Task> callback)
        {
            _callback = callback ?? throw new ArgumentNullException($"{nameof(callback)}");
        }

        protected override async Task ExecuteAsync(object parameter) => await _callback(parameter);
    }
}
