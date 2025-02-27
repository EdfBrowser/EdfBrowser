namespace EdfBrowser.EdfParser
{
    internal readonly struct SignalTransform
    {
        private readonly double _pMax;
        private readonly double _pMin;
        private readonly int _dMax;
        private readonly int _dMin;

        private readonly double _unit;
        private readonly double _offset;

        internal SignalTransform(double pMax, double pMin, int dMax, int dMin)
        {
            _pMax = pMax;
            _pMin = pMin;
            _dMax = dMax;
            _dMin = dMin;

            _unit = (_pMax - _pMin) / (_dMax - _dMin);
            _offset = (_pMax / _unit - _dMax);
        }

        internal SignalTransform(SignalInfo info) : this(
            info._physicalMax, info._physicalMin,
            info._digitalMax, info._digitalMin)
        { }

        internal double PMin => _pMin;
        internal double PMax => _pMax;
        internal int DMin => _dMin;
        internal int DMax => _dMax;
        internal double Unit => _unit;
        internal double Offset => _offset;
    }
}

