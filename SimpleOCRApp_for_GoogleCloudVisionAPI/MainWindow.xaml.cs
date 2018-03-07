using Microsoft.Win32;
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
using System.IO;
// add my class
using GoogleCloudVisionAPI;

namespace SimpleOCRApp_for_GoogleCloudVisionAPI
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage loadImage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source != null)
            {
                textBox.Text = "Start OCR Process for Google Cloud Vision API";

                Stream stream = loadImage.StreamSource;

                byte[] imageArray = System.IO.File.ReadAllBytes(loadImage.UriSource.LocalPath);

                GCPVisonAPI ocr = new GCPVisonAPI();

                string getText = "";

                ocr.getTextAndRoi(imageArray, ref getText);
                textBox.Text = getText;
            }
            else
            {
                textBox.Text = "Not Load Image File....";
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Open Image File";
            dialog.Filter = "Image Files (*.png)|*.png;";
            if (dialog.ShowDialog() == true)
            {
                var source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(dialog.FileName);
                source.EndInit();
                loadImage = source;
                image.Source = source;

            }
            else
            {
                // this.textBlockFileName.Text = "キャンセルされました";
            }
        }
    }
}
