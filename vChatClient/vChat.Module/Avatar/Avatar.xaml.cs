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
using MahApps.Metro.Controls;
using System.IO;
using System.ComponentModel;

namespace vChat.Module.Avatar
{
    /// <summary>
    /// Interaction logic for Avatar.xaml
    /// </summary>
    public partial class Avatar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Upload.UploadImage _UploadAvatarModule;
        private ImageSource _Image;

        public ImageSource Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    _Image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }

        public Avatar()
        {
            _UploadAvatarModule = new Upload.UploadImage();
            InitializeComponent();
        }

        public void ChangeAvatarFor(int UserID)
        {                
            this.Image = ConvertByteToImageSource(GetInfo(UserID).Picture);            

            _UploadAvatarModule = new Upload.UploadImage();
            _UploadAvatarModule.UploadFor(UserID);
            _UploadAvatarModule.IntegratedWith(this);

            DataContext = this;
        }

        private ImageSource ConvertByteToImageSource(byte[] data)
        {
            MemoryStream msImage = new MemoryStream(data);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = msImage;
            bitmap.EndInit();

            return bitmap as ImageSource;
        }

        private void CreateUploadWindow()
        {
            MetroWindow uploadWin = new MetroWindow();
            uploadWin.InitTheme();
            uploadWin.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            uploadWin.ResizeMode = ResizeMode.NoResize;
            uploadWin.Content = _UploadAvatarModule;
            uploadWin.Show();
        }

        //Call by UploadImage module
        public void ChangeAvatarWork(ImageSource Image)
        {
            this.Image = Image;
        }

        private void tbChangeAvatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateUploadWindow();
        }

        private void tbChangeAvatar_MouseEnter(object sender, MouseEventArgs e)
        {
            tbChangeAvatar.Cursor = Cursors.Hand;
            tbChangeAvatar.Foreground = Brushes.Blue;            
        }

        private void tbChangeAvatar_MouseLeave(object sender, MouseEventArgs e)
        {
            tbChangeAvatar.Foreground = Brushes.Black;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
