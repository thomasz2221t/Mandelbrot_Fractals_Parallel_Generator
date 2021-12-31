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
using System.IO;

namespace MandelbrotApp
{
    public partial class appWindow : Form
    {
        public appWindow()
        {
            InitializeComponent();
        }

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllCpp.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "generateMandelbrotFraktalCpp")]
        public static extern long generateMandelbrotFraktalCpp(byte[] imageBuffer, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, long maxIteration, double minR, double maxR, double minI, double maxI);

        [DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllAsm.dll")]
        public static extern void generateMandelbrotFraktalAsm(byte[] imageBuffer, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, long maxIteration, double minR, double maxR, double minI, double maxI);

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

        static Mutex objMutex = new Mutex();
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
            //void runMandelbrotDllCpp(Bitmap partOfBitmap, int bitMapX, int bitMapY, int dimensionX, int dimensionY, long maxIteration, int numThreads, double minR, double maxR, double minI, double maxI)
            void runMandelbrotDllCpp(byte[] bitMapValuesTable, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, long maxIteration, double minR, double maxR, double minI, double maxI)
            {
                Console.WriteLine("Threat input: begin at: {0}, size: {1}", subTabBeginPoint, sizeOfSubTable);
                //Bitmap bitmapFrame = partOfBitmap;
                //BitmapData bmpData = bitmapFrame.LockBits(new Rectangle(0,0, bitmapFrame.Width, bitmapFrame.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                // Get the address of the first line.
                //IntPtr ptr = bmpData.Scan0;
                // Declare an array to hold the bytes of the bitmap. 
                //int bitmapBytesCount = Math.Abs(bmpData.Stride) * bitmapFrame.Height;//how many bytes there are
                //byte[] bitmapValuesTable = new byte[bitmapBytesCount];//bytes table
                // Copy the RGB values into the array.
                //System.Runtime.InteropServices.Marshal.Copy(ptr, bitmapValuesTable, 0, bitmapBytesCount);

                byte[] partOfBmTable = new byte[3*sizeOfSubTable];

                //Executing the cpp function
                long result = generateMandelbrotFraktalCpp(partOfBmTable, subTabBeginPoint, sizeOfSubTable, dimensionX, dimensionY, maxIteration, minR, maxR, minI, maxI);
                Console.WriteLine("result: {0}", result);


                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting..." );
                objMutex.WaitOne();
                /*for (long i = subTabBeginPoint; i < 3*sizeOfSubTable; i++)
                {
                    Console.WriteLine("{0}: {1} ", i, partOfBmTable[i]);
                }*/

                for (long  i = 3*subTabBeginPoint; i<3*(sizeOfSubTable+subTabBeginPoint); i++)
                {
                    bitMapValuesTable[i] = partOfBmTable[i-3*subTabBeginPoint];
                }
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Finished Writting...");
                objMutex.ReleaseMutex();

                // foreach(byte parame in bitmapValuesTable)
                //{
                //    Console.WriteLine("{0:X16}",parame);
                //}
                // Unlock the bits.
                //bitmapFrame.UnlockBits(bmpData);
                return;
            }

            void runMandelbrotDllAsm(byte[] bitMapValuesTable, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, long maxIteration, double minR, double maxR, double minI, double maxI)
            {
                byte[] partOfBmTable = new byte[3 * sizeOfSubTable];




                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
                objMutex.WaitOne();
                /*for (long i = subTabBeginPoint; i < 3*sizeOfSubTable; i++)
                {
                    Console.WriteLine("{0}: {1} ", i, partOfBmTable[i]);
                }*/

                for (long i = 3 * subTabBeginPoint; i < 3 * (sizeOfSubTable + subTabBeginPoint); i++)
                {
                    bitMapValuesTable[i] = partOfBmTable[i - 3 * subTabBeginPoint];
                }
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Finished Writting...");
                objMutex.ReleaseMutex();
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

           /* void saveBitmapToFile(Bitmap bitmapImage, string fileSaveDirection)
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
            }*/

            void saveBitmapToFile(byte[] bitmapTable, string fileSaveDirection)
            {

                if (System.IO.File.Exists("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp"))
                {
                    System.IO.File.Delete("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp");
                }

                using (var bitmapImage = new Bitmap(resolutionX, resolutionY, PixelFormat.Format24bppRgb))
                {
                    BitmapData bmpData = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, bitmapImage.PixelFormat);

                    Marshal.Copy(bitmapTable, 0, bmpData.Scan0, bitmapTable.Length);

                    bitmapImage.UnlockBits(bmpData);

                    bitmapImage.Save("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp");
                }
                Console.WriteLine("Written to file!!");
                //bitmapImage.Dispose();//cleaning up after the image
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
                        Color newColor = Color.FromArgb(255, 255, 255);//RGB 255 for white colour, RGB 0 for black colour
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
            //Bitmap bitmapObject = new Bitmap(resolutionX, resolutionY, PixelFormat.Format32bppRgb);
            //generateTheBitmap(resolutionX, resolutionY,bitmapObject);
            //saveBitmapToFile(bitmapObject, outputFilePath);

            //making 1dim byte array that suits the resolution
            byte[] bitMapPixelValues = new byte [3*resolutionX*resolutionY];//3RGB values * resolutionX*resolutionY

            Thread[] array_of_threads = new Thread[threadsInput]; //tablica do przechowywania wątków
            Stopwatch stopWatch = new Stopwatch();//obsluga czasu

            if (isAsmOrCpp)
            {
                //cpp
                long sizeOfSubTable = (resolutionX * resolutionY) / threadsInput;
                int offset = 0;
                if (sizeOfSubTable * threadsInput < resolutionX * resolutionY)
                {
                    offset = (int)((resolutionX * resolutionY) - (sizeOfSubTable * threadsInput));
                    sizeOfSubTable += offset;
                }

                stopWatch.Start();
                for (int i = 0; i<threadsInput; i++)
                {
                    //Rectangle section = new Rectangle(new Point(0,(resolutionY/threadsInput)*i), new Size(resolutionX, resolutionY/ threadsInput));
                    //Bitmap partOfBitmap = cropBitmap(bitmapObject, section, 0, (resolutionY / threadsInput) * i);
                    //int x = partOfBitmap.Width;
                    //int y = partOfBitmap.Height;

                    //correct the begginig of the second thread tab beggining
                    if ((offset > 0)&&(i == 1))//if offset == 0 no effect
                    {
                        sizeOfSubTable -= offset;
                    }

                    long subTabBeginPoint = 0;
                    if(i != 0)
                    {
                        subTabBeginPoint = (sizeOfSubTable * i) + offset;
                    }

                    array_of_threads[i] = new Thread(unused => runMandelbrotDllCpp(bitMapPixelValues, subTabBeginPoint, sizeOfSubTable, resolutionX, resolutionY, iteratorInput, minRealis, maxRealis, minImaginaris, maxImaginaris));
                    array_of_threads[i].Start();
                    //Thread.Sleep(1000);

                    //if(i==0)
                    // {
                    //checkPartioningOfTheBitmap(partOfBitmap,"",i);
                    //  }
                    //partOfBitmap.Dispose();
                }
                
            }
            else
            {
                //asm
                long sizeOfSubTable = (resolutionX * resolutionY) / threadsInput;
                int offset = 0;
                if (sizeOfSubTable * threadsInput < resolutionX * resolutionY)
                {
                    offset = (int)((resolutionX * resolutionY) - (sizeOfSubTable * threadsInput));
                    sizeOfSubTable += offset;
                }

                for (int i = 0; i<threadsInput; i++)
                {
                    //correct the begginig of the second thread tab beggining
                    if ((offset > 0) && (i == 1))//if offset == 0 no effect
                    {
                        sizeOfSubTable -= offset;
                    }

                    long subTabBeginPoint = 0;
                    if (i != 0)
                    {
                        subTabBeginPoint = (sizeOfSubTable * i) + offset;
                    }

                    array_of_threads[i] = new Thread(unused => runMandelbrotDllAsm(bitMapPixelValues, subTabBeginPoint, sizeOfSubTable, resolutionX, resolutionY, iteratorInput, minRealis, maxRealis, minImaginaris, maxImaginaris));
                    array_of_threads[i].Start();
                }
            }

            //for (int i = 0; i < threadsInput; i++)
            //{
            //    array_of_threads[i].Start(2);//rozpoczynamy watki
            //    Thread.Sleep(100);
            //}

             /*bool threadAlive = true;
             while(threadAlive == false)
             {
                 threadAlive = false;
                 for (int i = 0; i < array_of_threads.Length; i++)
                 {
                     //array_of_threads[i].Join();//synchronizacja watkow (bez sensu jak juz sie koncza
                     if(threadAlive == false)
                     {
                         threadAlive = array_of_threads[i].IsAlive;
                     }
                 }
             }*/

            foreach (Thread workingThread in array_of_threads)
            {
                workingThread.Join();
            }

            /*bool threadAlive = true;
            while (threadAlive == false)
            {
                threadAlive = true;
                for (int i = 0; i < array_of_threads.Length; i++)
                {
                    //array_of_threads[i].Join();//synchronizacja watkow (bez sensu jak juz sie koncza
                    threadAlive = array_of_threads[i].IsAlive;
                }
            }*/

            stopWatch.Stop();

            TimeSpan time_period = stopWatch.Elapsed;
            string elapsedTime = time_period.TotalMilliseconds.ToString();
            Console.WriteLine("Time of executing dll library in miliseconds {0} for dll library in language: {1}", elapsedTime, (isAsmOrCpp ? "cpp" : "asm"));

            //checking bitMapPixelValue table
            /*for (int i = 0; i < bitMapPixelValues.Length; i++)
             {
                 Console.Write("{0}: {1}, ",i,bitMapPixelValues[i]);
             }*/

            saveBitmapToFile(bitMapPixelValues, outputFilePath);



        }
    }
}
