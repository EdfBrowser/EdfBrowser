using System;

namespace MVVMEssential
{
    public class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException($"{nameof(execute)}");
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
