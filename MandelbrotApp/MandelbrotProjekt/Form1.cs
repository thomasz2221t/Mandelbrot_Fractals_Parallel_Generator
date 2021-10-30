using System;
using System.Windows.Forms;

namespace MandelbrotProjekt
{
    public partial class appWindow : Form
    {
        public appWindow()
        {
            InitializeComponent();
        }

        long iteratorInput; //the input value set by the user as number of iteration cycles
        int threatsInput; //number of threats to divide the bitmap into chosen by the user
        bool isAsmOrCpp; //if false then asm is set, if true cpp is set
        int resolutionX; //resolution horizontal parameter
        int resolutionY; //resolution vertical parameter 
        string outputFilePath; //path to the output file with fractal

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
            if(cppBox.Checked)
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
            iterationButton_Click(sender,EventArgs.Empty);
            threatsButton_Click(sender, EventArgs.Empty);
            resolutionButton_Click(sender, EventArgs.Empty);
            outputButton_Click(sender, EventArgs.Empty);
            Console.WriteLine("iterator: {0}", iteratorInput);
            Console.WriteLine("threats: {0}", threatsInput);
            Console.WriteLine("cpp/asm: {0}", (isAsmOrCpp ? "cpp" : "asm"));
            Console.WriteLine("resolution: {0} x {1}", resolutionX, resolutionY);
            Console.WriteLine("file path: {0}", outputFilePath);
        }
    }
}
