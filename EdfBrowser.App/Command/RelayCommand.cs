using System;

namespace EdfBrowser.App
{
    internal class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        internal RelayCommand(Action<object> execute)
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
