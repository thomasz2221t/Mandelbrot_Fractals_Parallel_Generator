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

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllCpp.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "generateMandelbrotFraktalCpp")]
        public static extern int generateMandelbrotFraktalCpp(byte[] imageBuffer, int bufforLength, int bitmapPieceX, int bitmapPieceY);

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllAsm.dll")]
        public static extern void MyProc1(ulong[] data);

        long iteratorInput; //the input value set by the user as number of iteration cycles
        int threadsInput; //number of threads to divide the bitmap into chosen by the user
        bool isAsmOrCpp; //if false then asm is set, if true cpp is set
        int resolutionX; //resolution horizontal parameter
        int resolutionY; //resolution vertical parameter 
        string outputFilePath; //path to the output file with fractal
        double maxRealis = 0.7;
        double minRealis = -1.5;
        double maxImaginaris = -1.0;
        double minImaginaris = 1;

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
            void runMandelbrotDllCpp(object partOfBitmap)
            {
                Bitmap bitmapFrame = (Bitmap) partOfBitmap;
                BitmapData bmpData = bitmapFrame.LockBits(new Rectangle(0,0, bitmapFrame.Width, bitmapFrame.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. 
                int bitmapBytesCount = Math.Abs(bmpData.Stride) * bitmapFrame.Height;//how many bytes there are
                byte[] bitmapValuesTable = new byte[bitmapBytesCount];//bytes table

                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(ptr, bitmapValuesTable, 0, bitmapBytesCount);

                for(int i=0; i<100; i++)
                {
                    Console.WriteLine("bit: {0}", bitmapValuesTable[i]);

                }
                //Executing the cpp function
                int result = generateMandelbrotFraktalCpp(bitmapValuesTable,bitmapBytesCount, bitmapFrame.Width, bitmapFrame.Height);
                Console.WriteLine("result: {0}", result);

                // Unlock the bits.
                bitmapFrame.UnlockBits(bmpData);
                return;
            }

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

            void saveBitmapToFile(Bitmap bitmapImage, string fileSaveDirection)
            {
                //if (System.IO.File.Exists(fileSaveDirection))
                //    System.IO.File.Delete(fileSaveDirection);

                //bitmapObject.Save(fileSaveDirection, System.Drawing.Imaging.ImageFormat.Bmp);

                if (System.IO.File.Exists("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp"))
                { 
                    System.IO.File.Delete("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp");
                }

                bitmapImage.Save("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp");
                bitmapImage.Dispose();//cleaning up after the image
                return;
            }

            void checkPartioningOfTheBitmap(Bitmap bitmapImage, string fileSaveDirection, int i)
            {
                bitmapImage.Save("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktalTest.bmp");
                bitmapImage.Dispose();//cleaning up after the image
                return;
            }

            void generateTheBitmap(int resolutionX, int resolutionY,Bitmap bitmapImage)
            {
                //Making integer table an bitmap
                //Bitmap bitmapObject = new Bitmap(resolutionX, resolutionY, PixelFormat.Format32bppRgb);

                //reseting color of bitmap
                for(int y=0; y<resolutionY; y++)
                {
                    for(int x=0; x<resolutionX; x++)
                    {
                        Color pixelColor = bitmapImage.GetPixel(x, y);
                        Color newColor = Color.FromArgb(0, 0, 0);//RGB 255 for white colour, RGB 0 for black colour
                        bitmapImage.SetPixel(x, y, newColor);
                    }
                }

                return;
            }

            Bitmap cropBitmap(Bitmap bitmapImage, Rectangle cutSection, int point_of_bitmapX, int point_of_bitmapY)
            {
                Bitmap cutedBitmap = new Bitmap(cutSection.Width, cutSection.Height);
                using (Graphics g = Graphics.FromImage(bitmapImage))
                {
                    g.DrawImage(bitmapImage, point_of_bitmapX, point_of_bitmapY, cutSection, GraphicsUnit.Pixel);
                    return cutedBitmap;
                }
            }

            Bitmap AppendBitmap(Bitmap bitmapInput, Bitmap bitmapOutput)
            {
                int w = Math.Max(bitmapInput.Width, bitmapOutput.Width);
                int h = bitmapInput.Height + bitmapOutput.Height;
                Bitmap mergeBitmap = new Bitmap(w, h);

                using (Graphics g = Graphics.FromImage(mergeBitmap))
                {
                    g.DrawImage(bitmapInput, 0, 0);
                    g.DrawImage(bitmapOutput, 0, bitmapInput.Height);
                }

                return mergeBitmap;
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

            //Making integer table an bitmap
            Bitmap bitmapObject = new Bitmap(resolutionX, resolutionY, PixelFormat.Format32bppRgb);
            generateTheBitmap(resolutionX, resolutionY,bitmapObject);

            //saveBitmapToFile(bitmapObject, outputFilePath);

            Thread[] array_of_threads = new Thread[threadsInput]; //tablica do przechowywania wątków

            if(isAsmOrCpp)
            {
                //cpp
                for(int i = 0; i<threadsInput; i++)
                {
                    Rectangle section = new Rectangle(new Point(0,(resolutionY/threadsInput)*i), new Size(resolutionX, resolutionY/ threadsInput));
                    Bitmap partOfBitmap = cropBitmap(bitmapObject, section, 0, (resolutionY / threadsInput) * i);

                    int x = partOfBitmap.Width;
                    int y = partOfBitmap.Height;

                    array_of_threads[i] = new Thread(runMandelbrotDllCpp);
                    array_of_threads[i].Start(partOfBitmap);


                    //if(i==0)
                    //{
                    //checkPartioningOfTheBitmap(partOfBitmap,"",i);
                   // }
                    //partOfBitmap.Dispose();
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

            //for (int i = 0; i < threadsInput; i++)
            //{
            //    array_of_threads[i].Start(2);//rozpoczynamy watki
            //    Thread.Sleep(100);
            //}

            //for (int i = 0; i < threadsInput; i++)
            //{

            //     array_of_threads[i].Join();//synchronizacja watkow (bez sensu jak juz sie koncza
            //}

            stopWatch.Stop();

            TimeSpan time_period = stopWatch.Elapsed;
            string elapsedTime = time_period.TotalMilliseconds.ToString();
            Console.WriteLine("Time of executing dll library in miliseconds {0} for dll library in language: {1}", elapsedTime, (isAsmOrCpp ? "cpp" : "asm"));

            //int z = generateMandelbrotFraktalCpp(3, 4);
           // Console.WriteLine("Dll Cpp: {0}", z);


            ulong[] data = new ulong[4] { 0, 0, 0, 0 };
            Console.WriteLine("{0:X16}", data[0]);
            MyProc1(data);
            Console.WriteLine("{0:X16}", data[0]);

        }
    }
}
