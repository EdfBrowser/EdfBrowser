using System;
using System.Diagnostics;
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
            Debug.WriteLine("NET Framework");
#elif NET
            ApplicationConfiguration.Initialize();
            Debug.WriteLine("NET");
#endif

            Application.Run(new App());
        }
    }
}
