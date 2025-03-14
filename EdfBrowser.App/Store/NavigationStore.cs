using System;

namespace EdfBrowser.App
{
    internal class NavigationStore
    {
        private BaseViewModel _currentViewModel;

        internal BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel?.Dispose();
                    _currentViewModel = value;

                    OnCurrentViewModelChanged();
                }
            }
        }

        internal event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
