using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace vChat.Module.Upload
{
    /// <summary>
    /// Interaction logic for UploadImage.xaml
    /// </summary>
    public partial class UploadImage : UserControl
    {
        private Avatar.Avatar _IntegratedModule;

        private const int BUFFER_SIZE = 256;
        private byte[] _ImageBytes;
        private OpenFileDialog _OpenDialog;
        private int _UserID;

        public UploadImage()
        {
            InitializeComponent();
            InitOpenDialog();
        }

        #region MAIN METHOD

        private void InitOpenDialog()
        {
            _OpenDialog = new OpenFileDialog();
            _OpenDialog.Title = "Chọn ảnh đại diện";
            _OpenDialog.Filter = "Image (*.PNG, *.BMP, *.GIF, *.JPG, *.JPEG)|*.png;*.bmp;*.gif;*.jpg;*.jpeg";
            _OpenDialog.Multiselect = false;
            _OpenDialog.FileOk += new System.ComponentModel.CancelEventHandler(OpenDialog_FileOk);
        }

        public void UploadFor(int UserID)
        {
            _UserID = UserID;            
        }

        public void IntegratedWith(Avatar.Avatar IntegratedModule)
        {
            this._IntegratedModule = IntegratedModule;
        }

        private ImageSource ResizeImage(byte[] ImageBytes, int sourceWidth, int sourceHeight)
        {
            float resizePercent = ((float)(128 * 100) / (float)sourceWidth);

            int resizeWidth = sourceWidth - (((int)(sourceWidth * (100.0 - resizePercent))) / 100);
            int resizeHeight = sourceHeight - (((int)(sourceHeight * (100.0 - resizePercent))) / 100);

            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.DecodePixelWidth = resizeWidth;
            b.DecodePixelHeight = resizeHeight;
            b.StreamSource = new MemoryStream(ImageBytes);
            b.CreateOptions = BitmapCreateOptions.None;
            b.CacheOption = BitmapCacheOption.Default;
            b.EndInit();

            return b as ImageSource;
        }

        private byte[] ReadImageData(String Path)
        {
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);

            byte[] imgData = new byte[(int)fs.Length];

            br.Read(imgData, 0, (int)fs.Length);

            br.Close();
            fs.Close();

            return imgData;
        }

        private void SaveImageData(byte[] ImageBytes, String Path)
        {
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write);

            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(ImageBytes);

            bw.Close();
            fs.Close();
        }

        private byte[] GetEncodedImageData(ImageSource image, string preferredFormat)
        {
            byte[] result = null;
            BitmapEncoder encoder = null;

            switch (preferredFormat.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                    
                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break; 

                case ".png":
                    encoder = new PngBitmapEncoder();
                    break; 

                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
            } 

            if (image is BitmapSource)
            {
                MemoryStream stream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = new byte[stream.Length];
                BinaryReader br = new BinaryReader(stream);
                br.Read(result, 0, (int)stream.Length);
                br.Close();
                stream.Close();
            }

            return result;
        }

        #endregion

        #region EVENT

        private void OpenDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            String ImagePath = _OpenDialog.FileName;

            //Read image bytes data from file
            _ImageBytes = ReadImageData(ImagePath);

            //Temp display image
            ImageSourceConverter imgConverter = new ImageSourceConverter();
            imgPreview.Source = imgConverter.ConvertFromString(ImagePath) as ImageSource;

            //Resize and re-display image
            imgPreview.Source = ResizeImage(_ImageBytes, (int)imgPreview.Source.Width, (int)imgPreview.Source.Height);

            //Get new image bytes after resize
            _ImageBytes = GetEncodedImageData(imgPreview.Source, ".png");

            btnUpload.IsEnabled = true;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            _OpenDialog.ShowDialog();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            ChangeProfilePicture(_UserID, _ImageBytes);
            _IntegratedModule.ChangeAvatarWork(imgPreview.Source);
        }

        #endregion
    }
}
