using System.Collections.Generic;

namespace EdfBrowser.Model
{
    public class DataRecord
    {
        public DataRecord(uint sampleRate, uint index)
        {
            SampleRate = sampleRate;
            Index = index;
        }

        public uint SampleRate { get; }
        public uint Index { get; }
        public uint StartRecord { get; set; }
        public uint ReadCount { get; set; }
        public uint Length => SampleRate * ReadCount;
        public List<double> Buffer { get; } = new List<double>();

        public void Clear()
        {
            Buffer.Clear();
        }

        public void Add(double value)
        {
            Buffer.Add(value);
        }
    }
}

