using EdfBrowser.EdfParser;
using System.Text;

namespace EdfBrowser.App
{
    internal class SignalItem
    {
        internal SignalItem(SignalInfo signalInfo)
        {
            SampleRate = signalInfo._samples;

            StringBuilder sb = new StringBuilder();
            foreach (char c in signalInfo._label)
            {
                if (c == '\0')
                    break;

                sb.Append(c);
            }

            Label = sb.ToString();
        }

        internal string Label { get; }
        internal uint SampleRate { get; }
    }
}
