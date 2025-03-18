using System;
using System.Threading.Tasks;

namespace MVVMEssential
{
    public class AsyncRelayCommand : AsyncBaseCommand
    {
        private readonly Func<object, Task> _callback;

        public AsyncRelayCommand(Func<object, Task> callback)
        {
            _callback = callback ?? throw new ArgumentNullException($"{nameof(callback)}");
        }

        protected override async Task ExecuteAsync(object parameter) => await _callback(parameter);
    }
}
