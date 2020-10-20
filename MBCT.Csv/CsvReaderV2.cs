/* CsvReaderV2 - 
 *
 * Home Page: https://mindbody.sharepoint.com/sites/DataServicesEngineering/SitePages/Data-Services-Engineering.aspx
 * 
 * CsvReader and CsvHelper was not enough to handle all the wierd csv file formats that DataServices (Conversions) get from clients.
 * 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MBCT.Csv.Contracts;

// namespace mb.Admin.Helpers
namespace MBCT.Csv
{
    /// <summary>
    /// Read CSV-formatted data from a file or TextReader
    /// </summary>
    public partial class CsvReaderV2 : ICsvReaderV2, IDisposable
    {
        private CsvReaderV2Config _config;
        private char _delimeter;
        /// <summary>
        /// Alias: WrapperText
        /// </summary>
        private char _qualifier;

        /// <summary>
        /// CSVReader class. Assigns default config settings.
        /// </summary>
        public CsvReaderV2()
        {
            _config = new CsvReaderV2Config();
            ConfigSettings();
        }

        /// <summary>
        /// CSVReader class. Uses provided config settings.
        /// </summary>
        /// <param name="config"></param>
        public CsvReaderV2(CsvReaderV2Config config)
        {
            _config = config;
            ConfigSettings();
        }

        private void ConfigSettings()
        {
            _delimeter = _config.GetDelimiter();
            _qualifier = _config.GetQualifier();
        }

        /// <summary>
        /// Returns true if CSV string is corrupt. CSV is currupt if wrapper count is not even.
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        public bool IsCurrupt(string csvText)
        {
            bool isCorrupt = false;

            int wrapperCount = GetOccuranceCount(csvText, _qualifier.ToString());
            if (wrapperCount % 2 != 0)
            {
                isCorrupt = true;
            }

            return isCorrupt;
        }
       internal void IfCurruptThrowError(string csvText)
        {
            bool isCorrupt = IsCurrupt(csvText);
            if (isCorrupt)
            {
                throw new ArgumentException(CsvReaderV2Config.ErrorCorruptCsv);
            }
        }
        internal int GetOccuranceCount(string text, string valueToFind)
        {
            var len1 = text.Length - text.Replace(valueToFind, "").Length;
            var len2 = valueToFind.Length;
            var count = len1 / len2;

            return count;
        }
        internal string RemoveWrapper(string text)
        {
            bool startIf = false;
            bool endIf = false;

            try
            {
                startIf = text.StartsWith($"{_qualifier}");
            }
            catch
            {
                startIf = false;
            }

            try
            {
                endIf = text.EndsWith($"{_qualifier}");
            }
            catch
            {
                endIf = false;
            }


            if (startIf && endIf)
            {
                //trimming (TrimStart and TrimEnd) the wrapperText will also remove leading or trailing wrapperText inside the wrapper
                //need to .substring() to avoid this.
                int start = 1; //second char
                int end = text.Length - 2;  //length-1 is the last index, so length-2 is second to last.

                var newtext = text.Substring(start, end);
                text = newtext;
            }

            return text;
        }
        internal string ApplyWrapper(string text)
        {
            var newtext = $"{_qualifier}{text}{_qualifier}";
            return newtext;
        }
        internal string SanitizeTextForDatatable(string text)
        {
            var newtext = text.Replace($"{_qualifier}{_qualifier}", $"{_qualifier}");
            return newtext;
        }
        internal string SanitizeTextForCsv(string text)
        {
            var newtext = text.Replace($"{_qualifier}", $"{_qualifier}{_qualifier}");
            return newtext;
        }
        internal string ToDatatableRemoveWrapAndSanitize(string text)
        {
            text = RemoveWrapper(text);
            text = SanitizeTextForDatatable(text);
            return text;
        }
        internal string ToCsvSanitizeAndWrap(string text)
        {    
            text = SanitizeTextForCsv(text);
            text = ApplyWrapper(text);
            return text;
        }
        internal bool IsDataRowEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }

            foreach (var item in dr.ItemArray)
            {
                var condition = false;

                if(_config.IgnoreFalsePositiveEmptyRows)
                {
                    condition = String.IsNullOrWhiteSpace(item.ToString().Replace($"{_qualifier}", ""));
                }
                else
                {
                    condition = String.IsNullOrWhiteSpace(item.ToString());
                }

                if (!condition)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts csv string to list of lists type of string."
        /// </summary>
        /// <param name="strCsv"></param>
        /// <returns></returns>     
        private List<List<string>> CreateDataList(string strCsv)
        {
            List<List<string>> data = new List<List<string>>();

            string[] reader = strCsv.Split(_config.NewLine, StringSplitOptions.None);

            for (int x = 0; x < reader.Length; x++)
            {
                //#########################################################################
                //# - Part 1 -
                //#     Build the complete row of data.
                //#########################################################################
#region
                List<string> curRowData = new List<string>();

                string rowText = reader[x];
                int wrapperCount = GetOccuranceCount(rowText, _qualifier.ToString());

                if (wrapperCount > 0)
                {
                    //if wrapperCount is odd, we are still wrapped. Grab the next line till we are no longer wrapped.
                    while (wrapperCount % 2 != 0)
                    {
                        x++; //update the outer loop.

                        //commenting this out. Have no data to test this path. This may not be a thing anymore? Due to other path changes.
                        //can come back to this if something breaks due to this being missing.
                        //if (x >= reader.Length)
                        //{
                        //    break;
                        //}

                        rowText += reader[x];
                        wrapperCount = GetOccuranceCount(rowText, _qualifier.ToString());
                    }
                }
#endregion
                //#########################################################################
                //# - Part 2 -
                //#     Now we have the whole row of data, lets split it into columns.
                //#########################################################################
#region
                bool wrapped = false;
                string textBuilder = "";


                for (int y = 0; y < rowText.Length; y++)
                {
                    string currentChar = rowText.Substring(y, 1);

                    if (currentChar == _qualifier.ToString())
                    {
                        wrapped = wrapped == true ? false : true;
                    }


                    if (currentChar == _delimeter.ToString() && !wrapped)
                    {
                        //textBuilder = SanitizeToDatatable(textBuilder);
                        textBuilder = ToDatatableRemoveWrapAndSanitize(textBuilder);
                        curRowData.Add(textBuilder);
                        textBuilder = String.Empty;
                    }
                    else if (y == rowText.Length - 1)
                    {
                        //last char of row
                        textBuilder += currentChar;

                        //textBuilder = SanitizeToDatatable(textBuilder);
                        textBuilder = ToDatatableRemoveWrapAndSanitize(textBuilder);
                        curRowData.Add(textBuilder);
                        textBuilder = String.Empty;
                    }
                    else
                    {
                        textBuilder += currentChar;
                    }

                }

                //add completed row as new item(row) in data<List<string>>
                data.Add(curRowData);
#endregion

                //  if records is lessthan or equal to -1, get all records. Zero returns just headers
                //  otherwise, stop after we get the desired record count.
                if (_config.DisplayRows > -1)
                {
                    //first pass has 1 record
                    //if(!_config.IncludeHeaders && _config.DisplayRows == 0)
                    var stopRow = _config.DisplayRows + 1;

                    //if (data.Count == _config.DisplayRows)
                    if (data.Count == stopRow)
                    {
                        break;
                    }
                }
            }

            return data;
        }
        /// <summary>
        /// Converts a List of Lists of varrying lengths to a Datatable.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataTable CreateDataTable(List<List<string>> data)
        {
            //determine how many columns we are going to need to create
            int maxCols = 0;
            for (int x = 0; x < data.Count; x++)
            {
                var colCount = data[x].Count;
                maxCols = colCount > maxCols ? colCount : maxCols;
            }

            var dt = new DataTable("csv");

            //create headers
            for (int x = 0; x < maxCols; x++)
            {
                if (_config.IncludeHeaders && x < data[0].Count)
                {
                    string colName = data[0][x];

                    if(dt.Columns.Contains(colName))
                    {
                        colName = $"{colName}_{x+1}";
                    }

                    dt.Columns.Add(colName, typeof(string));
                }
                else
                {
                    dt.Columns.Add($"Column{x + 1}", typeof(string));
                }
            }

            //create rows
            int dataRowStart = _config.IncludeHeaders == true ? 1 : 0;
            for (int x = dataRowStart; x < data.Count; x++)
            {
                var dr = dt.NewRow();
                for (int y = 0; y < data[x].Count; y++)
                {
                    dr[y] = data[x][y];
                }

                var emptyRow = IsDataRowEmpty(dr);
                if (!emptyRow)
                {
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }
        
        /// <summary>
        /// Converts csv string to DataTable.
        /// </summary>
        /// <param name="strCsv"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(string strCsv)
        {
            IfCurruptThrowError(strCsv);

            var data = CreateDataList(strCsv);

            var dt = CreateDataTable(data);

            return dt;
        }
        
        /// <summary>
        /// Converts DataTable to csv string.
        /// </summary>
        /// <param name="dt">The DataTable containing the data.</param>
        /// <returns></returns>
        public string CreateString(DataTable dt)
        {
            var text = new StringBuilder();

            for (int row = -1; row < dt.Rows.Count; row++)
            {
                if (row == -1 && !_config.IncludeHeaders)
                {
                    continue;
                }

                var rowText = new StringBuilder();

                //each item in this row
                for (int rowCell = 0; rowCell < dt.Columns.Count; rowCell++)
                {
#region Get Data
                    string item = String.Empty;

                    if (row == -1)
                    {
                        //column headers
                        item = dt.Columns[rowCell].ColumnName;
                    }
                    else
                    {
                        item = dt.Rows[row][rowCell].ToString();
                    }
#endregion

#region Wrapper Handler
                    //CsvReaderConfig.ApplyQualifier.Always ?? needsWrapper = true
                    //CsvReaderConfig.ApplyQualifier.Never ?? needsWrapper =  ?? needsWrapper = false
                    //CsvReaderConfig.ApplyQualifier.OnlyWhenNeeded ?? needsWrapper = maybe...

                    bool needsWrapper = false; //ApplyQualifier.Never
                    if (_config.QualifierWhen == CsvReaderV2Config.ApplyQualifier.OnlyWhenNeeded) //wrapOnlyIfNeeded
                    {
                        //Check if needs wrapper
                        foreach (string condition in _config.WrapIf)
                        {
                            if (item.Contains(condition))
                            {
                                needsWrapper = true; //ApplyQualifier.OnlyWhenNeeded
                            }
                        }
                    }
                    else if (_config.QualifierWhen == CsvReaderV2Config.ApplyQualifier.Always)
                    {
                        needsWrapper = true; //ApplyQualifier.Always
                    }
#endregion Wrapper Handler

#region Sanitize and Append Data
                    if (needsWrapper == true)
                    {
                        item = ToCsvSanitizeAndWrap(item);
                    }
                    else
                    {
                        item = SanitizeTextForCsv(item);
                    }


                    if (rowCell > 0)
                    {
                        rowText.Append(",");
                    }

                    rowText.Append(item);

#endregion Sanitize and Append Data
                }

                text.Append(rowText + Environment.NewLine);
            }

            return text.ToString();
        }
       
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //nothing to dispose anymore
            }
        }

        /// <summary>
        /// Converts csv string to DataTable.
        /// </summary>
        /// <param name="strCsv"></param>
        /// <param name="headerRow"></param>
        /// <param name="displayRows"></param>
        /// <returns></returns>
        [Obsolete("Depricated. Use CreateDataTable(string strCsv) instead.")]
        public DataTable CreateDataTable(string strCsv, bool headerRow, int displayRows)
        {
            _config.IncludeHeaders = headerRow;
            _config.DisplayRows = displayRows;

            var dt = CreateDataTable(strCsv);

            return dt;
        }

        /// <summary>
        /// Converts DataTable to csv string.
        /// </summary>
        /// <param name="dt">The DataTable containing the data.</param>
        /// <param name="wrapOnlyIfNeeded">Wraps all data. Does not check to see if it its not needed. Faster.</param>
        /// <param name="includeHeaders">Controlls if the output should contain the column headers.</param>
        /// <returns></returns>
        [Obsolete("Depricated. Use CreateString(DataTable dt) instead.")]
        public string CreateString(DataTable dt, bool wrapOnlyIfNeeded, bool includeHeaders)
        {
            if (wrapOnlyIfNeeded)
            {
                _config.QualifierWhen = CsvReaderV2Config.ApplyQualifier.OnlyWhenNeeded;
            }
            else
            {
                _config.QualifierWhen = CsvReaderV2Config.ApplyQualifier.Always;
            }
   
            _config.IncludeHeaders = includeHeaders;

            var csv = CreateString(dt);

            return csv;
        }
    }
}