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
            //var fileDialog = new System.Windows.Forms.OpenFileDialog();
            //fileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            //fileDialog.FilterIndex = 1;
            //var result = fileDialog.ShowDialog();

            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    var file = fileDialog.FileName;
            //    FileInfo fi = new FileInfo(file);
            //    TxtFile.Text = fi.Extension;

            //    // Open a Uri and decode a BMP image
            //    Uri myUri = new Uri(file, UriKind.RelativeOrAbsolute);
            //    BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //    BitmapSource bitmapSource2 = decoder2.Frames[0];

            //    // Draw the Image
            //    myImage2.Source = bitmapSource2;
            //    myImage2.Stretch = Stretch.Fill;

            BitmapImage originalIamge = new BitmapImage();
            originalIamge.BeginInit();
            originalIamge.UriSource = new Uri("http://270c81.medialib.glogster.com/aidallanos/media/7d/7da3f80f5680169c6520d8bccfa781b9d452325c/ks-color.png");
            originalIamge.DownloadCompleted += delegate {
                BitmapSource bitmapSource = new FormatConvertedBitmap(originalIamge, PixelFormats.Pbgra32, null, 0);
                WriteableBitmap modifiedImage = new WriteableBitmap(bitmapSource);

                int h = modifiedImage.PixelHeight;
                int w = modifiedImage.PixelWidth;
                int[] pixelData = new int[w * h];
                int widthInByte = 4 * w;

                modifiedImage.CopyPixels(pixelData, widthInByte, 0);

                for (int i = 0; i < pixelData.Length; i++)
                {
                    pixelData[i] ^= 0x00ffffff;
                }

                modifiedImage.WritePixels(new Int32Rect(0, 0, w, h), pixelData, widthInByte, 0);

                ModifiedImage.Source = modifiedImage;
            };

            originalIamge.EndInit();
            OriginalImage.Source = originalIamge;


        }

    }
}
