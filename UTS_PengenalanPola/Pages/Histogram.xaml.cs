using System;
using System.Collections.Generic;
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

                BitmapImage originalImage = new BitmapImage();
                originalImage.BeginInit();

                originalImage.UriSource = new Uri(fileDialog.FileName);
                PngBitmapDecoder decoder = new PngBitmapDecoder(originalImage.UriSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                BitmapSource bitmapSource = decoder.Frames[0];

                WriteableBitmap modifiedImage = new WriteableBitmap(bitmapSource);

                int stride = bitmapSource.PixelWidth * 4;
                int size = bitmapSource.PixelHeight * stride;
                byte[] pixelData = new byte[size];

                modifiedImage.CopyPixels(pixelData, stride, 0);

                drawOriginalHistogram(pixelData);
                pixelData = equalize(pixelData);
                drawModifiedHistogram(pixelData);

                modifiedImage.WritePixels(new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight), pixelData, stride, 0);

                ModifiedImage.Source = modifiedImage;

                originalImage.EndInit();
                OriginalImage.Source = originalImage;
            }
        }

        private BGRAarray splitColor(byte[] pixelData)
        {
            int[] blueArray = new int[256];
            int[] greenArray = new int[256];
            int[] redArray = new int[256];
            int[] alphaArray = new int[256];

            for (int i = 0; i < pixelData.Length; i++)
            {
                switch (i % 4)
                {
                    case 0: // blue
                        blueArray[pixelData[i]]++;
                        break;
                    case 1: // green
                        greenArray[pixelData[i]]++;
                        break;
                    case 2: // merah
                        redArray[pixelData[i]]++;
                        break;
                    case 3: // alpha
                        alphaArray[pixelData[i]]++;
                        break;
                    default:
                        break;
                }
            }

            BGRAarray bgraArray = new BGRAarray();

            bgraArray.blueArray = blueArray;
            bgraArray.greenArray = greenArray;
            bgraArray.redArray = redArray;
            bgraArray.alphaArray = alphaArray;

            return bgraArray;
        }

        private byte[] equalize(byte[] rawPixels)
        {
            BGRAarray bgraArray = splitColor(rawPixels);
            int[] blueArray = bgraArray.blueArray;
            int[] greenArray = bgraArray.greenArray;
            int[] redArray = bgraArray.redArray;
            int[] alphaArray = bgraArray.alphaArray;

            double pixelLength = (double)alphaArray.Sum();

            byte[] blueALU = new byte[256];
            byte[] greenALU = new byte[256];
            byte[] redALU = new byte[256];
            byte[] alphaALU = new byte[256];

            blueALU[0] = (byte)Math.Round(255 * blueArray[0] / pixelLength);
            greenALU[0] = (byte)Math.Round(255 * greenArray[0] / pixelLength);
            redALU[0] = (byte)Math.Round(255 * redArray[0] / pixelLength);
            alphaALU[0] = (byte)Math.Round(255 * alphaArray[0] / pixelLength);

            for (int i = 1; i < 256; i++)
            {
                blueALU[i] = (byte)(blueALU[i - 1] + Math.Round(255 * blueArray[i] / pixelLength));
                greenALU[i] = (byte)(greenALU[i - 1] + Math.Round(255 * greenArray[i] / pixelLength));
                redALU[i] = (byte)(redALU[i - 1] + Math.Round(255 * redArray[i] / pixelLength));
                alphaALU[i] = (byte)(alphaALU[i - 1] + Math.Round(255 * alphaArray[i] / pixelLength));
            }

            byte[] sPixels = new byte[rawPixels.Length];

            for (int i = 0; i < sPixels.Length; i++)
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

            return sPixels;
        }

        private void drawOriginalHistogram(byte[] pixelData)
        {
            BGRAarray bgraArray = splitColor(pixelData);
            int[] blueArray = bgraArray.blueArray;
            int[] greenArray = bgraArray.greenArray;
            int[] redArray = bgraArray.redArray;
            int[] alphaArray = bgraArray.alphaArray;

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
            p.Y = (double)(OriginalBlueHistogram.Height * (1 - blueArray[0] / blueArrayMax));
            bluePoints.Add(p);
            p.Y = (double)(OriginalGreenHistogram.Height * (1 - greenArray[0] / greenArrayMax));
            greenPoints.Add(p);
            p.Y = (double)(OriginalRedHistogram.Height * (1 - redArray[0] / redArrayMax));
            redPoints.Add(p);
            p.Y = (double)(OriginalAlphaHistogram.Height * (1 - alphaArray[0] / alphaArrayMax));
            alphaPoints.Add(p);

            for (int i = 0; i < 256; i++)
            {
                p.X = (double)(i);
                p.Y = (double)(OriginalBlueHistogram.Height * (1 - blueArray[i] / blueArrayMax));
                bluePoints.Add(p);
                p.Y = (double)(OriginalGreenHistogram.Height * (1 - greenArray[i] / greenArrayMax));
                greenPoints.Add(p);
                p.Y = (double)(OriginalRedHistogram.Height * (1 - redArray[i] / redArrayMax));
                redPoints.Add(p);
                p.Y = (double)(OriginalAlphaHistogram.Height * (1 - alphaArray[i] / alphaArrayMax));
                alphaPoints.Add(p);
            }

            OriginalBlueHistogram.Points = bluePoints;
            OriginalGreenHistogram.Points = greenPoints;
            OriginalRedHistogram.Points = redPoints;
            OriginalAlphaHistogram.Points = alphaPoints;
        }

        private void drawModifiedHistogram(byte[] pixelData)
        {
            BGRAarray bgraArray = splitColor(pixelData);
            int[] blueArray = bgraArray.blueArray;
            int[] greenArray = bgraArray.greenArray;
            int[] redArray = bgraArray.redArray;
            int[] alphaArray = bgraArray.alphaArray;

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
            p.Y = (double)(ModifiedBlueHistogram.Height * (1 - blueArray[0] / blueArrayMax));
            bluePoints.Add(p);
            p.Y = (double)(ModifiedGreenHistogram.Height * (1 - greenArray[0] / greenArrayMax));
            greenPoints.Add(p);
            p.Y = (double)(ModifiedRedHistogram.Height * (1 - redArray[0] / redArrayMax));
            redPoints.Add(p);
            p.Y = (double)(ModifiedAlphaHistogram.Height * (1 - alphaArray[0] / alphaArrayMax));
            alphaPoints.Add(p);

            for (int i = 0; i < 256; i++)
            {
                p.X = (double)(i);
                p.Y = (double)(ModifiedBlueHistogram.Height * (1 - blueArray[i] / blueArrayMax));
                bluePoints.Add(p);
                p.Y = (double)(ModifiedGreenHistogram.Height * (1 - greenArray[i] / greenArrayMax));
                greenPoints.Add(p);
                p.Y = (double)(ModifiedRedHistogram.Height * (1 - redArray[i] / redArrayMax));
                redPoints.Add(p);
                p.Y = (double)(ModifiedAlphaHistogram.Height * (1 - alphaArray[i] / alphaArrayMax));
                alphaPoints.Add(p);
            }

            ModifiedBlueHistogram.Points = bluePoints;
            ModifiedGreenHistogram.Points = greenPoints;
            ModifiedRedHistogram.Points = redPoints;
            ModifiedAlphaHistogram.Points = alphaPoints;
        }
    }
}