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
            fileDialog.Filter = "Image Files (*.bmp)|*.bmp";
            fileDialog.FilterIndex = 1;
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    TxtFile.Text = file;
                    TxtFile.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    TxtFile.Text = null;
                    TxtFile.ToolTip = null;
                    break;
            }


            // Open a Uri and decode a BMP image
            Uri myUri = new Uri(fileDialog.FileName, UriKind.RelativeOrAbsolute);
            BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            //codeEmgucv

            // Draw the Image
            //Image myImage2 = new Image();
            myImage2.Source = bitmapSource2;
            myImage2.Stretch = Stretch.None;
            myImage2.Margin = new Thickness(20);

        }
    }
}
