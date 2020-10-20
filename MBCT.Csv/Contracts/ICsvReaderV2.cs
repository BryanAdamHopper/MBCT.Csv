using System;
using System.Data;

namespace MBCT.Csv.Contracts
{
    /// <summary>
    /// Contract for CsvReader(V2)
    /// </summary>
    public interface ICsvReaderV2
    {
        /// <summary>
        /// Returns true if CSV string is corrupt. CSV is currupt if wrapper count is not even.
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        bool IsCurrupt(string csvText);

        /// <summary>
        /// Converts DataTable to csv string.
        /// </summary>
        /// <param name="dt">The DataTable containing the data.</param>
        /// <returns></returns>
        string CreateString(DataTable dt);

        /// <summary>
        /// Converts csv string to DataTable.
        /// </summary>
        /// <param name="strCsv"></param>
        /// <returns></returns>
        DataTable CreateDataTable(string strCsv);


        /// <summary>
        /// Converts csv string to DataTable.
        /// </summary>
        /// <param name="strCsv"></param>
        /// <param name="headerRow"></param>
        /// <param name="displayRows"></param>
        /// <returns></returns>
        [Obsolete("Depricated. Use CreateDataTable(string strCsv) instead.")]
        DataTable CreateDataTable(string strCsv, bool headerRow, int displayRows);

        /// <summary>
        /// Converts DataTable to csv string.
        /// </summary>
        /// <param name="dt">The DataTable containing the data.</param>
        /// <param name="wrapOnlyIfNeeded">Wraps all data. Does not check to see if it its not needed. Faster.</param>
        /// <param name="includeHeaders">Controlls if the output should contain the column headers.</param>
        /// <returns></returns>
        [Obsolete("Depricated. Use CreateString(DataTable dt) instead.")]
        string CreateString(DataTable dt, bool wrapOnlyIfNeeded, bool includeHeaders);
    }
}
