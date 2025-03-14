using EdfBrowser.Services;

namespace EdfBrowser.App
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        internal MainViewModel(IMenuService menuService, EdfStore edfStore, NavigationStore navigationStore)
        {
            MenuViewModel = MenuViewModel.LoadMenus(menuService, edfStore);
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        protected override void Dispose(bool disposing)
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
        }

        internal MenuViewModel MenuViewModel { get; }
        internal BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    }
}
