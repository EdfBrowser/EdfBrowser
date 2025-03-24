using MVVMEssential;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class EdfPlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;
        private readonly NavigationService<SignalListViewModel> _navigationService;

        public EdfPlotViewModel(
            EdfStore edfStore,
            PlotViewModel plotViewModel,
            TimelineViewModel timelineViewModel,
            NavigationService<SignalListViewModel> navigationService)
        {
            _edfStore = edfStore;

            PlotViewModel = plotViewModel;
            TimelineViewModel = timelineViewModel;
            TimelineViewModel.TimelineValueChanged += OnTimelineValueChanged;

            _navigationService = navigationService;

            double totalDuration = _edfStore.TotalDuration;
            TimelineViewModel.MaxValue = totalDuration;

            BackwardCommand = new RelayCommand(Backward);
        }

        internal PlotViewModel PlotViewModel { get; }
        internal TimelineViewModel TimelineViewModel { get; }
        internal ICommand BackwardCommand { get; }

        protected override void Dispose(bool disposing)
        {
            TimelineViewModel.TimelineValueChanged -= OnTimelineValueChanged;
        }

        private uint _oldVal = 0u;
        readonly RecordRange _range = new RecordRange();

        private void OnTimelineValueChanged(object sender, uint e)
        {
            _range.Start = _oldVal;
            _range.End = e;

            PlotViewModel.ReadSamplesCommnad.Execute(_range);

            _oldVal = e;
        }

        private void Backward(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
