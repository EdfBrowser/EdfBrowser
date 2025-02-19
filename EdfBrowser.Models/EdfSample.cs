namespace EdfBrowser.Models
{
    public class EdfSample
    {
        public EdfSample(int signal, double[] buf)
        {
            Signal = signal;
            Buf = buf;
        }

        public int Signal { get; }
        public double[] Buf { get; }
    }
}
