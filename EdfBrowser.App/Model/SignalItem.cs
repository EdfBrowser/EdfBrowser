using EdfBrowser.EdfParser;

namespace EdfBrowser.App
{
    internal class SignalItem
    {
        private SignalInfo _signalInfo;

        internal SignalItem(SignalInfo signalInfo)
        {
            _signalInfo = signalInfo;
        }

        internal string Label => new string(_signalInfo._label);
        internal uint SampleRate => _signalInfo._samples;
    }
}
