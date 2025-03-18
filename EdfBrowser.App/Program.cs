using EdfBrowser.Services;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class Program
    {
        private readonly MainView _mainView;
        private readonly MainViewModel _mainViewModel;
        private readonly EdfStore _edfStore;
        private readonly NavigationStore _navigationStore;
        private readonly IMenuService _menuService;
        private readonly IEdfParserService _edfParserService;

        internal Program()
        {
            _menuService = new MenuService();
            _edfParserService = new EdfParserService();

            _edfStore = new EdfStore(_edfParserService);

            _navigationStore = new NavigationStore();

            _mainViewModel = new MainViewModel(_navigationStore);
            _mainView = new MainView(_mainViewModel);

            var navigation = CreateFileViewNavigationService();
            navigation.Navigate();
        }

        [STAThread]
        static void Main()
        {
#if NETFRAMEWORK
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#elif NET
            ApplicationConfiguration.Initialize();
#endif
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            Application.Run(_mainView);
        }

        private NavigationService<FileViewModel> CreateFileViewNavigationService()
        {
            return new NavigationService<FileViewModel>(_navigationStore,
                () => new FileViewModel(_edfStore, CreateSignalListNavigationService()));
        }

        private NavigationService<SignalListViewModel> CreateSignalListNavigationService()
        {
            return new NavigationService<SignalListViewModel>(_navigationStore,
                () => new SignalListViewModel(_edfStore, CreateEdfPlotNavigationService()));
        }

        private NavigationService<EdfPlotViewModel> CreateEdfPlotNavigationService()
        {
            return new NavigationService<EdfPlotViewModel>(_navigationStore,
                () => new EdfPlotViewModel(_edfStore));
        }
    }
}
