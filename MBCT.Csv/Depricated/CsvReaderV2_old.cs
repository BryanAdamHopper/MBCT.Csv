using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Text;

namespace MBCT.Csv.Depricated
{
    /// <summary>
    /// Read CSV-formatted data from a file or TextReader
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [Obsolete("Depricated. Please Use CsvReader().")]
    public class CSVReaderV2 : IDisposable
    {
        private readonly string _WrapperText = $"{(char)34}";
        private const string _Delimiter = ",";
        private readonly string[] _NewLine = { Environment.NewLine };
        private const string _ErrorMissingPath = "Null path passed to CSVReader";
        private const string _ErrorCorruptCsv = "CSV file is corrupt";

        /// <summary>
        /// Creates an instance of CSVReaderV2
        /// </summary>
        public CSVReaderV2()
        {
            //do nothing
        }

        private String RemoveWrapper(string text)
        {
            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
            //need to .substring() to avoid this.
            int start = 1; //second char
            int end = text.Length - 2;  //length-1 is the last index, so length-2 is second to last.
            return text.Substring(start, end);
        }
        private string SanitizeText(string text)
        {
            return text.Replace("\"\"", "\"");
        }
        private bool IsDataRowEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }

            foreach (var item in dr.ItemArray)
            {
                string text = item.ToString().Replace(Environment.NewLine, "").Replace("\r", "").Replace("\n", "").Trim();

                if (text != null && text != "")
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a csv file is corrupt.
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        public Boolean IsCorrupt(string csvText)
        {
            Boolean isCorrupt = false;

            string wrapperText = (char)34 + "";
            int wrapperCount = csvText.Length - csvText.Replace(wrapperText, "").Length;
            if (wrapperCount % 2 != 0)
            {
                isCorrupt = true;
            }

            return isCorrupt;
        }
        /// <summary>
        /// Creates a datatable from a csv text.
        /// </summary>
        /// <param name="strCsv"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <param name="displayRows"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(string strCsv, bool headerRow = true, int scanRows = 0, int displayRows = 0)
        {
            List<List<string>> data = new List<List<string>>();

            string[] reader = strCsv.Split(_NewLine, StringSplitOptions.None);
            for (int x = 0; x < reader.Length; x++)
            {
                //#########################################################################
                //# - Part 1 -
                //#     Build the complete row of data.
                //#########################################################################
                #region
                List<string> curRowData = new List<string>();

                string rowText = reader[x];
                int wrapperCount = rowText.Length - rowText.Replace(_WrapperText, "").Length;

                if (wrapperCount > 0)
                {
                    //if wrapperCount is odd, we are still wrapped. Grab the next line till we are no longer wrapped.
                    while (wrapperCount % 2 != 0)
                    {
                        x++; //update the outer loop.
                        if (x >= reader.Length)
                        {
                            break;
                        }
                        rowText += reader[x];
                        wrapperCount = rowText.Length - rowText.Replace(_WrapperText, "").Length;
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

                    if (currentChar == _WrapperText)
                    {
                        wrapped = wrapped == true ? false : true;
                    }


                    if (currentChar == _Delimiter && wrapped == false)
                    {
                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            textBuilder = RemoveWrapper(textBuilder);
                        }

                        curRowData.Add(SanitizeText(textBuilder));
                        textBuilder = "";
                    }
                    else if (y == rowText.Length - 1)
                    {
                        //last char of row
                        textBuilder += currentChar;

                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            textBuilder = RemoveWrapper(textBuilder);
                        }

                        curRowData.Add(SanitizeText(textBuilder));
                        textBuilder = "";
                    }
                    else
                    {
                        textBuilder += currentChar;
                    }

                }

                //add completed row as new item(row) in data<List<string>>
                data.Add(curRowData);
                #endregion

                //  if records is lessthan or equal to zero, get all records
                //  otherwise, stop after we get the desired record count.
                if (displayRows > 0)
                {
                    if (data.Count == displayRows)
                    {
                        break;
                    }
                }
            }

            //#########################################################################
            //# - Part 3 -
            //#     now we have all rows broken up into columns
            //#         List<List<string>> data
            //#         data[row][column]
            //#########################################################################
            #region
            //determine how many columns we are going to need to create
            int maxCols = 0;
            for (int x = 0; x < data.Count; x++)
            {
                int colCount = data[x].Count;
                maxCols = colCount > maxCols ? colCount : maxCols;
            }

            //populate datatable(dt)
            DataTable dt = new DataTable("csv");

            //create headers
            for (int x = 0; x < maxCols; x++)
            {
                if (headerRow == true)
                {
                    if (x < data[0].Count)
                    {
                        dt.Columns.Add($"{data[0][x]}", typeof(string));
                    }
                    else
                    {
                        dt.Columns.Add($"Column{x + 1}", typeof(string));
                    }
                }
                else
                {
                    dt.Columns.Add($"Column{x + 1}", typeof(string));
                }
            }

            //create rows
            int dataRowStart = headerRow == true ? 1 : 0;
            for (int x = dataRowStart; x < data.Count; x++)
            {
                DataRow dr = dt.NewRow();
                for (int y = 0; y < data[x].Count; y++)
                {
                    dr[y] = data[x][y];
                }

                if (!IsDataRowEmpty(dr))
                {
                    dt.Rows.Add(dr);
                }

            }
            #endregion

            return dt;
        }
        /// <summary>
        /// Creates a csv text from datatable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="wrapOnlyIfNeeded"></param>
        /// <param name="includeHeaders"></param>
        /// <returns></returns>
        public string CreateString(DataTable dt, bool wrapOnlyIfNeeded = true, bool includeHeaders = true)
        {
            if (!wrapOnlyIfNeeded)
            {
                return CreateString(dt, includeHeaders: includeHeaders);
            }

            string text = String.Empty;
            string[] wrapIf = { " ", "\"", "\'", ",", Environment.NewLine };


            for (int row = -1; row < dt.Rows.Count; row++)
            {
                if(row == -1 && !includeHeaders)
                {
                    continue;
                }

                string rowText = "";

                //each item in this row
                for (int rowCell = 0; rowCell < dt.Columns.Count; rowCell++)
                {
                    string item = "";

                    if (row == -1)
                    {
                        //column headers
                        item = dt.Columns[rowCell].ColumnName;
                    }
                    else
                    {
                        item = dt.Rows[row][rowCell].ToString();
                    }

                    bool needsWrapper = false;

                    //Check if needs wrapper
                    foreach (string condition in wrapIf)
                    {
                        if (item.Contains(condition))
                        {
                            needsWrapper = true;
                        }
                    }

                    if (rowCell > 0)
                    {
                        rowText += ",";
                    }

                    if (needsWrapper == true)
                    {
                        rowText += "\"" + item + "\"";
                    }
                    else
                    {
                        rowText += item;
                    }

                }

                text += rowText + Environment.NewLine;
            }

            return text;
        }

        private string CreateString(DataTable dt, bool includeHeaders)
        {
            char seperator = char.Parse(",");
            string wrapper = "\"";
            StringBuilder sb = new StringBuilder();

            //Headers
            if(includeHeaders)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var header = $"{wrapper}{dt.Columns[i]}{wrapper}";
                    sb.Append(header);
                    if (i < dt.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }

            //Body
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = $"{wrapper}{dr[i].ToString()}{wrapper}";
                    sb.Append(cell);

                    if (i < dt.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Call garbage collection and dispose of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose. Garbage Collect.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}