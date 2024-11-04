using NUnit.Framework;
using Moq;
using System;
using Browser.EDF;

namespace EdfBrowser.Test
{
    [TestFixture]
    public class EdfTests
    {
        private const string ValidFilePath = @"C:\Users\admin\source\c#\psd_csharp\nunit.test\asserts\X.edf";

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
