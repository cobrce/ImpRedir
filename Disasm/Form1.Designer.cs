namespace Disasm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtRVA = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDisasm = new System.Windows.Forms.Button();
            this.txtDisasm = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "File";
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Location = new System.Drawing.Point(64, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(296, 22);
            this.txtFile.TabIndex = 1;
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.txtFile.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(366, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(43, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtRVA
            // 
            this.txtRVA.Location = new System.Drawing.Point(64, 40);
            this.txtRVA.Name = "txtRVA";
            this.txtRVA.Size = new System.Drawing.Size(72, 22);
            this.txtRVA.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Offset";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(204, 40);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(72, 22);
            this.txtSize.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Size";
            // 
            // btnDisasm
            // 
            this.btnDisasm.Location = new System.Drawing.Point(327, 39);
            this.btnDisasm.Name = "btnDisasm";
            this.btnDisasm.Size = new System.Drawing.Size(82, 23);
            this.btnDisasm.TabIndex = 2;
            this.btnDisasm.Text = "Disasm";
            this.btnDisasm.UseVisualStyleBackColor = true;
            this.btnDisasm.Click += new System.EventHandler(this.btnDisasm_Click);
            // 
            // txtDisasm
            // 
            this.txtDisasm.Location = new System.Drawing.Point(12, 94);
            this.txtDisasm.Multiline = true;
            this.txtDisasm.Name = "txtDisasm";
            this.txtDisasm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDisasm.Size = new System.Drawing.Size(397, 349);
            this.txtDisasm.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Output";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Title = "Select file to disassemble";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 455);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDisasm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRVA);
            this.Controls.Add(this.btnDisasm);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Disasm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtRVA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDisasm;
        private System.Windows.Forms.TextBox txtDisasm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

