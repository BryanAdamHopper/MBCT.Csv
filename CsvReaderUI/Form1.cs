using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBCT.Csv;
using MBCT.Prompt;
using System.IO;

namespace CsvReaderUI
{
    public partial class Form1 : Form
    {
        OpenFileDialog _ofd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblfilepath.Text = "";
        }

        /// <summary>
        /// ... Select File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            _ofd = CsvTools.OpenFileDiag();

            if(_ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            lblfilepath.Text = _ofd.FileName;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var dt = LoadFile(_ofd.FileName);

            PromptTools.DataGrid(dt);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if(fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var savepath = Path.Combine(fbd.SelectedPath, _ofd.SafeFileName);
            var dt = LoadFile(_ofd.FileName);
            var text = String.Empty;



            var test = checkBoxTestOldSanitizer.Checked;

            string applyWrapper = comboBoxWrapWhen.Text;

            
            var config = new CsvReaderV2Config() { QualifierWhen = new CsvReaderV2Config().ConvertQualifier(applyWrapper) };
            using (var csv = new CsvReaderV2(config))
            {
                text = csv.CreateString(dt);
            }

            File.WriteAllText(savepath, text);

            MessageBox.Show("File saved!");
        }

        private DataTable LoadFile(string path)
        {
            var text = File.ReadAllText(path);
            var dt = new DataTable();

            using (var csv = new CsvReaderV2())
            {
                dt = csv.CreateDataTable(text);
            }

            return dt;
        }



    }
}
