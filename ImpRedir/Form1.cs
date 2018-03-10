using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportRedir.protection;
using System.Windows.Forms;
using AsmResolver.X86;
using AsmResolver;

namespace ImportRedir
{
    public partial class Form1 : Form
    {
        private Redirector redirector;

        public Form1()
        {
            InitializeComponent();
        }


        private void txtFile_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
                ChangeFile(files[0]);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                ChangeFile(openFileDialog1.FileName);
        }

        private void ChangeFile(string filename)
        {
            txtFile.Text = filename;
            gpMain.Enabled = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtFile.Text))
            {
                this.redirector = Redirector.Load(txtFile.Text);
                if (redirector != null)
                    gpMain.Enabled = true;
                else
                    MessageBox.Show(this, "Error reading PE file");
            }
            else
                MessageBox.Show(this, "File doesn't exist");
        }

        private void btnProtect_Click(object sender, EventArgs e)
        {
            ProtectionOptions options = new ProtectionOptions()
            {
                AddDllLoader = chkAddLoader.Checked
            };

            if (!redirector.Protect(options))
            {
                MessageBox.Show(this,redirector.Exception.Message);
            }
            else
                MessageBox.Show(this,"File protected");

            redirector = null;
            gpMain.Enabled = false;
        }
    }
}
