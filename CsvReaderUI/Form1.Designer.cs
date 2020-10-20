namespace CsvReaderUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.lblfilepath = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBoxAlwaysWrap = new System.Windows.Forms.CheckBox();
            this.checkBoxTestOldSanitizer = new System.Windows.Forms.CheckBox();
            this.checkBoxNeverWrap = new System.Windows.Forms.CheckBox();
            this.comboBoxWrapWhen = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblfilepath
            // 
            this.lblfilepath.AutoSize = true;
            this.lblfilepath.Location = new System.Drawing.Point(31, 7);
            this.lblfilepath.Name = "lblfilepath";
            this.lblfilepath.Size = new System.Drawing.Size(53, 13);
            this.lblfilepath.TabIndex = 1;
            this.lblfilepath.Text = "<filepath>";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(193, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBoxAlwaysWrap
            // 
            this.checkBoxAlwaysWrap.AutoSize = true;
            this.checkBoxAlwaysWrap.Location = new System.Drawing.Point(304, 67);
            this.checkBoxAlwaysWrap.Name = "checkBoxAlwaysWrap";
            this.checkBoxAlwaysWrap.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAlwaysWrap.TabIndex = 4;
            this.checkBoxAlwaysWrap.Text = "AlwaysWrap";
            this.checkBoxAlwaysWrap.UseVisualStyleBackColor = true;
            this.checkBoxAlwaysWrap.Visible = false;
            // 
            // checkBoxTestOldSanitizer
            // 
            this.checkBoxTestOldSanitizer.AutoSize = true;
            this.checkBoxTestOldSanitizer.Location = new System.Drawing.Point(482, 35);
            this.checkBoxTestOldSanitizer.Name = "checkBoxTestOldSanitizer";
            this.checkBoxTestOldSanitizer.Size = new System.Drawing.Size(202, 17);
            this.checkBoxTestOldSanitizer.TabIndex = 5;
            this.checkBoxTestOldSanitizer.Text = "Test Old way (dont sanitize wrappers)";
            this.checkBoxTestOldSanitizer.UseVisualStyleBackColor = true;
            this.checkBoxTestOldSanitizer.Visible = false;
            // 
            // checkBoxNeverWrap
            // 
            this.checkBoxNeverWrap.AutoSize = true;
            this.checkBoxNeverWrap.Location = new System.Drawing.Point(395, 67);
            this.checkBoxNeverWrap.Name = "checkBoxNeverWrap";
            this.checkBoxNeverWrap.Size = new System.Drawing.Size(81, 17);
            this.checkBoxNeverWrap.TabIndex = 6;
            this.checkBoxNeverWrap.Text = "NeverWrap";
            this.checkBoxNeverWrap.UseVisualStyleBackColor = true;
            this.checkBoxNeverWrap.Visible = false;
            // 
            // comboBoxWrapWhen
            // 
            this.comboBoxWrapWhen.FormattingEnabled = true;
            this.comboBoxWrapWhen.Items.AddRange(new object[] {
            "Always",
            "OnlyWhenNeeded",
            "Never"});
            this.comboBoxWrapWhen.Location = new System.Drawing.Point(317, 31);
            this.comboBoxWrapWhen.Name = "comboBoxWrapWhen";
            this.comboBoxWrapWhen.Size = new System.Drawing.Size(121, 21);
            this.comboBoxWrapWhen.TabIndex = 7;
            this.comboBoxWrapWhen.Text = "OnlyWhenNeeded";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(281, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Wrap";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 84);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxWrapWhen);
            this.Controls.Add(this.checkBoxNeverWrap);
            this.Controls.Add(this.checkBoxTestOldSanitizer);
            this.Controls.Add(this.checkBoxAlwaysWrap);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblfilepath);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CsvReaderUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblfilepath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBoxAlwaysWrap;
        private System.Windows.Forms.CheckBox checkBoxTestOldSanitizer;
        private System.Windows.Forms.CheckBox checkBoxNeverWrap;
        private System.Windows.Forms.ComboBox comboBoxWrapWhen;
        private System.Windows.Forms.Label label1;
    }
}

