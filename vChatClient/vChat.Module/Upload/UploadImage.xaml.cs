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
using vChat.Lib;

namespace vChat.Module.Upload
{
    /// <summary>
    /// Interaction logic for UploadImage.xaml
    /// </summary>
    public partial class UploadImage : UserControl
    {

        #region CLASS MEMBER

        private Avatar.Avatar integratedModule;

        private byte[] imageBytes;
        private const int AVATAR_WIDTH = 128;
        private OpenFileDialog openDialog;
        private const String IMAGE_FILTER = "Image (*.PNG, *.BMP, *.GIF, *.JPG, *.JPEG)|*.png;*.bmp;*.gif;*.jpg;*.jpeg";
        private int userId;

        #endregion

        public UploadImage()
        {
            InitializeComponent();
            InitOpenDialog();
        }

        #region MAIN METHOD

        private void InitOpenDialog()
        {
            openDialog = new OpenFileDialog();
            openDialog.Title = "Chọn ảnh đại diện";
            openDialog.Filter = IMAGE_FILTER;
            openDialog.Multiselect = false;
            openDialog.FileOk += new System.ComponentModel.CancelEventHandler(OpenDialog_FileOk);
        }

        public void UploadFor(int UserID)
        {
            userId = UserID;            
        }

        public void IntegratedWith(Avatar.Avatar IntegratedModule)
        {
            this.integratedModule = IntegratedModule;
        }

        #endregion

        #region EVENT

        private void OpenDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            String ImagePath = openDialog.FileName;

            //Read image bytes data from file
            imageBytes = ImageByteConverter.GetFromFile(ImagePath);

            //Temp display image
            ImageSourceConverter imgConverter = new ImageSourceConverter();
            imgPreview.Source = imgConverter.ConvertFromString(ImagePath) as ImageSource;

            //Resize and re-display image
            imgPreview.Source = ImageByteConverter.ResizeImageByWidth(imageBytes, (int)imgPreview.Source.Width, (int)imgPreview.Source.Height, AVATAR_WIDTH);

            //Get new image bytes after resize
            imageBytes = ImageByteConverter.GetEncodedImageData(imgPreview.Source, ".png");

            btnUpload.IsEnabled = true;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            openDialog.ShowDialog();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            ChangeProfilePicture(userId, imageBytes);
            integratedModule.ChangeAvatarWork(imgPreview.Source);
        }

        #endregion
    }
}
