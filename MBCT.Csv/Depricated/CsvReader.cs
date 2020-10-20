using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MBCT.Csv;
using MBCT.Csv.Models;

namespace MBCT.Csv.Depricated
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CSVReader_Depricated : IDisposable
    {
        private FileInfo _fileinfo;
        private string _filepath;
        private string _csvtext;
        private bool _iscorrupt = false;
        private string _wrappertext = $"{(char)34}";
        private string _delimeter = ",";
        private string[] _newline = { Environment.NewLine };

        /// <summary>
        /// 
        /// </summary>
        public CSVReader_Depricated()
        {
            //do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        public string WrapperText
        {
            get
            {
                return _wrappertext;
            }
            set
            {
                _wrappertext = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Delimeter
        {
            get
            {
                return _delimeter;
            }
            set
            {
                _delimeter = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string[] NewLine
        {
            get
            {
                return _newline;
            }
            set
            {
                _newline = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filepath;
            }
            set
            {
                _filepath = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public FileInfo FileInfo
        {
            get
            {
                return _fileinfo;
            }
            set
            {
                _fileinfo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                return _csvtext;
            }
            set
            {
                _csvtext = value;
            }
        }

        private void ClearAttributes()
        {
            _fileinfo = null;
            _filepath = null;
            _csvtext = null;
            _iscorrupt = false;
        }
        private void readToString(string path)
        {
            _csvtext = File.ReadAllText(path);
            _iscorrupt = IsCurrupt(_csvtext);

            if (_iscorrupt)
            {
                throw new ArgumentException("CSV file is corrupt");
            }
        }
        private Boolean IsCurrupt(string csvText)
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
        private String RemoveWrapper(string text)
        {
            //trimming the wrapperText will also remove leading or trailing wrapperText inside the wrapper
            //need to .substring() to avoid this.
            int start = 1; //second char
            int end = text.Length - 2;  //length-1 is the last index, so length-2 is second to last.
            return text.Substring(start, end);
        }
        private string SanitizeTextForTable(string text)
        {
            return text.Replace("\"\"", "\"");
        }
        private string SanitizeTextForCsv(string text)
        {
            return text.Replace("\"", "\"\"");
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
        /// 
        /// </summary>
        /// <param name="strCSV"></param>
        /// <param name="headerRow"></param>
        /// <param name="scanRows"></param>
        /// <param name="displayRows"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(string strCSV, bool headerRow = true, int scanRows = 0, int displayRows = 0)
        {
            List<List<string>> data = new List<List<string>>();
            int wrapperCount = 0;
            string rowText = "";

            //Can update these later into arrays for flexability
            //string wrapperText = String.Format("{0}", (char)34);   // (char)34 = double-quote(")
            //string delimeter = ",";   //char[] delimeter = { char.Parse(","), char.Parse("\t"), char.Parse(";") };
            //string[] newLine = { Environment.NewLine }; // string[] newLine = { Environment.NewLine, char.Parse("\r\n"), char.Parse("\n"), char.Parse("\r") }
            string wrapperText = _wrappertext;
            string delimeter = _delimeter;
            string[] newLine = _newline;

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
                        if (x >= reader.Length)
                        {
                            break;
                        }
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

                        curRowData.Add(SanitizeTextForTable(textBuilder));
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

                        curRowData.Add(SanitizeTextForTable(textBuilder));
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
        /// Creates a csv text from datatable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="wrapOnlyIfNeeded"></param>
        /// <param name="includeHeaders"></param>
        /// <returns></returns>
        public string CreateString(DataTable dt, bool wrapOnlyIfNeeded = true, bool includeHeaders = true)
        {
            var applyWrapper = "";
            if (wrapOnlyIfNeeded)
            {
                applyWrapper = CsvReaderV2Config.ApplyQualifier.OnlyWhenNeeded.ToString();
            }
            else
            {
                applyWrapper = CsvReaderV2Config.ApplyQualifier.Always.ToString();
            }

            return CreateString(dt, applyWrapper: applyWrapper, includeHeaders: includeHeaders);
        }

        /// <summary>
        /// Creates a csv text from datatable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="applyWrapper"></param>
        /// <param name="includeHeaders"></param>
        /// <param name="testOldSanitizer"></param>
        /// <returns></returns>
        public string CreateString(DataTable dt, string applyWrapper, bool includeHeaders = true, bool testOldSanitizer = false)
        {
            StringBuilder text = new StringBuilder();
            string[] wrapIf = { " ", "\"", "\'", ",", Environment.NewLine };

            for (int row = -1; row < dt.Rows.Count; row++)
            {
                if (row == -1 && !includeHeaders)
                {
                    continue;
                }

                StringBuilder rowText = new StringBuilder();
                
                //each item in this row
                for (int rowCell = 0; rowCell < dt.Columns.Count; rowCell++)
                {
                    string item = String.Empty; //using plain string for String.Contains().

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

                    if(applyWrapper == CsvReaderV2Config.ApplyQualifier.Never.ToString())
                    {
                        needsWrapper = false;
                    }
                    else if (applyWrapper == CsvReaderV2Config.ApplyQualifier.OnlyWhenNeeded.ToString())
                    {
                        //Check if needs wrapper
                        foreach (string condition in wrapIf)
                        {
                            if (item.Contains(condition))
                            {
                                needsWrapper = true;
                            }
                        }
                    }
                    else
                    {
                        needsWrapper = true;
                    }


                    if (rowCell > 0)
                    {
                        rowText.Append(",");
                    }

                    //set to false to test writing files the old wrong way
                    if(!testOldSanitizer)
                    {
                        item = SanitizeTextForCsv(item);
                    }
                    
                    if (needsWrapper == true)
                    {
                        rowText.Append("\"" + item + "\"");
                    }
                    else
                    {
                        rowText.Append(item);
                    }

                }

                text.Append(rowText);
                text.AppendLine();
            }

            return text.ToString();
        }
        
        private string CreateString2(DataTable dt, bool includeHeaders = true)
        {
            char seperator = char.Parse(",");
            string wrapper = "\"";
            StringBuilder sb = new StringBuilder();

            //Header
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
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                ClearAttributes();
            }
            // free native resources if there are any.
        }

        /// <summary>
        /// SOAP notes having wrapper without having internal double-qotes escaped.
        /// </summary>
        /// <param name="strCSV"></param>
        public string FixSoapNotes(string strCSV)
        {
            string[] newLine = _newline;
            string[] reader = strCSV.Split(newLine, StringSplitOptions.None);


            var dq = $"{(char)34}";
            var lcb = "{";
            var rcb = "}";
            var soapStart = $"{dq}{lcb}{dq}SerializedContentId{dq}:"; // "\"{ \"SerializedContentId\":";  // "{"SerializedContentId":
            var soapEnd = $"{dq}{rcb}{dq}"; // "\"}\""; // "}"
            var soapNotes = String.Empty;

            foreach (var line in reader)
            {
                if(soapNotes == String.Empty)
                {
                    soapNotes += $"{line}{Environment.NewLine}";
                    continue;
                }

                var i1 = line.IndexOf(soapStart, 0); //143

                if(i1 == -1)
                {
                    continue;
                }

                var i2 = line.IndexOf(soapEnd, i1); //415
                var length = (i2 + soapEnd.Length) - i1;

                var soapNote = line.Substring(i1, length);
                
                if(soapNote.Substring(0,1) == "\"" && soapNote.Substring(soapNote.Length-1, 1) == "\"")
                {
                    soapNote = soapNote.Substring(1, soapNote.Length - 2);
                }

                var soapNoteNew = soapNote.Replace("\"", "\"\"");
                //soapNoteNew = $"{dq}{soapNoteNew}{dq}";

                var lineNew = line.Replace(soapNote, soapNoteNew);

                soapNotes += $"{lineNew}{Environment.NewLine}";

                // will need to grab just the soap note
                // remove external wrapper
                // perform replace (escape internal wrappers)
                // add the external wrapper back in
                // then put soap note back in 

                //var breakHere = true;

            }

            return soapNotes;
        }

        /// <summary>
        /// Get SOAP notes from a DataTable. Returns a DataTable with just the SOAP notes.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetSoapNotes(DataTable dt)
        {
            var dtSoap = new DataTable();
            dtSoap.Columns.Add("ProgressNoteId", typeof(string));
            dtSoap.Columns.Add("SerializedContentId", typeof(string));
            dtSoap.Columns.Add("Subjective", typeof(string));
            dtSoap.Columns.Add("Objective", typeof(string));
            dtSoap.Columns.Add("Assessment", typeof(string));
            dtSoap.Columns.Add("Plan", typeof(string));

            foreach(DataRow row in dt.Rows)
            {
                var id = row["ProgressNoteId"].ToString();
                var note = row["EncryptedNote"].ToString();

                //var jsonObj = JsonConvert.DeserializeObject(note, type: SoapNote);
                var jsonObj = JsonConvert.DeserializeObject<SoapNote>(note);

                //var breakHere = true;

                var dr = dtSoap.NewRow();
                dr["ProgressNoteId"] = id;
                dr["SerializedContentId"] = jsonObj.SerializedContentId;
                dr["Subjective"] = jsonObj.Subjective;
                dr["Objective"] = jsonObj.Objective;
                dr["Assessment"] = jsonObj.Assessment;
                dr["Plan"] = jsonObj.Plan;

                dtSoap.Rows.Add(dr);
                dtSoap.AcceptChanges();
            }

            dtSoap.AcceptChanges();

            return dtSoap;
        }

    }
}
