using Browser.EDF;
using NUnit.Framework;

namespace EdfBrowser.Test
{
    [TestFixture]
    public class EdfTests
    {
        private const string ValidFilePath = @"D:\code\c#\psd_csharp\nunit.test\asserts\X.edf";

        [Test]
        public void Open_ValidFile_ReturnsTrue()
        {
            using (var edf = new Edf(ValidFilePath))
            {
                Assert.IsTrue(edf.Open());
            }
        }
    }
}
