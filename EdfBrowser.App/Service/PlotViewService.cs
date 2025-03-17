using EdfBrowser.CustomControl;
using Plot.Skia;
using Plot.WinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class PlotViewService
    {
        private readonly Control _hostControl;
        private readonly LoadingSpinner _spinner;
        private readonly FigureForm _figureForm;

        internal PlotViewService(Control hostControl, FigureForm figureForm)
        {
            _hostControl = hostControl;
            _figureForm = figureForm;
            _spinner = new LoadingSpinner()
            {
                Size = new Size(60, 60),
                Visible = false,
                Dock = DockStyle.Fill
            };

            _hostControl.Controls.Add(_spinner);
            _hostControl.Controls.Add(_figureForm);
            PositionSpinner();
        }

        private void PositionSpinner()
        {
            _spinner.Location = new Point(
                (_hostControl.Width - _spinner.Width) / 2,
                (_hostControl.Height - _spinner.Height) / 2
            );
        }

        internal void ShowLoading()
        {
            _spinner.BringToFront();
            _spinner.Visible = true;
        }

        internal void HideLoading()
        {
            _spinner.Visible = false;
            _figureForm.BringToFront();
        }

        internal void RefreshPlot()
        {
            _figureForm.Figure?.RenderManager.Fit();
            _figureForm.Refresh();
        }

        internal void ResetPlot(Figure figure)
        {
            _figureForm.Reset(figure, true);
            PositionSpinner();
        }
    }
}
