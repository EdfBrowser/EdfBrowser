using System.ComponentModel;
using System.Windows.Forms;

namespace MVVMEssential
{
    public abstract class BaseView : UserControl
    {
        private readonly IContainer components;

        private BaseViewModel _dataContext;

        protected BaseView()
        {
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
        }

        public BaseViewModel DataContext
        {
            get { return _dataContext; }
            set
            {
                if (value != _dataContext)
                {
                    if (_dataContext != null)
                        _dataContext.PropertyChanged -= OnPropertyChanged;

                    _dataContext = value;
                    _dataContext.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing); // disposed control

            if (disposing)
            {
                components?.Dispose();

                if (_dataContext != null)
                    _dataContext.PropertyChanged -= OnPropertyChanged;
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e) { }
    }
}
