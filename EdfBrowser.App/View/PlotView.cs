using Plot.WinForm;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    // TODO: disposed old figureForm if the instance type is signleton
    internal class PlotView : BaseView
    {
        private readonly PlotViewService _viewService;
        private readonly FigureForm _figureForm;

        public PlotView()
        {
            _figureForm = new FigureForm() { Dock = DockStyle.Fill };
            _viewService = new PlotViewService(this, _figureForm);

            AttachEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!(DataContext is PlotViewModel vm)) return;

            _viewService.ResetPlot(vm.FigurePlot);
        }


        protected override void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(DataContext is PlotViewModel vm)) return;

            switch (e.PropertyName)
            {
                case nameof(PlotViewModel.FigurePlot):
                    this.SafeInvoke(() => _viewService.ResetPlot(vm.FigurePlot));
                    break;
                case nameof(PlotViewModel.IsLoading):
                    this.SafeInvoke(() =>
                    {
                        if (vm.IsLoading)
                            _viewService.ShowLoading();
                        else
                            _viewService.HideLoading();
                    });
                    break;
                case nameof(PlotViewModel.NeedsRefresh):
                    this.SafeInvoke(_viewService.RefreshPlot);
                    break;
            }
        }

        private void AttachEvents()
        {

        }
    }

    internal static class ControlExtensions
    {
        internal static void SafeInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
