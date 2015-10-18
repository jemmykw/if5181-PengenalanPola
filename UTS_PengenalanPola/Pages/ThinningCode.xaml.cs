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
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace UTS_PengenalanPola.Pages
{
    /// <summary>
    /// Interaction logic for ThinningCode.xaml
    /// </summary>
    public partial class ThinningCode : UserControl
    {
        public ThinningCode()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var fileDialog = new System.Windows.Forms.OpenFileDialog();
            //fileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            //fileDialog.FilterIndex = 1;
            //var result = fileDialog.ShowDialog();
            //switch (result)
            //{
            //    case System.Windows.Forms.DialogResult.OK:
            //        var file = fileDialog.FileName;
            //        TxtFile.Text = file;
            //        TxtFile.ToolTip = file;
            //        break;
            //    case System.Windows.Forms.DialogResult.Cancel:
            //    default:
            //        TxtFile.Text = null;
            //        TxtFile.ToolTip = null;
            //        break;
            //}


            // Open a Uri and decode a BMP image
            //Uri myUri = new Uri(fileDialog.FileName, UriKind.RelativeOrAbsolute);
            //BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //BitmapSource My_Image = decoder2.Frames[0];

            //codeEmgucv
            //Image<Bgr, byte> My_Image = new Image<Bgr, byte>(fileDialog.FileName);
            //myImage2.Source = My_Image.ToBitmap();
            // Draw the Image
            //Image myImage2 = new Image();
            //myImage2.Source = bitmapSource2;
            //myImage2.Source = My_Image.ToBitmap(); 
            //myImage2.Stretch = Stretch.None;
            //myImage2.Margin = new Thickness(20);

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

                //drawOriginalHistogram(pixelData);
                //pixelData = equalize(pixelData);
                //drawModifiedHistogram(pixelData);

                modifiedImage.WritePixels(new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight), pixelData, stride, 0);

                ModifiedImage.Source = modifiedImage;

                originalImage.EndInit();
                OriginalImage.Source = originalImage;
            }
        }//end method

        private void ImageCheckToBool(byte[] pixelData)
        {
            
        }
    }
}
