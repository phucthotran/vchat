using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using Core.Data;
using System.Net;
using Core.Client;
using vChat.Lib;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Các thông tin dành cho Binding bạn bè trên TreeView
    /// </summary>
    public class FriendViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private readonly GroupViewModel parent;
        private readonly Users friend;

        private bool isSelected;
        private bool isChecked;
        private bool isOnline;
        private Visibility toogleCheckbox = Visibility.Collapsed; //Default
        private Brush matchColor = Brushes.Black; //Default (Not yet search)

        #endregion

        #region PROPERTY

        /// <summary>
        /// Nhóm cha
        /// </summary>
        public GroupViewModel Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Thông tin bạn bè
        /// </summary>
        public Users Friend
        {
            get { return friend; }
        }

        /// <summary>
        /// Tên bạn bè
        /// </summary>
        public String FriendName
        {
            get { return (String.Format("{0} {1}", friend.FirstName, friend.LastName)); }
        }

        /// <summary>
        /// Ảnh avatar
        /// </summary>
        public ImageSource Avatar
        {
            get
            {
                if (friend.Picture == null)
                    return null;                                
               
                return ImageByteConverter.GetFromBytes(friend.Picture);                
            }
        }

        /// <summary>
        /// Trạng thái được chọn hay không (Dựa trên thuộc tính IsSelected của TreeViewItem)
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Trạng thái được đánh dấu chọn hay không
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;

                    //Nếu tất cả bạn bè đã được đánh dấu chọn thì nhóm cũng sẽ được đánh dấu chọn
                    int TotalChild = parent.Children.Count;
                    int TotalChecked = parent.Children.Where(p => p.IsChecked == value).Count();

                    if (TotalChecked == TotalChild)
                        parent.IsChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// Trạng thái online/offline của bạn bè
        /// </summary>
        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                if (isOnline != value)
                {
                    isOnline = value;
                    this.OnPropertyChanged("IsOnline");
                }
            }
        }

        /// <summary>
        /// Trạng thái ẩn/hiện của Checkbox (Bên tay trái của Avatar)
        /// </summary>
        public Visibility ToogleCheckbox
        {
            get { return toogleCheckbox; }
            set
            {
                if (value != toogleCheckbox)
                {
                    toogleCheckbox = value;
                    this.OnPropertyChanged("ToogleCheckbox");
                }
            }
        }

        /// <summary>
        /// Màu chữ của tên bạn bè (trước/sau khi tìm kiếm). Mặc định là màu đen
        /// </summary>
        public Brush MatchColor
        {
            get { return matchColor; }
            set
            {
                if (value != matchColor)
                {
                    matchColor = value;
                    this.OnPropertyChanged("MatchColor");
                }
            }
        }

        #endregion

        /// <summary>
        /// Khởi tạo bạn bè dùng cho Binding
        /// </summary>
        /// <param name="Friend">Đối tượng chứa thông tin bạn bè</param>
        /// <param name="Parent">Đối tượng chứa thông tin nhóm mà bạn bè thuộc vào</param>
        public FriendViewModel(Users Friend, GroupViewModel Parent)
        {
            friend = Friend;
            parent = Parent;
        }

        #region MAIN METHOD

        /// <summary>
        /// Kiểm tra tên của bạn bè có khớp với từ khóa tìm kiếm hay không (Phục vụ cho việc tìm kiếm bạn bè)
        /// </summary>
        /// <param name="Text">Từ khóa</param>
        /// <returns></returns>
        public bool NameContainsText(String Text)
        {
            if (String.IsNullOrWhiteSpace(FriendName) && String.IsNullOrWhiteSpace(Text))
                return false;

            return FriendName.IndexOf(Text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
