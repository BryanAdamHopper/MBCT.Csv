using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBCT.Csv.Contracts;
using System.Data;
using System.Windows.Forms;

namespace MBCT.Csv
{
    public partial class CsvReaderV2 : ICsvReaderV2, IDisposable
    {

        /// <summary>
        /// Converts tabbed string to csv string.
        /// </summary>
        /// <param name="dataGridView">The string containing the data.</param>
        /// <returns></returns>
        public string CreateString(DataGridView dataGridView)
        {
            var dt = ConvertSelectionToDataTable(dataGridView);
            var result = new CsvReaderV2().CreateString(dt);
            return result;
        }

        private DataTable ConvertSelectionToDataTable(DataGridView dataGridView)
        {
            var selectedCellCount = dataGridView.GetCellCount(DataGridViewElementStates.Selected);
            DataGridViewCell[] data2 = new DataGridViewCell[selectedCellCount];
            dataGridView.SelectedCells.CopyTo(data2, 0);

            List<int> Rows = new List<int>();
            List<int> Columns = new List<int>();
            foreach (var cell in data2)
            {
                var iRow = cell.RowIndex;
                var iCol = cell.ColumnIndex;

                if(!Rows.Contains(iRow))
                {
                    Rows.Add(iRow);
                }
                if(!Columns.Contains(iCol))
                {
                    Columns.Add(iCol);
                }
            }

            var dtSource = (DataTable)dataGridView.DataSource;
            var dt = ((DataTable)dataGridView.DataSource).Clone();

            //add rows to dt
            foreach (DataRow row in dtSource.Rows)
            {
                int rowIndex = dtSource.Rows.IndexOf(row);

                if (Rows.Contains(rowIndex))
                {
                    dt.Rows.Add(row.ItemArray);
                }
            }
            dt.AcceptChanges();

            //Remove un-selected columns
            foreach (DataColumn col in dtSource.Columns)
            {
                int colIndex = dtSource.Columns.IndexOf(col);
                var name = col.ColumnName;

                if (!Columns.Contains(colIndex))
                {
                    //dt.Columns.RemoveAt(colIndex);
                    dt.Columns.Remove(name);
                }
            }
            dt.AcceptChanges();

            return dt;
        }

    }
}
