using EdfBrowser.App.Commands;
using EdfBrowser.App.Store;
using EdfBrowser.Models;
using System.Windows.Input;

namespace EdfBrowser.App.ViewModels
{
    public class EdfDashBoardViewModel
    {
        private readonly EdfStore m_edfStore;

        public EdfDashBoardViewModel(EdfStore edfStore)
        {
            m_edfStore = edfStore;
            ParseEdfCommand = new RelayCommand(ParseEdf);
        }

        public ICommand ParseEdfCommand { get; }

        public EdfInfo EdfInfo => m_edfStore.EdfInfo;

        private void ParseEdf(object parameter)
        {
            if (parameter is string path)
            {
                m_edfStore.ParseEdf(path);
            }
        }
    }
}
