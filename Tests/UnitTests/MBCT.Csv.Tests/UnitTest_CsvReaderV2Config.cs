using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections.Generic;

namespace MBCT.Csv.Tests
{
    [TestClass]
    public class UnitTest_CsvReaderV2Config
    {
        [Ignore]
        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void TestTemplate_CsvReaderV2Config_Method_Input_Returns()
        {
            //Prep
            //var config = new CsvReaderV2Config() { };

            //Action

            //Assert

        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConfigDelimiter_SemiColon_ReturnsDataTable()
        {
            //Prep
            var config = new CsvReaderV2Config() { Delimiter = CsvReaderV2Config.CsvDelimiter.SemiColon };

            //Action
            var dt = GetTestTable(config);

            //Assert
            var pass = DataIsGood(dt);
            Assert.IsTrue(pass);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConfigDelimiter_Tab_ReturnsDataTable()
        {
            //Prep
            var config = new CsvReaderV2Config() { Delimiter = CsvReaderV2Config.CsvDelimiter.Tab };

            //Action
            var dt = GetTestTable(config);

            //Assert
            var pass = DataIsGood(dt);
            Assert.IsTrue(pass);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConfigDelimiter_Space_ReturnsDataTable()
        {
            //Prep
            var config = new CsvReaderV2Config() { Delimiter = CsvReaderV2Config.CsvDelimiter.Space };

            //Action
            var dt = GetTestTable(config);

            //Assert
            var pass = DataIsGood(dt);
            Assert.IsTrue(pass);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConfigQualifier_SingleQuote_ReturnsDataTable()
        {
            //Prep
            var config = new CsvReaderV2Config() { Qualifier = CsvReaderV2Config.CsvQualifier.SingleQuote };

            //Action
            var dt = GetTestTable(config);

            //Assert
            var pass = DataIsGood(dt);
            Assert.IsTrue(pass);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConvertQualifier_StringOnlyWhenNeeded_ReturnsApplyQualifier()
        {
            //Prep
            var config = new CsvReaderV2Config() { };

            //Action
            var expected = CsvReaderV2Config.ApplyQualifier.OnlyWhenNeeded;
            var actual = config.ConvertQualifier("OnlyWhenNeeded");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConvertQualifier_StringNever_ReturnsApplyQualifier()
        {
            //Prep
            var config = new CsvReaderV2Config() { };

            //Action
            var expected = CsvReaderV2Config.ApplyQualifier.Never;
            var actual = config.ConvertQualifier("Never");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConvertQualifier_StringAlways_ReturnsApplyQualifier()
        {
            //Prep
            var config = new CsvReaderV2Config() { };

            //Action
            var expected = CsvReaderV2Config.ApplyQualifier.Always;
            var actual = config.ConvertQualifier("Always");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_ConvertQualifier_StringNotCovered_ReturnsDefaultApplyQualifier()
        {
            //Prep
            var config = new CsvReaderV2Config() { };

            //Action
            var expected = CsvReaderV2Config.ApplyQualifier.Always;
            var actual = config.ConvertQualifier("ThisIsNotAValidApplyQualifier");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2Config_SetQualifierWhen_StringQualifier_ReturnsApplyQualifier()
        {
            //Prep
            var config = new CsvReaderV2Config() { };

            //Action
            var expected = CsvReaderV2Config.ApplyQualifier.Always;

            config.SetQualifierWhen("Always");
            var actual = config.QualifierWhen;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        private bool DataIsGood(DataTable dt)
        {
            //table
            if(dt == null) { return false; }
            if(dt.Columns.Count != 3) { return false; }
            if(dt.Rows.Count != 3) { return false; }

            //columns
            if (dt.Columns[0].ToString() != "A") { return false; }
            if (dt.Columns[1].ToString() != "B") { return false; }
            if (dt.Columns[2].ToString() != "C") { return false; }
            //row 0
            if (dt.Rows[0][0].ToString() != "1") { return false; }
            if (dt.Rows[0][1].ToString() != "2") { return false; }
            if (dt.Rows[0][2].ToString() != "3") { return false; }
            //row 1
            if (dt.Rows[1][0].ToString() != "4") { return false; }
            if (dt.Rows[1][1].ToString() != "5") { return false; }
            if (dt.Rows[1][2].ToString() != "6") { return false; }
            //row2
            if (dt.Rows[2][0].ToString() != "7") { return false; }
            if (dt.Rows[2][1].ToString() != "8") { return false; }
            if (dt.Rows[2][2].ToString() != "9") { return false; }

            //All tests passed. Table data is good.
            return true;
        }

        private DataTable GetTestTable(CsvReaderV2Config config)
        {
            var text = GetCsvStringBaic(config);
            return new CsvReaderV2(config).CreateDataTable(text);
        }

        private string GetCsvStringBaic(CsvReaderV2Config config)
        {
            var q = config.GetQualifier();
            var d = config.GetDelimiter();

            var text = String.Empty;
            text += $"{q}A{q}{d}{q}B{q}{d}{q}C{q}" + Environment.NewLine;
            text += $"{q}1{q}{d}{q}2{q}{d}{q}3{q}" + Environment.NewLine;
            text += $"{q}4{q}{d}{q}5{q}{d}{q}6{q}" + Environment.NewLine;
            text += $"{q}7{q}{d}{q}8{q}{d}{q}9{q}" + Environment.NewLine;
            text += String.Empty;

            return text;
        }

    }
}
