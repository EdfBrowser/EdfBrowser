namespace EdfBrowser.App
{
    internal class EdfPlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;

        internal EdfPlotViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;

            PlotViewModel = new PlotViewModel(edfStore);
            TimelineViewModel = new TimelineViewModel();
            TimelineViewModel.TimelineValueChanged += OnTimelineValueChanged;

            double totalDuration = _edfStore.EdfInfo._recordDuration * _edfStore.EdfInfo._recordCount;
            TimelineViewModel.MaxValue = totalDuration;

            PlotViewModel.ResetCommnad.Execute(null);
        }

        internal PlotViewModel PlotViewModel { get; }
        internal TimelineViewModel TimelineViewModel { get; }

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
    }
}
