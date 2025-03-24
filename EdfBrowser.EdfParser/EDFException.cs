using System;

namespace EdfBrowser.EdfParser
{
    internal class EDFException : Exception
    {
        internal EDFException(string message) : base(message) { }
    }
}


