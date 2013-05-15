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
using System.ComponentModel;
using vChat.Model.Entities;

namespace vChat.Module.ContactViewer
{
    /// <summary>
    /// Interaction logic for ContactViewer.xaml
    /// </summary>
    public partial class ContactViewer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private ImageSource avatar;
        private String username;
        private String firstName;
        private String lastName;
        private String birthdate;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Lấy/gán ảnh avatar
        /// </summary>
        public ImageSource Avatar
        {
            get { return avatar; }
            set
            {
                if (value != avatar)
                {
                    avatar = value;
                    this.OnPropertyChanged("Avatar");
                }
            }
        }

        /// <summary>
        /// Lấy/gán tên người dùng
        /// </summary>
        public String Username
        {
            get { return username.ToUpper(); }
            set
            {
                if (value != username)
                {
                    username = value;
                    this.OnPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// Lấy/gán tên và tên đệm
        /// </summary>
        public String FirstName
        {
            get { return firstName; }
            set
            {
                if (value != firstName)
                {
                    firstName = value;
                    this.OnPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// Lấy/gán họ
        /// </summary>
        public String LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    this.OnPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Lấy/gán ngày sinh
        /// </summary>
        public String Birthdate
        {
            get { return birthdate; }
            set
            {
                if (value != birthdate)
                {
                    birthdate = value;
                    this.OnPropertyChanged("Birthdate");
                }
            }
        }

        #endregion

        public ContactViewer()
        {
            InitializeComponent();
        }

        #region MAIN METHOD

        /// <summary>
        /// Cài đặt thông tin để xem thông tin user
        /// </summary>
        /// <param name="Friend">Đối tượng chứa thông của user</param>
        public void ViewFor(Users Friend)
        {
            this.Avatar = vChat.Lib.ImageByteConverter.GetFromBytes(Friend.Picture);
            this.Username = Friend.Username;
            this.FirstName = Friend.FirstName;
            this.LastName = Friend.LastName;
            this.Birthdate = Friend.Birthdate.ToString("dd/MM/yyyy");

            DataContext = this;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

    }
}
