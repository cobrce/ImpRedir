using AsmResolver;
using AsmResolver.X86;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disasm
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
                txtFile.Text = files[0];
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                txtFile.Text = openFileDialog1.FileName;
        }

        private void btnDisasm_Click(object sender, EventArgs e)
        {
            int offset;
            int size;

            if (!int.TryParse(txtRVA.Text, System.Globalization.NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out offset))
                MessageBox.Show(this, "Couldn't parse Offset");
            else
            {
                if (!int.TryParse(txtSize.Text, System.Globalization.NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out size))
                    MessageBox.Show(this, "Couldn't parse Size");
                else
                {
                    if (!File.Exists(txtFile.Text))
                        MessageBox.Show(this, "File doesn't exist");
                    else
                        txtDisasm.Text = Disasm(txtFile.Text, offset, size);
                }
            }
        }

        private string Disasm(string path, int offset, int size)
        {
            string lines = "";
            try
            {
                MemoryStreamReader reader = new MemoryStreamReader(File.ReadAllBytes(path));

                reader.Position = offset;
                List<X86Instruction> instrs = ReadInstructions(offset, size, reader);
                List<ulong> labels = GetLabels(instrs);
                lines = Format(instrs, labels);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return lines;
        }

        private string Format(List<X86Instruction> instrs, List<ulong> labels)
        {
            FasmX86Formatter formatter = new FasmX86Formatter();
            string lines = "";
            foreach (X86Instruction instr in instrs)
            {

                if (labels.Contains((ulong)instr.Offset))
                    lines += string.Format("\r\n@L{0:X}:\r\n", instr.Offset);

                if (MemOperand(instr) && labels.Contains((ulong)instr.Operand1.Value))
                    lines += string.Format("{0} @L{1:X}\r\n", instr.Mnemonic, instr.Operand1.Value);

                else
                    lines += formatter.FormatInstruction(instr) + Environment.NewLine;

            }
            return lines;
        }

        private List<ulong> GetLabels(List<X86Instruction> instrs)
        {
            List<ulong> labels = new List<ulong>();

            foreach (X86Instruction inst in instrs)
                if (MemOperand(inst))
                    labels.Add((ulong)(inst.Operand1.Value));

            return labels;

        }

        private static bool MemOperand(X86Instruction inst)
        {
            return 
                (inst.OpCode.OperandTypes1 != null &&
                    (inst.OpCode.OperandTypes1.Contains(X86OperandType.DirectAddress) ||
                    inst.OpCode.OperandTypes1.Contains(X86OperandType.MemoryAddress) ||
                    inst.OpCode.OperandTypes1.Contains(X86OperandType.RelativeOffset))
                );

        }

        private static List<X86Instruction> ReadInstructions(int offset, int size, MemoryStreamReader reader)
        {
            List<X86Instruction> instrs = new List<X86Instruction>();
            X86Disassembler disasm = new X86Disassembler(reader);
            while (reader.Position < offset + size)
                instrs.Add(disasm.ReadNextInstruction());
            return instrs;
        }
    }
}
