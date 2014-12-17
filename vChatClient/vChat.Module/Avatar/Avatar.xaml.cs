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

        /// <summary>
        /// Lấy/gán ảnh
        /// </summary>
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

        /// <summary>
        /// Cài đặt thông tin thay đổi avatar cho user được chỉ định
        /// </summary>
        /// <param name="UserID">ID của user cần thay đổi avatar</param>
        public void ChangeAvatarFor(int UserID)
        {
            uploadAvatarModule = new Upload.UploadImage();
            uploadAvatarModule.UploadFor(UserID);
            uploadAvatarModule.IntegratedWith(this); //Dùng cho việc tương tác với UploadImage module (cập nhập avatar sau khi UploadImage module thực hiện thao tác thành công)

            this.Image = ImageByteConverter.GetFromBytes(GetInfo(UserID).Picture); //Lấy avatar hiện tại của user và hiển thị

            DataContext = this;
        }

        /// <summary>
        /// Thực hiện thay đổi avatar sau khi upload thành công (Thực hiện từ module UploadImage)
        /// </summary>
        /// <param name="Image">Ảnh cần thay đổi</param>
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
