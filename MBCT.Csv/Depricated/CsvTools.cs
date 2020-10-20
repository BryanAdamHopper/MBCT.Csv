using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MBCT.Csv.Models;

namespace MBCT.Csv.Depricated
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class CsvTools
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string Type = "CSV";
        /// <summary>
        /// 
        /// </summary>
        public static readonly List<string> Types = new List<string>
        {
            "csv",
            "txt",
        };
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> OpenFileDialogueFilter()
        {
            var types = Types.Aggregate("", (current, type) => current + $"*.{type};");
            types += "|All Files|*.*";
            return new List<string> { $"{Type}", $"{Type} Files|{types}" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static OpenFileDialog OpenFileDiag()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<string> types = OpenFileDialogueFilter();

            ofd.Title = $"Open {types[0]} File";
            ofd.Filter = $"{types[1]}";
            ofd.FileName = null;

            return ofd;
        }

        /// <summary>
        /// Returns true if file extension matchs common file formats.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isCsv(string path)
        {
            string ext = Path.GetExtension(path);
            string filesTypes = "*.csv; *.txt;";

            if (filesTypes.IndexOf(ext) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Legacy CSV Reader by DerekH.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable Reader_Legacy(string path)
        {
            // load tab delimited file at path to datatable
            DataTable dt = new DataTable();
            string[] datatext = System.IO.File.ReadAllLines(path); //Load Tab delimited text file.     
            string[] text_data = null; //loop each line to string array
            int x = 0;
            foreach (string csvrow in datatext)
            {
                text_data = csvrow.Split(','); //Split data to array
                if (x == 0)  // if x = 0 first line header
                {
                    for (int i = 0; i <= text_data.Count() - 1; i++)
                    {
                        dt.Columns.Add(text_data[i]); //add columns
                    }
                }
                else  //create datarow for datatable
                {
                    DataRow dtr = dt.NewRow(); //if not first row data, not headers.
                    dtr.ItemArray = text_data; //array to datarow
                    dt.Rows.Add(dtr); // add datarow to datatable
                }
                x++; //step
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvFileInfo"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable Reader(FileInfo csvFileInfo, bool headerRow = true, int scanRows = 0)
        {
            string path = csvFileInfo.ToString();
            return Reader(path, headerRow, scanRows);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable Reader(string path, bool headerRow = true, int scanRows = 0)
        {
            string csvString = File.ReadAllText(path);
            return StringReader(csvString, headerRow, scanRows);
        }
        /// <summary>
        /// Provide a csv string already pulled from a file.
        /// </summary>
        /// <param name="csvString"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable StringReader(string csvString, bool headerRow = true, int scanRows = 0)
        {
            if (IsCurrupt(csvString))
            {
                return null;
            }

            return CSVReader(csvString, headerRow, scanRows);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        public static Boolean IsCurrupt(string csvText)
        {
            Boolean isCorrupt = false;

            string wrapperText = (char)34+"";
            int wrapperCount = csvText.Length - csvText.Replace(wrapperText, "").Length;
            if (wrapperCount%2 != 0)
            {
                isCorrupt = true;
            }

            return isCorrupt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String RemoveWrapper(string text)
        {   
            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
            //need to .substring() to avoid this.
            int start = 1; //second char
            int end = text.Length - 2;  //length-1 is the last index, so length-2 is second to last.
            return text.Substring(start, end);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SanitizeText(string text)
        {
            return text.Replace("\"\"", "\"");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static bool IsDataRowEmpty(DataRow dr)
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
        /// Advanced CSV Reader. Keeps track of wrapped text.
        /// </summary>
        /// <param name="strCSV"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable CSVReader(string strCSV, bool headerRow = true, int scanRows = 0)
        {
            List<List<string>> data = new List<List<string>>();
            int wrapperCount = 0;
            string rowText = "";

            //Can update these later into arrays for flexability
            string wrapperText = String.Format("{0}", (char)34);   // (char)34 = double-quote(")
            string delimeter = ",";   //char[] delimeter = { char.Parse(","), char.Parse("\t"), char.Parse(";") };
            string[] newLine = { Environment.NewLine }; // string[] newLine = { Environment.NewLine, char.Parse("\r\n"), char.Parse("\n"), char.Parse("\r") }

            string[] reader = strCSV.Split(newLine, StringSplitOptions.None);
            for (int x = 0; x < reader.Length; x++)
            {
                //#########################################################################
                //# - Part 1 -
                //#     Build the complete row of data.
                //#########################################################################
                #region
                List<string> curRowData = new List<string>();

                rowText = reader[x].ToString();
                wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;

                if (wrapperCount > 0)
                {
                    //if wrapperCount is odd, we are still wrapped. Grab the next line till we are no longer wrapped.
                    while (wrapperCount % 2 != 0)
                    {
                        x++; //update the outer loop.
                        rowText += reader[x].ToString();
                        wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;
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

                    if (currentChar == wrapperText)
                    {
                        wrapped = wrapped == true ? false : true;
                    }


                    if (currentChar == delimeter.ToString() && wrapped == false)
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
                        dt.Columns.Add($"{data[0][x].ToString()}", typeof(string));
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
                    dr[y] = data[x][y].ToString();
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
        /// 
        /// </summary>
        /// <param name="strCSV"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable CSVReader_ShortTest(string strCSV, bool headerRow = true, int scanRows = 0)
        {

            headerRow = true; // forcing headerRows to true;

            DataTable dt = new DataTable("csv");
            bool wrapped = false;
            string wrapperText = String.Format("{0}", (char) 34);
            int wrapperCount = 0;
            string[] delimeter = {",", "\t", ";"};
            string newLine = "\r\n"; //reading row by row. If wrapperCount is odd, append next row to this row.

            int col = 0;
            //string[] reader = File.ReadAllLines(filename);

            string[] reader = strCSV.Split(new string[] {newLine}, StringSplitOptions.None);

            int dataIndexRowStart = 0;
            string rowText = "";
            //get column headers

            #region column headers

            if (headerRow == true)
            {
                dataIndexRowStart = 1;

                rowText = reader[0].ToString();
                string[] columns = rowText.Split(Char.Parse(delimeter[0]));

                foreach (string item in columns)
                {
                    string columnName = item.ToString();

                    if (columnName.StartsWith("\"") && columnName.EndsWith("\""))
                    {
                        int start = 1; //second char
                        int end = columnName.Length - 2; //length-1 is the last index, so length-2 is second to last.
                        columnName = columnName.Substring(start, end);
                    }

                    dt.Columns.Add(columnName, typeof (string));
                }

            }

            #endregion

            //Prompt.DataGrid(dt, "dt"); //return null;


            //get rows

            #region get rows

            for (int x = dataIndexRowStart; x < reader.Length; x++)
            {
                rowText = reader[x].ToString();
                wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;

                //if wrapperCount is odd, we are still wrapped. Grab the next line till we are no longer wrapped.
                while (wrapperCount%2 != 0)
                {
                    x++; //keep the outer loop up to date.
                    rowText += reader[x].ToString();
                    wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;
                }


                //Prompt.MessageTextBox(rowText, "rowText");


                //now we have the whole row of data!
                //lets split it into columns.
                string textBuilder = "";
                DataRow dr = dt.NewRow();
                col = 0;
                for (int y = 0; y < rowText.Length; y++)
                {
                    string currentChar = rowText.Substring(y, 1);

                    if (currentChar == wrapperText)
                    {
                        wrapped = wrapped == true ? false : true;
                    }

                    //if (delimeter.Contains(currentChar)) { }
                    //if(currentChar == delimeter[0] && wrapped == false)
                    if (delimeter.Contains(currentChar) && wrapped == false)
                    {
                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            //.Trim(char.Parse(wrapperText))    //trim wrapper     
                            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
                            //need to .substring() to avoid this.
                            int start = 1; //second char
                            int end = textBuilder.Length - 2;
                                //length-1 is the last index, so length-2 is second to last.
                            textBuilder = textBuilder.Substring(start, end);
                        }
                        textBuilder = textBuilder.Replace("\"\"", "\"");

                        dr[col] = textBuilder;
                        textBuilder = "";
                        col++;
                    }
                    else if (y == rowText.Length - 1)
                    {
                        //last char of row
                        textBuilder += currentChar;

                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            //.Trim(char.Parse(wrapperText))    //trim wrapper     
                            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
                            //need to .substring() to avoid this.
                            int start = 1; //second char
                            int end = textBuilder.Length - 2;
                                //length-1 is the last index, so length-2 is second to last.
                            textBuilder = textBuilder.Substring(start, end);
                        }
                        textBuilder = textBuilder.Replace("\"\"", "\"");

                        dr[col] = textBuilder;
                        textBuilder = "";
                        col++;
                    }
                    else
                    {
                        textBuilder += currentChar;
                    }
                }

                dt.Rows.Add(dr);


            }

            #endregion

            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCSV"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <returns></returns>
        public static DataTable CSVReader_Legacy(string strCSV, bool headerRow = true, int scanRows = 0)
        {
            DataTable dt = new DataTable("csv");
            bool wrapped = false;
            int wrapperCount = 0;
            int dataIndexRowStart = 0;
            int col = 0;
            string rowText = "";

            //Can update these later into arrays for flexability
            string wrapperText = String.Format("{0}", (char)34); 
            char[] delimeter = { char.Parse(",")};   //char[] delimeter = { char.Parse(","), char.Parse("\t"), char.Parse(";") };
            string[] newLine = { Environment.NewLine }; 
            
            //reading row by row. If wrapperCount is odd, append next row to this row.
            string[] reader = strCSV.Split(newLine, StringSplitOptions.RemoveEmptyEntries);

            

            //get column headers
            #region column headers

            if (headerRow == true)
            {
                dataIndexRowStart = 1;

                rowText = reader[0].ToString();
                string[] columns = rowText.Split(delimeter);

                foreach (string item in columns)
                {
                    string columnName = item.ToString();

                    if (columnName.StartsWith("\"") && columnName.EndsWith("\""))
                    {
                        int start = 1; //second char
                        int end = columnName.Length - 2; //length-1 is the last index, so length-2 is second to last.
                        columnName = columnName.Substring(start, end);
                    }

                    dt.Columns.Add(columnName, typeof(string));
                }

            }

            #endregion

            //Prompt.DataGrid(dt, "dt"); //return null;

            //get rows
            #region get rows

            for (int x = dataIndexRowStart; x < reader.Length; x++)
            {
                rowText = reader[x].ToString();
                wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;

                //if wrapperCount is odd, we are still wrapped. Grab the next line till we are no longer wrapped.
                while (wrapperCount % 2 != 0)
                {
                    x++; //update the outer loop.
                    rowText += reader[x].ToString();
                    wrapperCount = rowText.Length - rowText.Replace(wrapperText, "").Length;
                }


                //Prompt.MessageTextBox(rowText, "rowText");


                //now we have the whole row of data!
                //lets split it into columns.
                string textBuilder = "";
                DataRow dr = dt.NewRow();
                col = 0;
                for (int y = 0; y < rowText.Length; y++)
                {
                    string currentChar = rowText.Substring(y, 1); //char currentChar = char.Parse(rowText.Substring(y, 1));

                    if (currentChar == wrapperText)
                    {
                        wrapped = wrapped == true ? false : true;
                    }

                    //if (delimeter.Contains(currentChar)) { }
                    //if(currentChar == delimeter[0] && wrapped == false)
                    if (delimeter.Contains(char.Parse(currentChar)) && wrapped == false)    //if (delimeter.Contains(currentChar) && wrapped == false)
                    {
                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            //.Trim(char.Parse(wrapperText))    //trim wrapper     
                            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
                            //need to .substring() to avoid this.
                            int start = 1; //second char
                            int end = textBuilder.Length - 2;
                            //length-1 is the last index, so length-2 is second to last.
                            textBuilder = textBuilder.Substring(start, end);
                        }
                        textBuilder = textBuilder.Replace("\"\"", "\"");

                        dr[col] = textBuilder;
                        textBuilder = "";
                        col++;
                    }
                    else if (y == rowText.Length - 1)
                    {
                        //last char of row
                        textBuilder += currentChar;

                        if (textBuilder.StartsWith("\"") && textBuilder.EndsWith("\""))
                        {
                            //.Trim(char.Parse(wrapperText))    //trim wrapper     
                            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
                            //need to .substring() to avoid this.
                            int start = 1; //second char
                            int end = textBuilder.Length - 2;
                            //length-1 is the last index, so length-2 is second to last.
                            textBuilder = textBuilder.Substring(start, end);
                        }
                        textBuilder = textBuilder.Replace("\"\"", "\"");

                        dr[col] = textBuilder;
                        textBuilder = "";
                        col++;
                    }
                    else
                    {
                        textBuilder += currentChar;
                    }
                }

                dt.Rows.Add(dr);


            }

            #endregion

            return dt;
        }

        /// <summary>
        /// depricated to new method of same name.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete("Depricated to new method of same name.")]
        private static string CreateString_(DataTable dt)
        {
            string text = "";
            string[] wrapIf = { " ", "\"", "\'", ",", Environment.NewLine };


            for (int row = -1; row < dt.Rows.Count; row++)
            {
                string rowText = "";

                //each item in this row
                for (int rowCell = 0; rowCell < dt.Columns.Count; rowCell++)
                {
                    string item = "";

                    if (row == -1)
                    {
                        //column headers
                        item = dt.Columns[rowCell].ColumnName.ToString();
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

                    if (rowText.Length > 0)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CreateString(DataTable dt)
        {
            using (CsvReaderV2 csv = new CsvReaderV2())
            {
                return csv.CreateString(dt);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCSV"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <param name="displayRows"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(string strCSV, bool headerRow = true, int scanRows = 0, int displayRows = 0)
        {
            var config = new CsvReaderV2Config() { IncludeHeaders = true, DisplayRows = displayRows };

            using (CsvReaderV2 csv = new CsvReaderV2(config))
            {
                return csv.CreateDataTable(strCSV);
            }
        }
    }
}
