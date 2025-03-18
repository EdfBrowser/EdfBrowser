
namespace EdfBrowser.App
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        internal MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        internal BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        protected override void Dispose(bool disposing)
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
        }
    }
}
