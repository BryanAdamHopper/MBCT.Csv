using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBCT.Csv.Depricated;

namespace MBCT.Csv.Tests
{
    [TestClass]
    public class UnitTest_CsvExtensions
    {
        private string _dq = $"{(char) 34}";
        private string _sq = $"{(char) 39}";

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvExtensions")]
        public void CsvExtensions_SanatizeAndWrap()
        {
            //Prep
            string text = $"Bryan {_dq}h0ppa{_sq}s, the{_dq} Hopper";

            //Action
            string actual = text.CsvSanitizeAndWrap();

            //Assert
            string expected = $"{_dq}Bryan {_dq}{_dq}h0ppa{_sq}{_sq}s, the{_dq}{_dq} Hopper{_dq}";
            Assert.AreEqual(expected: expected, actual: actual);
        }
    }
}
