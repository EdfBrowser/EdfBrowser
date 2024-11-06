using EdfBrowser.Models;
using EdfBrowser.Services;

namespace EdfBrowser.App.Store
{
    public class EdfStore
    {
        private readonly IEdfService m_edfService;

        public EdfStore(IEdfService edfService)
        {
            m_edfService = edfService;
        }

        public EdfInfo EdfInfo { get; private set; }

        public void ParseEdf(string path)
        {
            EdfInfo = m_edfService.ParseEdf(path);
        }
    }
}
