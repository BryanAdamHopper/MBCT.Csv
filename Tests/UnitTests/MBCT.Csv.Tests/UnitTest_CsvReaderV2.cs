using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Windows.Forms;

namespace MBCT.Csv.Tests
{
    [TestClass]
    public partial class UnitTest_CsvReaderV2
    {
        [Ignore]
        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void TestTemplate_CsvReaderV2_Method_Input_Returns()
        {
            //Prep
            //var csvText = File.ReadAllText(GetTestDataPath("CSV_Basic01_HeaderUnwrapped.txt"));

            //Action

            //Assert

        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateDataTable_ConfigDefault_CsvString_ReturnsDataTableWithAllRows()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();

            var config = new CsvReaderV2Config() { };

            var dt = new DataTable();
            using (var csv = new CsvReaderV2(config))
            {
                //Action
                dt = csv.CreateDataTable(csvText);
            }

            //Assert
            Assert.IsTrue(dt.Rows.Count == 5, message: $"Rows({dt.Rows.Count}) Cols({dt.Columns.Count})");
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateDataTable_ConfigDisplayRows1_CsvString_ReturnsDataTableWith1Row()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var config = new CsvReaderV2Config() { IncludeHeaders = true, DisplayRows = 1 };
            var csv = new CsvReaderV2(config);

            //Action
            var dt = csv.CreateDataTable(csvText);

            //Assert
            Assert.IsTrue(dt.Rows.Count == 1);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateDataTable_ConfigDisplkayRows0NoHeaders_CsvString_ReturnsDataTableWith1Row()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var config = new CsvReaderV2Config() { IncludeHeaders = false, DisplayRows = 0 };
            var csv = new CsvReaderV2(config);

            //Action
            var dt = csv.CreateDataTable(csvText);

            //Assert
            Assert.IsTrue(dt.Rows.Count == 1);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateString_DataTable_ReturnsCsvString()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var config = new CsvReaderV2Config() { IgnoreFalsePositiveEmptyRows = true };
            var dt = new CsvReaderV2(config).CreateDataTable(csvText);

            //Action
            var text = new CsvReaderV2(config).CreateString(dt);
            var expected = GetCsvStringAdvanced_Wrapped();

            //Assert
            Assert.AreEqual(expected, text);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateString_ConfigNewFlagFalse_DataTable_ReturnsCsvString()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var config = new CsvReaderV2Config() { IgnoreFalsePositiveEmptyRows = false };
            var dt = new CsvReaderV2(config).CreateDataTable(csvText);

            //Action
            var text = new CsvReaderV2(config).CreateString(dt);
            var expected = "";
            //expected = File.ReadAllText(GetTestDataPath("Result_Advanced04_LotsOfStuff_ConfigNewFlagFalse.txt"));
            expected = GetExpected_Advanced_ConfigNewFlagFalse();

            //Assert
            Assert.AreEqual(expected: expected, actual: text);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateString_ConfigHeadersFalse_DataTable_ReturnsCsvString()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var config = new CsvReaderV2Config() { IncludeHeaders = false };
            var dt = new CsvReaderV2(config).CreateDataTable(csvText);

            //Action
            var text = new CsvReaderV2(config).CreateString(dt);
            var expected = GetExpected_Advanced_ConfigHeadersFalse();

            //Assert
            Assert.AreEqual(expected, text);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsCorrupt_CsvString_ReturnsFalse()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();

            //Action
            var condition = new CsvReaderV2().IsCurrupt(csvText);

            //Assert
            Assert.IsFalse(condition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsCorrupt_CsvString_ReturnsTrue()
        {
            //Prep
            var csvText = GetCsvString_Wrapped_QuoteMissing();

            //Action
            var condition = new CsvReaderV2().IsCurrupt(csvText);

            //Assert
            Assert.IsTrue(condition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IfCorruptThrowError_CsvString_ThrowError()
        {
            //Prep
            var csvText = GetCsvString_Wrapped_QuoteMissing();

            var expected = CsvReaderV2Config.ErrorCorruptCsv;

            //Action
            var actual = String.Empty;
            try
            {
                new CsvReaderV2().IfCurruptThrowError(csvText);
            }
            catch(Exception ex)
            {
                actual = ex.Message;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IfCorruptThrowError_CsvString_NoError()
        {
            //Prep
            var csvText = GetCsvStringAdvanced();
            var expected = String.Empty;

            //Action
            var actual = String.Empty;
            try
            {
                new CsvReaderV2().IfCurruptThrowError(csvText);

            }
            catch (Exception ex)
            {
                actual = ex.Message;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_GetOccuranceCount_CsvString_ReturnsCount()
        {
            //Prep
            var csvText = "Dan \"The Man\" Jones";
            var expected = 2;

            //Action
            var count = new CsvReaderV2().GetOccuranceCount(csvText, "\"");

            //Assert
            Assert.AreEqual(expected, count);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_RemoveWrapper_WrappedString_ReturnsUnwrapped()
        {
            //Prep
            var text = "\"Dan \"The Man\" Jones\"";
            var expected = "Dan \"The Man\" Jones";

            //Action
            var actual = new CsvReaderV2().RemoveWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_RemoveWrapper_UnWrappedString_ReturnsUnwrapped()
        {
            //Prep
            var text = "Dan \"The Man\" Jones";
            var expected = "Dan \"The Man\" Jones";

            //Action
            var actual = new CsvReaderV2().RemoveWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_RemoveWrapper_FrontWrappedString_ReturnsUnchanged()
        {
            //Prep
            var text = "\"Dan \"The Man\" Jones";
            var expected = "\"Dan \"The Man\" Jones";

            //Action
            var actual = new CsvReaderV2().RemoveWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_RemoveWrapper_BackWrappedString_ReturnsUnchanged()
        {
            //Prep
            var text = "Dan \"The Man\" Jones\"";
            var expected = "Dan \"The Man\" Jones\"";

            //Action
            var actual = new CsvReaderV2().RemoveWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_RemoveWrapper_NullString_ReturnsUnchanged()
        {
            //Prep
            string text = null;
            string expected = null;

            //Action
            var actual = new CsvReaderV2().RemoveWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_ApplyWrapper_UnwrappedString_ReturnsWrapped()
        {
            //Prep
            var expected = "\"Dan \"The Man\" Jones\"";
            var text = "Dan \"The Man\" Jones";

            //Action
            var actual = new CsvReaderV2().ApplyWrapper(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_SanitizeTextForDatatable_CsvTextEscapedChars_ReturnsUnEscaped()
        {
            //Prep
            var expected = "Dan \"The Man\" Jones";
            var text = "Dan \"\"The Man\"\" Jones";

            //Action
            var actual = new CsvReaderV2().SanitizeTextForDatatable(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_SanitizeTextForCsv_CsvTextUnEscapedChars_ReturnsEscaped()
        {
            //Prep
            var text = "Dan \"The Man\" Jones";
            var expected = "Dan \"\"The Man\"\" Jones";

            //Action
            var actual = new CsvReaderV2().SanitizeTextForCsv(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_ToDatatableRemoveWrapAndSanitize_CsvTextWrappedEscaped_ReturnsUnWrappedUnEscaped()
        {
            //Prep
            var expected = "Dan \"The Man\" Jones";
            var text = "\"Dan \"\"The Man\"\" Jones\"";

            //Action
            var actual = new CsvReaderV2().ToDatatableRemoveWrapAndSanitize(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_ToCsvSanitizeAndWrap_CsvTextUnWrappedUnEscaped_ReturnsWrappedEscaped()
        {
            //Prep
            var text = "Dan \"The Man\" Jones";
            var expected = "\"Dan \"\"The Man\"\" Jones\"";

            //Action
            var actual = new CsvReaderV2().ToCsvSanitizeAndWrap(text);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsDataRowEmpty_DataRow_ReturnsFalse()
        {
            //Prep
            var dt = new DataTable();
            dt.Columns.Add("Test", typeof(string));
            var dr = dt.NewRow();
            dr[0] = "Test";

            //Action
            var condition = new CsvReaderV2().IsDataRowEmpty(dr);

            //Assert
            Assert.IsFalse(condition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsDataRowEmpty_DataRow_ReturnsTrue()
        {
            //Prep
            var dt = new DataTable();
            dt.Columns.Add("Test", typeof(string));
            var dr = dt.NewRow();
            dr[0] = "";

            //Action
            var condition = new CsvReaderV2().IsDataRowEmpty(dr);

            //Assert
            Assert.IsTrue(condition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsDataRowEmpty_DataRowFalsePositive_ReturnsTrue()
        {
            //Prep
            var dt = new DataTable();
            dt.Columns.Add("Test", typeof(string));
            var dr = dt.NewRow();
            dr[0] = "\" \"";

            //Action
            var condition = new CsvReaderV2().IsDataRowEmpty(dr);

            //Assert
            Assert.IsTrue(condition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_IsDataRowEmpty_DataRowNull_ReturnsTrue()
        {
            //Prep
            DataRow dr = null;

            //Action
            var condition = new CsvReaderV2().IsDataRowEmpty(dr);

            //Assert
            Assert.IsTrue(condition);
        }




        //[Ignore]
        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateDataList_Data_Returns()
        {
            //Prep
            var text = GetCsvStringAdvanced_LastRecordMultiline();

            //Action
            var condition = new CsvReaderV2().CreateDataTable(text);

            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [Owner("CsvReaderV2")]
        public void CsvReaderV2_CreateDataTable_DataWithDupeColumnName_ReturnsValidData()
        {
            //Prep
            var text = GetCsvStringDuplicateColumunName();

            //Action
            var dt = new DataTable();

            bool pass = false;
            try
            {
                dt = new CsvReaderV2().CreateDataTable(text);

                if(dt.Columns.Contains("Address_3"))
                {
                    pass = true;
                }
                
            }
            catch
            {
                //do nothing
            }

            //Assert
            Assert.IsTrue(pass);
        }

        private string GetCsvStringDuplicateColumunName()
        {
            string text = String.Empty;

            text += "Name,Address,Address,Zip,State";
            text += "Dan,Main,Apt3,12345,CA";

            return text;
        }

        private string GetCsvStringAdvanced()
        {
            string text = String.Empty;
            text += "ExcelLine,DataTableLine,Name,Number,Letters,Address,Notes,test,quoteTest" + Environment.NewLine;
            text += "2,1,\"\"\"true\"\" that\",123,abc,\"456 Main Street, Unit A\",\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",test,\"\"\"コンピュータ\"\"\"" + Environment.NewLine;
            text += "" + Environment.NewLine;
            text += "\"\"\" \"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"\" \"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"\"\"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\" \"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"" + Environment.NewLine;
            text += "3,2,\"\"\"false\"\" that\",124,def,457 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",コンピュータ," + Environment.NewLine;
            text += "4,3,\"\"\"true\"\" that\",125,hij,458 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",," + Environment.NewLine;
            text += "5,4,false_that,126,klm,459 Main Street Unit A,Some notes,test,stuff" + Environment.NewLine;
            text += "6,5,\"\"\"true\"\" that\",127,nop,460 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",\"\",\"\\\"\"\"" + Environment.NewLine;
            text += "";
            return text;
        }

        private string GetCsvStringAdvanced_LastRecordMultiline()
        {
            string text = String.Empty;
            text += "ExcelLine,DataTableLine,Name,Number,Letters,Address,Notes,test,quoteTest" + Environment.NewLine;
            text += "2,1,\"\"\"true\"\" that\",123,abc,\"456 Main Street, Unit A\",\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",test,\"\"\"コンピュータ\"\"\"" + Environment.NewLine;
            text += "" + Environment.NewLine;
            text += "\"\"\" \"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"\" \"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"\"\"\"\"" + Environment.NewLine;
            text += ",,,,,,,,,\" \"" + Environment.NewLine;
            text += ",,,,,,,,,\"\"" + Environment.NewLine;
            text += "3,2,\"\"\"false\"\" that\",124,def,457 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",コンピュータ," + Environment.NewLine;
            text += "4,3,\"\"\"true\"\" that\",125,hij,458 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",," + Environment.NewLine;
            text += "5,4,false_that,126,klm,459 Main Street Unit A,Some notes,test,stuff" + Environment.NewLine;
            text += "6,5,\"\"\"true\"\" that\",127,nop,460 Main Street Unit A,\"Has a lot of \"\"gas" + Environment.NewLine;
            text += "\"\"" + Environment.NewLine;
            text += "Also \"\"loves\"\" tacos;\",\"\",\"\\\"\"\"";
            return text;
        }

        private string GetCsvStringAdvanced_Wrapped()
        {
            var text = String.Empty;
            text += "\"ExcelLine\",\"DataTableLine\",\"Name\",\"Number\",\"Letters\",\"Address\",\"Notes\",\"test\",\"quoteTest\",\"Column10\"" + Environment.NewLine;
            text += "\"2\",\"1\",\"\"\"true\"\" that\",\"123\",\"abc\",\"456 Main Street, Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"test\",\"\"\"コンピュータ\"\"\",\"\"" + Environment.NewLine;
            text += "\"3\",\"2\",\"\"\"false\"\" that\",\"124\",\"def\",\"457 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"コンピュータ\",\"\",\"\"" + Environment.NewLine;
            text += "\"4\",\"3\",\"\"\"true\"\" that\",\"125\",\"hij\",\"458 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\",\"\"" + Environment.NewLine;
            text += "\"5\",\"4\",\"false_that\",\"126\",\"klm\",\"459 Main Street Unit A\",\"Some notes\",\"test\",\"stuff\",\"\"" + Environment.NewLine;
            text += "\"6\",\"5\",\"\"\"true\"\" that\",\"127\",\"nop\",\"460 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\\\"\"\",\"\"" + Environment.NewLine;
            text += "";
            return text;
        }

        private string GetCsvString_Wrapped_QuoteMissing()
        {
            string text = String.Empty;
            text += "\"FirstName\",\"LastName\",\"Address\"";
            text += "\"Bryan,\"Hopper\",\"123 Main st.\"";
            text += "";
            return text;
        }

        private string GetExpected_WrapOnlyNeeded()
        {
            string text = String.Empty;
            text = "ExcelLine,DataTableLine,Name,Number,Letters,Address,Notes,test,quoteTest,Column10" + Environment.NewLine;
            text += "2,1,\"\"\"true\"\" that\",123,abc,\"456 Main Street, Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",test,\"\"\"コンピュータ\"\"\"," + Environment.NewLine;
            text += "3,2,\"\"\"false\"\" that\",124,def,\"457 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",コンピュータ,," + Environment.NewLine;
            text += "4,3,\"\"\"true\"\" that\",125,hij,\"458 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",,," + Environment.NewLine;
            text += "5,4,false_that,126,klm,\"459 Main Street Unit A\",\"Some notes\",test,stuff," + Environment.NewLine;
            text += "6,5,\"\"\"true\"\" that\",127,nop,\"460 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",,\"\\\"\"\"," + Environment.NewLine;
            text += "";
            return text;
        }

        private string GetExpected_Advanced_ConfigNewFlagFalse()
        {
            var text = String.Empty;
            text += "\"ExcelLine\",\"DataTableLine\",\"Name\",\"Number\",\"Letters\",\"Address\",\"Notes\",\"test\",\"quoteTest\",\"Column10\"" + Environment.NewLine;
            text += "\"2\",\"1\",\"\"\"true\"\" that\",\"123\",\"abc\",\"456 Main Street, Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"test\",\"\"\"コンピュータ\"\"\",\"\"" + Environment.NewLine;
            text += "\"\"\" \"\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"" + Environment.NewLine;
            text += "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"\" \"\"\"" + Environment.NewLine;
            text += "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"\"\"\"\"" + Environment.NewLine;
            text += "\"3\",\"2\",\"\"\"false\"\" that\",\"124\",\"def\",\"457 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"コンピュータ\",\"\",\"\"" + Environment.NewLine;
            text += "\"4\",\"3\",\"\"\"true\"\" that\",\"125\",\"hij\",\"458 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\",\"\"" + Environment.NewLine;
            text += "\"5\",\"4\",\"false_that\",\"126\",\"klm\",\"459 Main Street Unit A\",\"Some notes\",\"test\",\"stuff\",\"\"" + Environment.NewLine;
            text += "\"6\",\"5\",\"\"\"true\"\" that\",\"127\",\"nop\",\"460 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\\\"\"\",\"\"" + Environment.NewLine;
            text += "";
            return text;
        }

        private string GetExpected_Advanced_ConfigHeadersFalse()
        {
            var text = String.Empty;
            //text += "\"Column1\",\"Column2\",\"Column3\",\"Column4\",\"Column5\",\"Column6\",\"Column7\",\"Column8\",\"Column9\",\"Column10\"" + Environment.NewLine;
            text += "\"ExcelLine\",\"DataTableLine\",\"Name\",\"Number\",\"Letters\",\"Address\",\"Notes\",\"test\",\"quoteTest\",\"\"" + Environment.NewLine;
            text += "\"2\",\"1\",\"\"\"true\"\" that\",\"123\",\"abc\",\"456 Main Street, Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"test\",\"\"\"コンピュータ\"\"\",\"\"" + Environment.NewLine;
            text += "\"3\",\"2\",\"\"\"false\"\" that\",\"124\",\"def\",\"457 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"コンピュータ\",\"\",\"\"" + Environment.NewLine;
            text += "\"4\",\"3\",\"\"\"true\"\" that\",\"125\",\"hij\",\"458 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\",\"\"" + Environment.NewLine;
            text += "\"5\",\"4\",\"false_that\",\"126\",\"klm\",\"459 Main Street Unit A\",\"Some notes\",\"test\",\"stuff\",\"\"" + Environment.NewLine;
            text += "\"6\",\"5\",\"\"\"true\"\" that\",\"127\",\"nop\",\"460 Main Street Unit A\",\"Has a lot of \"\"gas\"\"Also \"\"loves\"\" tacos;\",\"\",\"\\\"\"\",\"\"" + Environment.NewLine;
            text += "";
            return text;
        }
    }
}
