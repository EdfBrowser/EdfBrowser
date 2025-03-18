using System.Threading.Tasks;

namespace MVVMEssential
{
    public abstract class AsyncBaseCommand : BaseCommand
    {
        private bool _isExecuting;

        public override bool CanExecute(object parameter)
        {
            return !_isExecuting && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            _isExecuting = true;

            await ExecuteAsync(parameter);

            _isExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
