using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class MainWindow : BaseForm
    {
        private readonly ViewFactory _viewFactory;
        private readonly Panel _contentPanel;

        public MainWindow(ViewFactory viewFactory)
        {
            SuspendLayout();

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            ClientSize = new System.Drawing.Size(816, 457);
            Margin = new Padding(3, 2, 3, 2);
            Name = "App";
            Text = "Edf Browser";

            ResumeLayout(false);

            _viewFactory = viewFactory;

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(_contentPanel);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!(DataContext is MainViewModel vm)) return;

            SwitchControl(vm.CurrentViewModel);
        }

        protected override void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(DataContext is MainViewModel vm)) return;

            if (e.PropertyName == nameof(vm.CurrentViewModel))
                SwitchControl(vm.CurrentViewModel);
        }

        protected override void Free(bool disposing)
        {
            if (disposing)
            {
                _viewFactory.Dispose();
            }
        }

        private void SwitchControl(BaseViewModel vm)
        {
            _contentPanel.Controls.Clear();
            if (_contentPanel.Controls.Count > 0 && _contentPanel.Controls[0] is IDisposable disposable)
                disposable.Dispose();
            //_viewFactory.Dispose();

            BaseView view = _viewFactory.CreateView(vm);
            if (view == null) return;

            view.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(view);
        }
    }
}
