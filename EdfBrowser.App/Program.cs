using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if NETFRAMEWORK
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#elif NET
            ApplicationConfiguration.Initialize();
#endif
            Application.Run(new App());
        }
    }
}
