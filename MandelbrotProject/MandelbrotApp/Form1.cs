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
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;

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
        int threadsInput; //number of threads to divide the bitmap into chosen by the user
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
            threadsInput = (int)threatsUpDown.Value;
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
            void thread_function1(object a)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("The thread's numer is {0} i= {1}", a, i);
                }
                return;
            }
            /*
            void displayBitmap(Bitmap bitmapObject)
            {
                //PictureBox1.Image = bitmapObject;

                // Display the pixel format in Label1.
                //Label1.Text = "Pixel format: " + bitmapObject.PixelFormat.ToString();
            }
            */

            void saveBitmapToFile(Bitmap bitmapObject, string fileSaveDirection)
            {
                //bitmapObject.Save(fileSaveDirection, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapObject.Save("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                return;
            }

            Bitmap generateTheBitmap(int resolutionX, int resolutionY)
            {
                //Making integer table an bitmap
                Bitmap bitmapObject = new Bitmap(resolutionX, resolutionY, PixelFormat.Format32bppRgb);

                //reseting color of bitmap
                for(int y=0; y<resolutionY; y++)
                {
                    for(int x=0; x<resolutionX; x++)
                    {
                        Color pixelColor = bitmapObject.GetPixel(x, y);
                        Color newColor = Color.FromArgb(0, 0, 0);//RGB 255 for white colour, RGB 0 for black colour
                        bitmapObject.SetPixel(x, y, newColor);
                    }
                }
                return bitmapObject;
            }

            iterationButton_Click(sender, EventArgs.Empty);
            threatsButton_Click(sender, EventArgs.Empty);
            resolutionButton_Click(sender, EventArgs.Empty);
            outputButton_Click(sender, EventArgs.Empty);
            Console.WriteLine("iterator: {0}", iteratorInput);
            Console.WriteLine("threats: {0}", threadsInput);
            Console.WriteLine("cpp/asm: {0}", (isAsmOrCpp ? "cpp" : "asm"));
            Console.WriteLine("resolution: {0} x {1}", resolutionX, resolutionY);
            Console.WriteLine("file path: {0}", outputFilePath);

            Bitmap bitmapImage = generateTheBitmap(resolutionX,resolutionY);

            saveBitmapToFile(bitmapImage, outputFilePath);

            Thread[] array_of_threads = new Thread[threadsInput]; //tablica do przechowywania wątków

            if(isAsmOrCpp)
            {
                //cpp
                for(int i = 0; i<threadsInput; i++)
                {
                    array_of_threads[i] = new Thread(thread_function1);
                }
            }
            else
            {
                //asm
                for(int i = 0; i<threadsInput; i++)
                {
                    array_of_threads[i] = new Thread(thread_function1);
                }
            }    

            Stopwatch stopWatch = new Stopwatch();//obsluga czasu
            stopWatch.Start();

            for (int i = 0; i < threadsInput; i++)
            {
                array_of_threads[i].Start(2);//rozpoczynamy watki
                Thread.Sleep(100);
            }

            //for (int i = 0; i < threadsInput; i++)
            //{

            //     array_of_threads[i].Join();//synchronizacja watkow (bez sensu jak juz sie koncza
            //}

            stopWatch.Stop();

            TimeSpan time_period = stopWatch.Elapsed;
            string elapsedTime = time_period.TotalMilliseconds.ToString();
            Console.WriteLine("Time of executing dll library in miliseconds {0} for dll library in language: {1}", elapsedTime, (isAsmOrCpp ? "cpp" : "asm"));

            int z = Procedura1(3, 4);
            Console.WriteLine("Dll Cpp: {0}", z);


            ulong[] data = new ulong[4] { 0, 0, 0, 0 };
            Console.WriteLine("{0:X16}", data[0]);
            MyProc1(data);
            Console.WriteLine("{0:X16}", data[0]);

        }
    }
}
