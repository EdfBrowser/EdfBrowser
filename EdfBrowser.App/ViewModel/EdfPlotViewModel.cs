using System;

namespace EdfBrowser.App
{
    internal class EdfPlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;

        internal EdfPlotViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;
            _edfStore.EdfFilePathChanged += OnEdfFilePathChanged;

            PlotViewModel = new PlotViewModel(edfStore);
            TimelineViewModel = new TimelineViewModel();
            TimelineViewModel.TimelineValueChanged += OnTimelineValueChanged;
        }

        internal PlotViewModel PlotViewModel { get; }
        internal TimelineViewModel TimelineViewModel { get; }

        protected override void Dispose(bool disposing)
        {
            _edfStore.EdfFilePathChanged -= OnEdfFilePathChanged;
            TimelineViewModel.TimelineValueChanged -= OnTimelineValueChanged;
        }

        private async void OnEdfFilePathChanged(object sender, EventArgs e)
        {
            await _edfStore.ReadInfo();

            double totalDuration = _edfStore.EdfInfo._recordDuration * _edfStore.EdfInfo._recordCount;
            TimelineViewModel.MaxValue = totalDuration;

            PlotViewModel.ResetCommnad.Execute(null);
        }

        private void OnTimelineValueChanged(object sender, uint e)
        {
            PlotViewModel.ReadSamplesCommnad.Execute(e);
        }
    }
}
