using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UTS_PengenalanPola.Pages
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : UserControl
    {
        public Histogram()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Image Files (*.png)|*.png";
            fileDialog.FilterIndex = 1;
            var result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {

                BitmapImage originalIamge = new BitmapImage();
                originalIamge.BeginInit();
                originalIamge.UriSource = new Uri(fileDialog.FileName, UriKind.RelativeOrAbsolute);
                PngBitmapDecoder decoder2 = new PngBitmapDecoder(originalIamge.UriSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder2.Frames[0];
                WriteableBitmap modifiedImage = new WriteableBitmap(bitmapSource);

                int stride = bitmapSource.PixelWidth * 4;
                int size = bitmapSource.PixelHeight * stride;
                byte[] rawPixels = new byte[size];
                bitmapSource.CopyPixels(rawPixels, stride, 0);
                
                for (int i = 0; i < rawPixels.Length; i++)
                {
                    OriginalPixel.Text += "\n" + i + " = " + rawPixels[i];
                }

                byte[] sPixels = transform(rawPixels);

                for (int i = 0; i < sPixels.Length; i++)
                {
                    ModifiedPixel.Text += "\n" + i + " = " + sPixels[i];
                }

                modifiedImage.WritePixels(new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight), sPixels, stride, 0);

                ModifiedImage.Source = modifiedImage;

                originalIamge.EndInit();
                OriginalImage.Source = originalIamge;
            }

        }

        private byte[] transform(byte[] rawPixels)
        {
            int ncol = 256;
            int rawPixelsLength = rawPixels.Length;
            double eachColLength = (double)rawPixelsLength / 4;

            int[] blueArray = new int[ncol];
            int[] greenArray = new int[ncol];
            int[] redArray = new int[ncol];
            int[] alphaArray = new int[ncol];

            for (int i = 0; i < rawPixelsLength; i++)
            {
                switch (i % 4)
                {
                    case 0: // blue
                        blueArray[rawPixels[i]]++;
                        break;
                    case 1: // green
                        greenArray[rawPixels[i]]++;
                        break;
                    case 2: // merah
                        redArray[rawPixels[i]]++;
                        break;
                    case 3: // alpha
                        alphaArray[rawPixels[i]]++;
                        break;
                    default:
                        break;
                }
            }

            PointCollection bluePoints = new PointCollection();
            PointCollection greenPoints = new PointCollection();
            PointCollection redPoints = new PointCollection();
            PointCollection alphaPoints = new PointCollection();

            Point p = new Point();

            double blueArrayMax = (double)blueArray.Max();
            double greenArrayMax = (double)greenArray.Max();
            double redArrayMax = (double)redArray.Max();
            double alphaArrayMax = (double)alphaArray.Max();

            p.X = (double)(0);
            p.Y = (double)(BlueHistogram.Height * (1 - blueArray[0] / blueArrayMax));
            bluePoints.Add(p);
            p.Y = (double)(GreenHistogram.Height * (1 - greenArray[0] / greenArrayMax));
            greenPoints.Add(p);
            p.Y = (double)(RedHistogram.Height * (1 - redArray[0] / redArrayMax));
            redPoints.Add(p);
            p.Y = (double)(AlphaHistogram.Height * (1 - alphaArray[0] / alphaArrayMax));
            alphaPoints.Add(p);

            double[] blueAccumulator = new double[ncol];
            double[] greenAccumulator = new double[ncol];
            double[] redAccumulator = new double[ncol];
            double[] alphaAccumulator = new double[ncol];

            byte[] blueALU = new byte[ncol];
            byte[] greenALU = new byte[ncol];
            byte[] redALU = new byte[ncol];
            byte[] alphaALU = new byte[ncol];

            blueAccumulator[0] = blueArray[0] / eachColLength;
            greenAccumulator[0] = greenArray[0] / eachColLength;
            redAccumulator[0] = redArray[0] / eachColLength;
            alphaAccumulator[0] = alphaArray[0] / eachColLength;

            blueALU[0] = (byte)Math.Round((decimal)((ncol - 1) * blueAccumulator[0]));
            greenALU[0] = (byte)Math.Round((decimal)((ncol - 1) * greenAccumulator[0]));
            redALU[0] = (byte)Math.Round((decimal)((ncol - 1) * redAccumulator[0]));
            alphaALU[0] = (byte)Math.Round((decimal)((ncol - 1) * alphaAccumulator[0]));

            for (int i = 1; i < ncol; i++)
            {
                p.X = (double)(i);
                p.Y = (double)(BlueHistogram.Height * (1 - blueArray[i] / blueArrayMax));
                bluePoints.Add(p);
                p.Y = (double)(GreenHistogram.Height * (1 - greenArray[i] / greenArrayMax));
                greenPoints.Add(p);
                p.Y = (double)(RedHistogram.Height * (1 - redArray[i] / redArrayMax));
                redPoints.Add(p);
                p.Y = (double)(AlphaHistogram.Height * (1 - alphaArray[i] / alphaArrayMax));
                alphaPoints.Add(p);

                blueAccumulator[i] = blueAccumulator[i - 1] + blueArray[i] / eachColLength;
                greenAccumulator[i] = greenAccumulator[i - 1] + greenArray[i] / eachColLength;
                redAccumulator[i] = redAccumulator[i - 1] + redArray[i] / eachColLength;
                alphaAccumulator[i] = alphaAccumulator[i - 1] + alphaArray[i] / eachColLength;

                blueALU[i] = (byte)Math.Round((decimal)((ncol - 1) * blueAccumulator[i]));
                greenALU[i] = (byte)Math.Round((decimal)((ncol - 1) * greenAccumulator[i]));
                redALU[i] = (byte)Math.Round((decimal)((ncol - 1) * redAccumulator[i]));
                alphaALU[i] = (byte)Math.Round((decimal)((ncol - 1) * alphaAccumulator[i]));
            }

            BlueHistogram.Points = bluePoints;
            GreenHistogram.Points = greenPoints;
            RedHistogram.Points = redPoints;
            AlphaHistogram.Points = alphaPoints;

            byte[] sPixels = new byte[rawPixelsLength];

            for (int i = 0; i < rawPixelsLength; i++)
            {
                switch (i % 4)
                {
                    case 0: // blue
                        sPixels[i] = blueALU[rawPixels[i]];
                        break;
                    case 1: // green
                        sPixels[i] = greenALU[rawPixels[i]];
                        break;
                    case 2: // merah
                        sPixels[i] = redALU[rawPixels[i]];
                        break;
                    case 3: // alpha
                        sPixels[i] = alphaALU[rawPixels[i]];
                        break;
                    default:
                        break;
                }
            }

            // New Histogram

            blueArray = new int[ncol];
            greenArray = new int[ncol];
            redArray = new int[ncol];
            alphaArray = new int[ncol];

            for (int i = 0; i < rawPixelsLength; i++)
            {
                switch (i % 4)
                {
                    case 0: // blue
                        blueArray[sPixels[i]]++;
                        break;
                    case 1: // green
                        greenArray[sPixels[i]]++;
                        break;
                    case 2: // merah
                        redArray[sPixels[i]]++;
                        break;
                    case 3: // alpha
                        alphaArray[sPixels[i]]++;
                        break;
                    default:
                        break;
                }
            }

            bluePoints = new PointCollection();
            greenPoints = new PointCollection();
            redPoints = new PointCollection();
            alphaPoints = new PointCollection();

            blueArrayMax = (double)blueArray.Max();
            greenArrayMax = (double)greenArray.Max();
            redArrayMax = (double)redArray.Max();
            alphaArrayMax = (double)alphaArray.Max();

            for (int i = 0; i < ncol; i++)
            {
                p.X = (double)(i);
                p.Y = (double)(NewBlueHistogram.Height * (1 - blueArray[i] / blueArrayMax));
                bluePoints.Add(p);
                p.Y = (double)(NewGreenHistogram.Height * (1 - greenArray[i] / greenArrayMax));
                greenPoints.Add(p);
                p.Y = (double)(NewRedHistogram.Height * (1 - redArray[i] / redArrayMax));
                redPoints.Add(p);
                p.Y = (double)(NewAlphaHistogram.Height * (1 - alphaArray[i] / alphaArrayMax));
                alphaPoints.Add(p);
            }

            NewBlueHistogram.Points = bluePoints;
            NewGreenHistogram.Points = greenPoints;
            NewRedHistogram.Points = redPoints;
            NewAlphaHistogram.Points = alphaPoints;

            return sPixels;
        }

    }
}
