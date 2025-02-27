namespace EdfBrowser.EdfParser
{
    public class Sample
    {
        public Sample(uint sampleRate,
            uint index = 0, uint startRecord = 0, uint readCount = 1)
        {
            Index = index;
            StartRecord = startRecord;
            ReadCount = readCount;
            Length = sampleRate;
            Buffer = new double[sampleRate];
        }

        public uint Index { get; set; }
        public uint StartRecord { get; set; }
        public uint ReadCount { get; set; }
        public uint Length { get; set; }
        public double[] Buffer { get; set; }
    }
}

