using Plot.WinForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class PlotView : UserControl
    {
        private readonly PlotViewModel _viewModel;
        private readonly PlotViewService _viewService;

        private readonly FigureForm _figureForm;
        private readonly Dictionary<string, Action> _actions;

        private readonly Timer _animationTimer;
        private float _rotationAngle;
        private readonly Panel _loadingPanel;

        internal PlotView(PlotViewModel plotViewModel)
        {
            InitializeComponent();

            _viewModel = plotViewModel;
            // TODO: disposed old figureForm
            _figureForm = new FigureForm() { Dock = DockStyle.Fill };
            _viewService = new PlotViewService(this, _figureForm);

            InitializeBindings();
            AttachEvents();

            _viewService.ResetPlot(_viewModel.FigurePlot);
        }


        private void InitializeBindings()
        {
            _viewModel.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(PlotViewModel.FigurePlot):
                        this.SafeInvoke(() => _viewService.ResetPlot(_viewModel.FigurePlot));
                        break;
                    case nameof(PlotViewModel.IsLoading):
                        this.SafeInvoke(() =>
                        {
                            if (_viewModel.IsLoading)
                                _viewService.ShowLoading();
                            else
                                _viewService.HideLoading();
                        });
                        break;
                    case nameof(PlotViewModel.NeedsRefresh):
                        this.SafeInvoke(_viewService.RefreshPlot);
                        break;
                }
            };
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
