using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Diagnostics;
using System.IO;
using static MBCT.Helpers.TestTools.Files;
using static MBCT.Csv.Depricated.CsvTools;


namespace MBCT.Csv.Tests
{
    [TestClass]
    public class UnitTest_CsvTools : CsvTools
    {
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Basic01_HeaderUnwrapped()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Basic01_HeaderUnwrapped.txt"));
            DataTable dt = CSVReader(csvTest, true);
            Assert.AreEqual(3, dt.Columns.Count);
        }

        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Basic02_HeaderWrapped()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Basic02_HeaderWrapped.txt"));
            DataTable dt = CSVReader(csvTest, true);
            Assert.AreEqual(3, dt.Columns.Count);
        }

        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Basic03_DataUnWrapped()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Basic03_DataUnWrapped.txt"));
            DataTable dt = CSVReader(csvTest, true);
            Assert.AreEqual(3, dt.Columns.Count);
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Basic04_DataWrapped()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Basic04_DataWrapped.txt"));
            DataTable dt = CSVReader(csvTest, true);
            Assert.AreEqual(3, dt.Columns.Count);
        }

        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Advanced01_DataWrapped_CommaInWrapper()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Advanced01_DataWrapped_CommaInWrapper.txt"));

            Trace.WriteLine(csvTest);

            DataTable dt = CSVReader(csvTest, true);
            Assert.AreEqual(3, dt.Columns.Count);
        }

        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_Advanced02_HeaderWrapped_QuoteMissing()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Advanced02_HeaderWrapped_QuoteMissing.txt"));

            Trace.WriteLine(csvTest);

            Assert.AreEqual(true, IsCurrupt(csvTest));
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_IsCorrupt_true()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Advanced03_DataWrapped_QuoteMissing.txt"));
            Trace.WriteLine(csvTest);

            Assert.AreEqual(true, IsCurrupt(csvTest));
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_IsCorrupt_false()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Basic04_DataWrapped.txt"));
            Trace.WriteLine(csvTest);

            Assert.AreEqual(false, IsCurrupt(csvTest));
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_IsCorrupt_false_LotsOfStuff()
        {
            string csvTest = File.ReadAllText(GetTestDataPath("CSV_Advanced04_LotsOfStuff.txt"));
            Trace.WriteLine(csvTest);

            Assert.AreEqual(false, IsCurrupt(csvTest));
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_IsDataRowEmpty()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1", typeof(string));

            DataRow dr = dt.NewRow();
            dr[0] = "\r\n";


            Assert.AreEqual(true, IsDataRowEmpty(dr));
        }
        [TestMethod]
        [Owner("CsvTools")]
        [TestCategory("Unit")]
        public void CsvTools_IsDataRowEmpty_02()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1", typeof (string));

            DataRow dr = dt.NewRow();
            dr[0] = "WooHoo!!!";


            Assert.AreEqual(false, IsDataRowEmpty(dr));
        }



    }
}
