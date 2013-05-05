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
using vChat.Lib;

namespace vChat.Module.Avatar
{
    /// <summary>
    /// Interaction logic for Avatar.xaml
    /// </summary>
    public partial class Avatar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private Upload.UploadImage uploadAvatarModule;
        private ImageSource image;

        #endregion

        #region PROPERTY

        public ImageSource Image
        {
            get { return image; }
            set
            {
                if (value != image)
                {
                    image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }

        #endregion

        public Avatar()
        {
            uploadAvatarModule = new Upload.UploadImage();
            InitializeComponent();
        }

        #region MAIN METHOD

        public void ChangeAvatarFor(int UserID)
        {
            uploadAvatarModule = new Upload.UploadImage();
            uploadAvatarModule.UploadFor(UserID);
            uploadAvatarModule.IntegratedWith(this);

            this.Image = ImageByteConverter.GetFromBytes(GetInfo(UserID).Picture);

            DataContext = this;
        }

        //Call by UploadImage module
        public void ChangeAvatarWork(ImageSource Image)
        {
            this.Image = Image;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT

        private void tbChangeAvatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MetroWindow uploadWin = null;
            Helper.CreateWindow(ref uploadWin, "Đăng Ảnh Đại Diện", uploadAvatarModule).ShowDialog();
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

        #endregion

    }
}
