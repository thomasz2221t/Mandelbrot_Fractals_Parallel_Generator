using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MandelbrotApp
{
    public partial class appWindow : Form
    {
        public appWindow()
        {
            InitializeComponent();
        }

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllCpp.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Procedura1")]
        public static extern int Procedura1(int x, int y);

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllAsm.dll")]
        public static extern void MyProc1(ulong[] data);

        long iteratorInput; //the input value set by the user as number of iteration cycles
        int threatsInput; //number of threats to divide the bitmap into chosen by the user
        bool isAsmOrCpp; //if false then asm is set, if true cpp is set
        int resolutionX; //resolution horizontal parameter
        int resolutionY; //resolution vertical parameter 
        string outputFilePath; //path to the output file with fractal

        private void iterationLabel_Click(object sender, EventArgs e)
        {

        }

        private void iterationBox_TextChanged(object sender, EventArgs e)
        {
            //check if the value is a number
        }

        private void iterationButton_Click(object sender, EventArgs e)
        {
            iteratorInput = long.Parse(iterationBox.Text);
        }

        private void threatsUpDown_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void threatsButton_Click(object sender, EventArgs e)
        {
            threatsInput = (int)threatsUpDown.Value;
        }

        private void asmBox_CheckedChanged(object sender, EventArgs e)
        {
            if (asmBox.Checked)
            {
                isAsmOrCpp = false;
                if (cppBox.Checked)
                {
                    cppBox.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void cppBox_CheckedChanged(object sender, EventArgs e)
        {
            if (cppBox.Checked)
            {
                isAsmOrCpp = true;
                if (asmBox.Checked)
                {
                    asmBox.CheckState = CheckState.Unchecked;

                }
            }
        }

        private void resolutionXBox_TextChanged(object sender, EventArgs e)
        {
            //check if the value is number
        }

        private void resolutionYBox_TextChanged(object sender, EventArgs e)
        {
            //check if the value is number
        }

        private void resolutionButton_Click(object sender, EventArgs e)
        {
            resolutionX = int.Parse(resolutionXBox.Text);
            resolutionY = int.Parse(resolutionYBox.Text);
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void outputButton_Click(object sender, EventArgs e)
        {
            outputFilePath = outputBox.Text;
        }

        private void proceedButton_Click(object sender, EventArgs e)
        {
            iterationButton_Click(sender, EventArgs.Empty);
            threatsButton_Click(sender, EventArgs.Empty);
            resolutionButton_Click(sender, EventArgs.Empty);
            outputButton_Click(sender, EventArgs.Empty);
            Console.WriteLine("iterator: {0}", iteratorInput);
            Console.WriteLine("threats: {0}", threatsInput);
            Console.WriteLine("cpp/asm: {0}", (isAsmOrCpp ? "cpp" : "asm"));
            Console.WriteLine("resolution: {0} x {1}", resolutionX, resolutionY);
            Console.WriteLine("file path: {0}", outputFilePath);

            int z = Procedura1(3, 4);
            Console.WriteLine("Dll Cpp: {0}", z);


            ulong[] data = new ulong[4] { 0, 0, 0, 0 };
            Console.WriteLine("{0:X16}", data[0]);
            MyProc1(data);
            Console.WriteLine("{0:X16}", data[0]);
        }
    }
}
