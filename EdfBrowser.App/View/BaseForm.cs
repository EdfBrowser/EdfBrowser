using System.ComponentModel;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal abstract class BaseForm : Form
    {
        private readonly IContainer components;

        private BaseViewModel _dataContext;

        protected BaseForm()
        {
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
        }

        ~BaseForm()
        {
            Dispose(false);
        }

        internal BaseViewModel DataContext
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
            base.Dispose(disposing);

            if (disposing)
            {
                components?.Dispose();

                if (_dataContext != null)
                    _dataContext.PropertyChanged -= OnPropertyChanged;
                _dataContext.Dispose();
            }

            Free(disposing);
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        protected virtual void Free(bool disposing) { }
    }
}
