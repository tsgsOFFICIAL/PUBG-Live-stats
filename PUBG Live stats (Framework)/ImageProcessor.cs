using System;
using Tesseract;
using System.Drawing;

namespace PUBG_Live_stats__Framework_
    {
    /// <summary>
    /// ImageProcessor contains all image processing methods and functions
    /// </summary>
    public class ImageProcessor
        {

        /// <summary>
        /// Capture the log region of the screen
        /// </summary>
        /// <returns>The first bitmap, ready for image processing</returns>
        public Bitmap CaptureScreen()
            {
            int _x = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.21875), _y = Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 0.0925925925925926 * 0.23);
            Rectangle rect = new Rectangle(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - _x, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y + Convert.ToInt32(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 0.0925925925925926 * 0.77), _x, _y);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            return bmp;
            }

        /// <summary>
        /// Convert the image to a grayscale version
        /// </summary>
        /// <param name="bmp">Takes the previous bitmap as a parameter</param>
        /// <returns>The final bitmap</returns>
        public Bitmap GrayscaleImage(Bitmap bmp)
            {
            using (Graphics gr = Graphics.FromImage(bmp)) //SourceImage is a Bitmap object
                {
                float[][] gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            };
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(gray_matrix));
                ia.SetThreshold(0.7f); //Change this threshold as needed
                Rectangle rc = new Rectangle(0, 0, bmp.Width, bmp.Height);
                gr.DrawImage(bmp, rc, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
                }

            return bmp;
            }

        /// <summary>
        /// Reads OCR from a passed Bitmap
        /// </summary>
        /// <param name="bmp">This bitmap is the final, processed version</param>
        /// <returns>The OcrResult object</returns>
        public string ReadOCR(Bitmap bmp)
            {
            TesseractEngine Ocr = new TesseractEngine("eng.traineddata", "eng", EngineMode.TesseractAndCube);
            Page page = Ocr.Process(bmp);
            return page.GetText();
            }

        }
    }
