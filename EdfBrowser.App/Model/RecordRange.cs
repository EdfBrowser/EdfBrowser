namespace EdfBrowser.App
{
    internal class RecordRange
    {
        internal uint Start { get; set; }
        internal uint End { get; set; }
        internal bool Forward => Start < End;
    }
}
