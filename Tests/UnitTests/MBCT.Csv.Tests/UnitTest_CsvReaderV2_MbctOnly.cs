using System;
using System.IO;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBCT.Csv;
using static MBCT.Helpers.TestTools.Files;

namespace MBCT.Csv.Tests
{
    public partial class UnitTest_CsvReaderV2
    {
        [Ignore]
        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void TestTemplate_CsvReaderV2MbctOnly_Method_Input_Returns()
        {
            //Prep
            var csvText = File.ReadAllText(GetTestDataPath("CSV_Basic01_HeaderUnwrapped.txt"));

            //Action

            //Assert

        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2MbctOnly_CreateDataTableLegacy_Input_Returns()
        {
            //Prep
            var csvText = File.ReadAllText(GetTestDataPath("CSV_Advanced04_LotsOfStuff.txt"));
            var config = new CsvReaderV2Config() { };

            var dt = new DataTable();
            using (var csv = new CsvReaderV2(config))
            {
                //Action
#pragma warning disable CS0618 // Obsolete Attribbute on method call
                dt = csv.CreateDataTable(csvText, headerRow: true, displayRows: -1);
#pragma warning restore CS0618 // Obsolete Attribbute on method call
            }

            //Assert
            Assert.IsTrue(dt.Rows.Count == 5, message: $"Rows({dt.Rows.Count}) Cols({dt.Columns.Count})");
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2MbctOnly_CreateStringLegacy_Input_Returns()
        {
            //Prep
            var csvText = File.ReadAllText(GetTestDataPath("CSV_Advanced04_LotsOfStuff.txt"));
            var config = new CsvReaderV2Config() { };
            var dt = new CsvReaderV2(config).CreateDataTable(csvText);

            //Action
#pragma warning disable CS0618 // Obsolete Attribbute on method call
            var text = new CsvReaderV2().CreateString(dt, wrapOnlyIfNeeded: false, includeHeaders: true);
#pragma warning restore CS0618 // Obsolete Attribbute on method call
            var expected = File.ReadAllText(GetTestDataPath("Result_Advanced04_LotsOfStuff.txt"));

            //Assert
            Assert.AreEqual(expected, text);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2MbctOnly_CreateStringLegacy_ConfigWrapOnlyNeeded_Input_Returns()
        {
            //Prep
            var csvText = String.Empty;
            csvText = File.ReadAllText(GetTestDataPath("CSV_Advanced04_LotsOfStuff.txt"));
            csvText = GetCsvStringAdvanced();

            var config = new CsvReaderV2Config() { };
            var dt = new CsvReaderV2(config).CreateDataTable(csvText);

            //Action
#pragma warning disable CS0618 // Obsolete Attribbute on method call
            var text = new CsvReaderV2().CreateString(dt, wrapOnlyIfNeeded: true, includeHeaders: true);
#pragma warning restore CS0618 // Obsolete Attribbute on method call
            var expected = String.Empty;
            expected = File.ReadAllText(GetTestDataPath("Result_Advanced04_LotsOfStuff.txt"));
            expected = GetExpected_WrapOnlyNeeded();

            //Assert
            Assert.AreEqual(expected, text);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2MbctOnly_FixSoapNotes_Input_Returns()
        {
            //Prep
            var csvText = String.Empty;
            //csvText = File.ReadAllText(GetTestDataPath("CSV_SOAP_Basic01.txt"));
            csvText = "\"{\"SerializedContentId\":\"bc49686c\",\"Subjective\":\"<div><br></div><div><br></div>\",\"Objective\":\"\",\"Assessment\":\"<div><br bogus=\"1\"></div>\",\"Plan\":\"<div><br bogus=\"1\"></div>\",\"Note\":\"\"}\"";
            csvText = Environment.NewLine + csvText + Environment.NewLine;

            var expected = String.Empty;
            //expected = File.ReadAllText(GetTestDataPath("Result_SOAP_Basic01.txt"));
            expected = "\"{\"\"SerializedContentId\"\":\"\"bc49686c\"\",\"\"Subjective\"\":\"\"<div><br></div><div><br></div>\"\",\"\"Objective\"\":\"\"\"\",\"\"Assessment\"\":\"\"<div><br bogus=\"\"1\"\"></div>\"\",\"\"Plan\"\":\"\"<div><br bogus=\"\"1\"\"></div>\"\",\"\"Note\"\":\"\"\"\"}\"";
            expected = Environment.NewLine + expected + Environment.NewLine;

            //Action
            var actual = new CsvReaderV2().FixSoapNotes(csvText);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2MbctOnly_GetSoapNotes_Input_Returns()
        {
            //Prep
            var dt = new DataTable();
            dt.Columns.Add("ProgressNoteId", typeof(string));
            dt.Columns.Add("EncryptedNote", typeof(string));
            var row = dt.NewRow();
            row[0] = "1";
            row[1] = "{\"SerializedContentId\": \"123\", \"Subjective\": \"peanut butter\", \"Objective\": \"jelly\", \"Assessment\": \"bread\", \"Plan\": \"eat\"}";
            dt.Rows.Add(row);

            //Action
            var results = new CsvReaderV2().GetSoapNotes(dt);
            var result = results.Rows[0];

            //Assert
            var SerializedContentId = result["SerializedContentId"].ToString() == "123";
            var Subjective = result["Subjective"].ToString() == "peanut butter";
            var Objective = result["Objective"].ToString() == "jelly";
            var Assessment = result["Assessment"].ToString() == "bread";
            var Plan = result["Plan"].ToString() == "eat";

            var condition = SerializedContentId && Subjective && Objective && Assessment && Plan;
            Assert.IsTrue(condition);
        }

    }
}
