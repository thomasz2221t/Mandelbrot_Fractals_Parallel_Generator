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
using System.Globalization;
using System.Text.RegularExpressions;

namespace MandelbrotApp
{
    public partial class appWindow : Form
    {
        public appWindow()
        {
            InitializeComponent();
        }

        //Debug
        //[DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllCpp.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "generateMandelbrotFraktalCpp")]
        //Release
        [DllImport(@"C:\Users\0_0\Documents\Assembler_projekt\tc2221t_assembler_mandelbrot\MandelbrotProject\x64\Release\MandelbrotDllCpp.dll")]
        public static extern long generateMandelbrotFraktalCpp(byte[] bitMapValuesTable, double[] tableMappedToReal, double[] tableMappedToImaginaris, long subTabBeginPoint, long sizeOfSubTableCpp, long maxIteration);

        //Debug
        //[DllImport("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\x64\\Debug\\MandelbrotDllAsm.dll")]
        //Release
        [DllImport(@"C:\Users\0_0\Documents\Assembler_projekt\tc2221t_assembler_mandelbrot\MandelbrotProject\x64\Release\MandelbrotDllAsm.dll")]
        public static extern long generateMandelbrotFraktalAsm(double[] imageBuffer, double[] tableMappedToReal, double[] tableMappedToImaginaris, long subTabBeginPoint, long sizeOfSubTableAsm, long maxIteration);
       
        long iteratorInput; //the input value set by the user as number of iteration cycles
        int threadsInput; //number of threads to divide the bitmap into chosen by the user
        bool isAsmOrCpp; //if false then asm is set, if true cpp is set
        int resolutionX; //resolution horizontal parameter
        int resolutionY; //resolution vertical parameter 
        string outputFilePath; //path to the output file with fractal
        double maxRealis; //0.7
        double minRealis; //-1.5
        double maxImaginaris; //1.0
        double minImaginaris; //-1.0

        bool validationOkIteration = false;
        bool validationOkComplexPlane = false;
        bool validationOkResolution = false;
        bool validationOkFilePath = false;

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
            //check if the value is a number
            bool validation = validateNumberOnly(iterationBox.Text);
            if (!validation)
            {
                MessageBox.Show("Incorrect input. Number of iteration parameter must be a number.");
                iterationBox.BackColor = Color.Red;
                validationOkIteration = false;
            }
            else
            {
                iterationBox.BackColor = SystemColors.Window;
                iteratorInput = long.Parse(iterationBox.Text);
                validationOkIteration = true;
            }
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
            bool validationX = validateNumberOnly(resolutionXBox.Text);
            bool validationY = validateNumberOnly(resolutionYBox.Text);
            bool validationOkX;
            bool validationOkY;
            if (!validationX)
            {
                MessageBox.Show("Incorrect input. Dimension X parameter must be a number.");
                resolutionXBox.BackColor = Color.Red;
                validationOkX = false;
                validationOkResolution = false;
            }
            else
            {
                resolutionXBox.BackColor = SystemColors.Window;
                validationOkX = true;
            }

            if (!validationY)
            {
                MessageBox.Show("Incorrect input. Dimension Y parameter must be a number.");
                resolutionYBox.BackColor = Color.Red;
                validationOkY = false;
                validationOkResolution = false;
            }
            else
            {
                resolutionYBox.BackColor = SystemColors.Window;
                validationOkY = true;
            }

            if ((validationOkX) && (validationOkY))
            {
                resolutionX = int.Parse(resolutionXBox.Text);
                resolutionY = int.Parse(resolutionYBox.Text);
                validationOkResolution = true;
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void outputButton_Click(object sender, EventArgs e)
        {

            if (outputBox.Text.Length > 0)
            {
                if(File.Exists(outputBox.Text))
                {
                    outputFilePath = outputBox.Text;
                    validationOkFilePath = true;
                }
                else
                {
                    MessageBox.Show("The file in given path does not exist");
                    validationOkFilePath = false;
                }
            }
            else
            {
                OpenFileDialog v1 = new OpenFileDialog();
                v1.Title = "Select saving direction of Mandelbrot fractal.";
                v1.Filter = "All files (*.*)|*.*";
                if (v1.ShowDialog() == DialogResult.OK)
                {
                    outputBox.Text = v1.FileName;
                    validationOkFilePath = true;
                }
                else
                {
                    validationOkFilePath = false;
                }
            }
            //powiadomienie, ze sciezka nie istnieje
        }

        private void complexPlaneButton_Click(object sender, EventArgs e)
        {
            bool validationMaxRealis = validateDoubleOnly(maxRealisText.Text);
            bool validationMinRealis = validateDoubleOnly(minRealisText.Text);
            bool validationMaxImaginaris = validateDoubleOnly(maxImaginarisText.Text);
            bool validationMinImaginaris = validateDoubleOnly(minImaginarisText.Text);
            bool validationOkMaxRealis;
            bool validationOkMinRealis;
            bool validationOkMaxImaginaris;
            bool validationOkMinImaginaris;
            if (!validationMaxRealis)
            {
                MessageBox.Show("Incorrect input. Maximum Realis parameter must be a double precission floating point value.");
                maxRealisText.BackColor = Color.Red;
                validationOkMaxRealis = false;
                validationOkComplexPlane = false;
            }
            else
            {
                maxRealisText.BackColor = SystemColors.Window;
                validationOkMaxRealis = true;
            }

            if (!validationMinRealis)
            {
                MessageBox.Show("Incorrect input. Minimum Realis parameter must be a double precission floating point value.");
                minRealisText.BackColor = Color.Red;
                validationOkMinRealis = false;
                validationOkComplexPlane = false;
            }
            else
            {
                minRealisText.BackColor = SystemColors.Window;
                validationOkMinRealis = true;
            }

            if (!validationMaxImaginaris)
            {
                MessageBox.Show("Incorrect input. Maximum Imaginaris parameter must be a double precission floating point value.");
                maxImaginarisText.BackColor = Color.Red;
                validationOkMaxImaginaris = false;
                validationOkComplexPlane = false;
            }
            else
            {
                maxImaginarisText.BackColor = SystemColors.Window;
                validationOkMaxImaginaris = true;
            }

            if (!validationMinImaginaris)
            {
                MessageBox.Show("Incorrect input. Minimum Imaginaris parameter must be a double precission floating point value.");
                minImaginarisText.BackColor = Color.Red;
                validationOkMinImaginaris = false;
                validationOkComplexPlane = false;
            }
            else
            {
                minImaginarisText.BackColor = SystemColors.Window;
                validationOkMinImaginaris = true;
            }

            if((validationOkMaxRealis) &&(validationOkMinRealis) &&(validationOkMaxImaginaris) &&(validationOkMinImaginaris))
            {
                maxRealis = double.Parse(maxRealisText.Text, CultureInfo.InvariantCulture);
                minRealis = double.Parse(minRealisText.Text, CultureInfo.InvariantCulture);
                maxImaginaris = double.Parse(maxImaginarisText.Text, CultureInfo.InvariantCulture);
                minImaginaris = double.Parse(minImaginarisText.Text, CultureInfo.InvariantCulture);
                validationOkComplexPlane = true;
            }
        }

        private void mapToRealisAndImaginaris(double[] tabWithRealis, double[] tabWithImaginaris, long subTabBeginPoint, long sizeOfSubTable, int dimensionX, int dimensionY, double minR, double maxR, double minI, double maxI)
        {
            for (long i = subTabBeginPoint; i < (subTabBeginPoint + sizeOfSubTable); i++)
            {
                int x = (int)i % dimensionX;
                int y = (int)i / dimensionX;

                double range = maxR - minR;
                tabWithRealis[i - subTabBeginPoint] = x * (range / dimensionX) + minR;
                range = maxI - minI;
                tabWithImaginaris[i - subTabBeginPoint] = y * (range / dimensionY) + minI;
            }
            return;
        }

        private void runMandelbrotDllCpp(byte[] bitMapValuesTable, double[] tableMappedToReal, double[] tableMappedToImaginaris, long subTabBeginPoint, long sizeOfSubTableCpp, long maxIteration)
        {
            Console.WriteLine("Threat input: begin at: {0}, size: {1}", subTabBeginPoint, sizeOfSubTableCpp);
            //Bitmap bitmapFrame = partOfBitmap;
            //BitmapData bmpData = bitmapFrame.LockBits(new Rectangle(0,0, bitmapFrame.Width, bitmapFrame.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            // Get the address of the first line.
            //IntPtr ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap. 
            //int bitmapBytesCount = Math.Abs(bmpData.Stride) * bitmapFrame.Height;//how many bytes there are
            //byte[] bitmapValuesTable = new byte[bitmapBytesCount];//bytes table
            // Copy the RGB values into the array.
            //System.Runtime.InteropServices.Marshal.Copy(ptr, bitmapValuesTable, 0, bitmapBytesCount);

            byte[] partOfBmTable = new byte[3 * sizeOfSubTableCpp];

            //Executing the cpp function
            long result = generateMandelbrotFraktalCpp(partOfBmTable, tableMappedToReal, tableMappedToImaginaris, subTabBeginPoint, sizeOfSubTableCpp, maxIteration);
            Console.WriteLine("result: {0}", result);


           // Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            //objMutex.WaitOne();
            /*for (long i = subTabBeginPoint; i < 3*sizeOfSubTable; i++)
            {
                Console.WriteLine("{0}: {1} ", i, partOfBmTable[i]);
            }*/

            for (long i = 3 * subTabBeginPoint; i < 3 * (sizeOfSubTableCpp + subTabBeginPoint); i++)
            {
                bitMapValuesTable[i] = partOfBmTable[i - 3 * subTabBeginPoint];
            }
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Finished Writting...");
            //objMutex.ReleaseMutex();

            // foreach(byte parame in bitmapValuesTable)
            //{
            //    Console.WriteLine("{0:X16}",parame);
            //}
            //}
            // Unlock the bits.
            //bitmapFrame.UnlockBits(bmpData);
            return;
        }

        private void runMandelbrotDllAsm(byte[] bitMapValuesTable, double[] tableMappedToReal, double[] tableMappedToImaginaris, long subTabBeginPoint, long sizeOfSubTableAsm, long maxIteration, int offsetAsm)
        {

            /*for (long i = subTabBeginPoint; i < sizeOfSubTable; i++)
            {
                Console.WriteLine("Realis{0}: {1} ", i, tableMappedToReal[i]);
            }

            for (long i = subTabBeginPoint; i < sizeOfSubTable; i++)
            {
                Console.WriteLine("Imaginaris: {0}: {1} ", i, tableMappedToImaginaris[i]);
            }*/

            double[] partOfBmTable = new double[sizeOfSubTableAsm + offsetAsm]; //tablica przechowujaca ilosc iteracji wykonana dla pikseli

            long iteracja = generateMandelbrotFraktalAsm(partOfBmTable, tableMappedToReal, tableMappedToImaginaris, subTabBeginPoint, sizeOfSubTableAsm, maxIteration);

            //Console.WriteLine("liczba iteracji: {0}", iteracja);
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Waiting...");
            //objMutex.WaitOne();
            /*for (long i = subTabBeginPoint; i < sizeOfSubTable; i++)
            {
                Console.WriteLine("{0}: {1} ", i, partOfBmTable[i]);
            }*/

            for (long i = 0; i < (sizeOfSubTableAsm); i++)
            {
                int n = (int)(partOfBmTable[i] / 4);
                //int r = ((int)(n * Math.Sinh(n)) % 256);
                int r = ((n * 2 * n) % 256);
                int g = ((n * n) % 256);
                int b = (n % 256);
                bitMapValuesTable[3 * (i + subTabBeginPoint)] = (byte)b;
                bitMapValuesTable[3 * (i + subTabBeginPoint) + 1] = (byte)g;
                bitMapValuesTable[3 * (i + subTabBeginPoint) + 2] = (byte)r;
            }
            //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Finished Writting...");
            //objMutex.ReleaseMutex();
            return;
        }

        private void saveBitmapToFile(byte[] bitmapTable, string fileSaveDirection)
        {
            // if (System.IO.File.Exists("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktal.bmp"))
            if (System.IO.File.Exists(outputFilePath))
            {
                //if(pictureFractal.Image != null)
                //{
                //    pictureFractal.CancelAsync();
                //    pictureFractal.Image.Dispose();
                //    pictureFractal.Image = null;
                //}
                System.IO.File.Delete(outputFilePath);
            }

            using (var bitmapImage = new Bitmap(resolutionX, resolutionY, PixelFormat.Format24bppRgb))
            {
                BitmapData bmpData = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, bitmapImage.PixelFormat);

                Marshal.Copy(bitmapTable, 0, bmpData.Scan0, bitmapTable.Length);

                bitmapImage.UnlockBits(bmpData);

                bitmapImage.Save(outputFilePath);
            }
            Console.WriteLine("Written to file!!");
            pictureFractal.Image = new Bitmap(outputFilePath, true);
            //bitmapImage.Dispose();//cleaning up after the image
            return;
        }

        private static bool validateNumberOnly(string inputNumber)
        {
            string regex = @"[0-9]";
            Regex regexObject = new Regex(regex);
            if (regexObject.IsMatch(inputNumber))
                return (true);
            else
                return (false);
        }

        private static bool validateDoubleOnly(string inputDouble)
        {
            string regex = @"[+-]?([0-9]*[.])";
            Regex regexObject = new Regex(regex);
            if (regexObject.IsMatch(inputDouble))
                return (true);
            else
                return (false);
        }

        private void proceedButton_Click(object sender, EventArgs e)
        {
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

            /*
            void checkPartioningOfTheBitmap(Bitmap bitmapImage, string fileSaveDirection, int i)
            {
                bitmapImage.Save("C:\\Users\\0_0\\Documents\\Assembler_projekt\\tc2221t_assembler_mandelbrot\\MandelbrotProject\\OutputFiles\\MandelbrotFraktalTest.bmp");
                bitmapImage.Dispose();//cleaning up after the image
                return;
            }*/

            /*
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
            }*/

            /*
            Bitmap cropBitmap(Bitmap bitmapImage, Rectangle cutSection, int point_of_bitmapX, int point_of_bitmapY)
            {
                Bitmap cutedBitmap = new Bitmap(cutSection.Width, cutSection.Height);
                using (Graphics g = Graphics.FromImage(bitmapImage))
                {
                    g.DrawImage(bitmapImage, point_of_bitmapX, point_of_bitmapY, cutSection, GraphicsUnit.Pixel);
                    return cutedBitmap;
                }
            }*/

            /*Bitmap AppendBitmap(Bitmap bitmapInput, Bitmap bitmapOutput)
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
            }*/
            
                iterationButton_Click(sender, EventArgs.Empty);
                threatsButton_Click(sender, EventArgs.Empty);
                resolutionButton_Click(sender, EventArgs.Empty);
                outputButton_Click(sender, EventArgs.Empty);
                complexPlaneButton_Click(sender, EventArgs.Empty);
            if ((validationOkIteration) &&(validationOkResolution)&&(validationOkComplexPlane)&&(validationOkFilePath))
            {
                if (pictureFractal.Image != null)
                {
                    pictureFractal.CancelAsync();
                    pictureFractal.Image.Dispose();
                    pictureFractal.Image = null;
                }
                Console.WriteLine("iterator: {0}", iteratorInput);
                Console.WriteLine("threats: {0}", threadsInput);
                Console.WriteLine("cpp/asm: {0}", (isAsmOrCpp ? "cpp" : "asm"));
                Console.WriteLine("resolution: {0} x {1}", resolutionX, resolutionY);
                Console.WriteLine("file path: {0}", outputFilePath);

                progressBar.Value=0;
                //calculate offset for the progress bar, maximum of progress bar is 100 2 + liczba watkow
                int progressBarOffset = progressBar.Maximum/(threadsInput+3);
                progressBar.Increment(progressBarOffset);

                //Making integer table an bitmap
                //Bitmap bitmapObject = new Bitmap(resolutionX, resolutionY, PixelFormat.Format32bppRgb);
                //generateTheBitmap(resolutionX, resolutionY,bitmapObject);
                //saveBitmapToFile(bitmapObject, outputFilePath);

                //making 1dim byte array that suits the resolution
                byte[] bitMapPixelValues = new byte [3*resolutionX*resolutionY];//3RGB values * resolutionX*resolutionY

                Thread[] array_of_threads = new Thread[threadsInput]; //tablica do przechowywania wątków
                Stopwatch stopWatch = new Stopwatch();//obsluga czasu

                long sizeOfSubTable = (resolutionX * resolutionY) / threadsInput;
                int offset = 0;
                if (sizeOfSubTable * threadsInput < resolutionX * resolutionY)
                {
                    offset = (int)((resolutionX * resolutionY) - (sizeOfSubTable * threadsInput));
                    sizeOfSubTable += offset;
                }

                //stopWatch.Start();
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
                    if (isAsmOrCpp)
                    {
                        double[] subTabWithRealis = new double[sizeOfSubTable];
                        double[] subTabWithImaginaris = new double[sizeOfSubTable];

                        mapToRealisAndImaginaris(subTabWithRealis, subTabWithImaginaris, subTabBeginPoint, sizeOfSubTable, resolutionX, resolutionY, minRealis, maxRealis, minImaginaris, maxImaginaris);
                        //cpp
                        stopWatch.Start();
                        array_of_threads[i] = new Thread(unused => runMandelbrotDllCpp(bitMapPixelValues, subTabWithRealis, subTabWithImaginaris, subTabBeginPoint, sizeOfSubTable, iteratorInput));
                        array_of_threads[i].Start();
                    }
                    else
                    {
                        int offsetAsm = 0;
                        if (sizeOfSubTable % 4 != 0)
                        {
                            offsetAsm = (int)sizeOfSubTable % 4;
                        }
                        double[] subTabWithRealis = new double[sizeOfSubTable + offsetAsm];
                        double[] subTabWithImaginaris = new double[sizeOfSubTable + offsetAsm];

                        mapToRealisAndImaginaris(subTabWithRealis, subTabWithImaginaris, subTabBeginPoint, sizeOfSubTable, resolutionX, resolutionY, minRealis, maxRealis, minImaginaris, maxImaginaris);

                        //asm
                        stopWatch.Start();
                        array_of_threads[i] = new Thread(unused => runMandelbrotDllAsm(bitMapPixelValues, subTabWithRealis, subTabWithImaginaris, subTabBeginPoint, sizeOfSubTable, iteratorInput, offsetAsm));
                        array_of_threads[i].Start();
                    }
                    //Thread.Sleep(1000);

                    //if(i==0)
                    // {
                    //checkPartioningOfTheBitmap(partOfBitmap,"",i);
                    //  }
                    //partOfBitmap.Dispose();
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

                progressBar.Increment(progressBarOffset);
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
                progressBar.Increment(progressBarOffset);
                TimeSpan time_period = stopWatch.Elapsed;
                string elapsedTime = time_period.TotalMilliseconds.ToString();
                Console.WriteLine("Time of executing dll library in miliseconds {0} for dll library in language: {1}", elapsedTime, (isAsmOrCpp ? "cpp" : "asm"));

                timeLabel.Text = isAsmOrCpp ? "Time of execution the algorithm from cpp dll: " + elapsedTime + " ms" : "Time of execution the algorithm from asm dll: " + elapsedTime + " ms";
                /*for (int i = 0; i < bitMapPixelValues.Length; i++)
                 {
                     Console.Write("{0}: {1}, ",i,bitMapPixelValues[i]);
                 }*/

                saveBitmapToFile(bitMapPixelValues, outputFilePath);
                progressBar.Increment(progressBar.Maximum - progressBar.Value);
            }

        }
    }
}
