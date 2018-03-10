namespace ImportRedir
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
            this.txtFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnLoad = new System.Windows.Forms.Button();
            this.gpMain = new System.Windows.Forms.GroupBox();
            this.btnProtect = new System.Windows.Forms.Button();
            this.chkAddLoader = new System.Windows.Forms.CheckBox();
            this.gpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Location = new System.Drawing.Point(48, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(200, 22);
            this.txtFile.TabIndex = 0;
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.txtFile.DragOver += new System.Windows.Forms.DragEventHandler(this.txtFile_DragOver);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "File";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(254, 11);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "exe file|*.exe|all files|*.*";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(21, 45);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // gpMain
            // 
            this.gpMain.Controls.Add(this.btnProtect);
            this.gpMain.Controls.Add(this.chkAddLoader);
            this.gpMain.Enabled = false;
            this.gpMain.Location = new System.Drawing.Point(15, 51);
            this.gpMain.Name = "gpMain";
            this.gpMain.Size = new System.Drawing.Size(270, 133);
            this.gpMain.TabIndex = 4;
            this.gpMain.TabStop = false;
            // 
            // btnProtect
            // 
            this.btnProtect.Location = new System.Drawing.Point(92, 104);
            this.btnProtect.Name = "btnProtect";
            this.btnProtect.Size = new System.Drawing.Size(75, 23);
            this.btnProtect.TabIndex = 5;
            this.btnProtect.Text = "Protect";
            this.btnProtect.UseVisualStyleBackColor = true;
            this.btnProtect.Click += new System.EventHandler(this.btnProtect_Click);
            // 
            // chkAddLoader
            // 
            this.chkAddLoader.AutoSize = true;
            this.chkAddLoader.Checked = true;
            this.chkAddLoader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddLoader.Location = new System.Drawing.Point(6, 23);
            this.chkAddLoader.Name = "chkAddLoader";
            this.chkAddLoader.Size = new System.Drawing.Size(158, 21);
            this.chkAddLoader.TabIndex = 0;
            this.chkAddLoader.Text = "Load imported DLLs";
            this.chkAddLoader.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 196);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.gpMain);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.gpMain.ResumeLayout(false);
            this.gpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.GroupBox gpMain;
        private System.Windows.Forms.CheckBox chkAddLoader;
        private System.Windows.Forms.Button btnProtect;
    }
}

