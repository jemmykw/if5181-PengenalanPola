using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTS_PengenalanPola.Pages
{
    class Thining 
    {
        /*
        public static bool[][] ImageCheckToBool(string filename)
        {
            
            Bitmap img = new Bitmap(filename);
            if (img.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new FormatException("The image must be in the 24bpp format.");
            }
            int[] array = new int[img.Width * img.Height - 1];
            BitmapData bmp = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bool[][] s = new bool[bmp.Height][];

            unsafe
            {
                int PixelSize = 3;
                int count = 0;

                for (int y = 0; y < bmp.Height; y++)
                {
                    byte* row = (byte*)bmp.Scan0 + (y * bmp.Stride);
                    s[y] = new bool[bmp.Width];
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int r = (int)row[(x * PixelSize) + 2];
                        int g = (int)row[(x * PixelSize) + 1];
                        int b = (int)row[(x * PixelSize)];

                        float[] hsb = RGBtoHSB(r, g, b, null);
                        s[y][x] = hsb[2] < 0.3;
                    }
                }
            }
            img.UnlockBits(bmp);
            img.Dispose();
            GC.SuppressFinalize(bmp);

            return s;
        }
        */
    }
}
