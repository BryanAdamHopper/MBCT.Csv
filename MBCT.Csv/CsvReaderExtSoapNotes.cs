using System;
using System.Data;
using MBCT.Csv.Models;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace MBCT.Csv
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CsvReaderV2
    {
        /// <summary>
        /// SOAP notes having wrapper without having internal double-qotes escaped.
        /// </summary>
        /// <param name="strCSV"></param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public string FixSoapNotes(string strCSV)
        {
            string[] newLine = new string[] { Environment.NewLine };
            string[] reader = strCSV.Split(newLine, StringSplitOptions.None);


            var dq = $"{(char)34}";
            var lcb = "{";
            var rcb = "}";
            var soapStart = $"{dq}{lcb}{dq}SerializedContentId{dq}:"; // "\"{ \"SerializedContentId\":";  // "{"SerializedContentId":
            var soapEnd = $"{dq}{rcb}{dq}"; // "\"}\""; // "}"
            var soapNotes = String.Empty;

            foreach (var line in reader)
            {
                if (soapNotes == String.Empty)
                {
                    soapNotes += $"{line}{Environment.NewLine}";
                    continue;
                }

                var i1 = line.IndexOf(soapStart, 0); //143

                if (i1 == -1)
                {
                    continue;
                }

                var i2 = line.IndexOf(soapEnd, i1); //415
                var length = (i2 + soapEnd.Length) - i1;

                var soapNote = line.Substring(i1, length);

                if (soapNote.Substring(0, 1) == "\"" && soapNote.Substring(soapNote.Length - 1, 1) == "\"")
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
        /// Returns a new DataTable containing just the SOAP notes. Expects at DataTable with a SOAP notes field called EncryptedNote. A ProgressNoteId field should be provided to allow for mapping back to origional data.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DataTable GetSoapNotes(DataTable dt)
        {
            var dtSoap = new DataTable();
            dtSoap.Columns.Add("ProgressNoteId", typeof(string));
            dtSoap.Columns.Add("SerializedContentId", typeof(string));
            dtSoap.Columns.Add("Subjective", typeof(string));
            dtSoap.Columns.Add("Objective", typeof(string));
            dtSoap.Columns.Add("Assessment", typeof(string));
            dtSoap.Columns.Add("Plan", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                var id = row["ProgressNoteId"].ToString();
                var note = row["EncryptedNote"].ToString();

                var jsonObj = JsonConvert.DeserializeObject<SoapNote>(note);

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
