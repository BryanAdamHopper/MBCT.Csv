using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace MBCT.Csv
{
    /// <summary>
    /// Used for one-off methods for working with CSV files.
    /// </summary>
    public partial class CsvTools
    {
        /// <summary>
        /// Csv file type.
        /// </summary>
        public static readonly string Type = "CSV";
        /// <summary>
        /// List of csv compatible file extensions.
        /// </summary>
        public static readonly List<string> Types = new List<string>
        {
            "csv",
            "txt",
        };
        /// <summary>
        /// Returns a string formated for compatible extensions.
        /// </summary>
        /// <returns></returns>
        public static List<string> OpenFileDialogueFilter()
        {
            var types = Types.Aggregate("", (current, type) => current + $"*.{type};");
            types += "|All Files|*.*";
            return new List<string> { $"{Type}", $"{Type} Files|{types}" };
        }
        /// <summary>
        /// Returns an OpenFielDialog window ready to open a csv compatible file.
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
        public static bool IsCsv(string path)
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
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable Reader(string filePath)
        {
            var csvText = File.ReadAllText(filePath);

            var dt = CreateDataTable(csvText);
            
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(string csvText)
        {
            var dt = new DataTable();

            dt = new CsvReaderV2().CreateDataTable(csvText);

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CreateString(DataTable dt)
        {
            var csvText = new CsvReaderV2().CreateString(dt);
            return csvText;
        }

    }
}
