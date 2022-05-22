using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcProject.Tool
{
    public class FileHelper
    {
        public static void PlanFileDelete(string filePath, string fileName)
        {
            string file = Path.Combine(filePath, fileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public static Image RefactorImage(Image imgToResize, int maxPixels)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;


            int destWidth = sourceWidth;
            int destHeight = sourceHeight;

            // Resize if needed
            if (sourceWidth > maxPixels || sourceHeight > maxPixels)
            {
                float thePercent = 0;
                float thePercentW = 0;
                float thePercentH = 0;

                thePercentW = maxPixels / (float)sourceWidth;
                thePercentH = maxPixels / (float)sourceHeight;


                if (thePercentH < thePercentW)
                {
                    thePercent = thePercentH;
                }
                else
                {
                    thePercent = thePercentW;
                }


                destWidth = (int)(sourceWidth * thePercent);
                destHeight = (int)(sourceHeight * thePercent);
            }


            Bitmap tmpImage = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(tmpImage);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return tmpImage;

        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}