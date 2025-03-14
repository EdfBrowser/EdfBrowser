using System;

namespace EdfBrowser.App
{
    internal class NavigationService<TViewModel> where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        internal NavigationService(NavigationStore navigationStore,
            Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        internal void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
