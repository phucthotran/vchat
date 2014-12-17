using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows.Controls;
using System.Windows;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Các thông tin dành cho Binding nhóm trên TreeView
    /// </summary>
    public class GroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private readonly ObservableCollection<FriendViewModel> children;
        private readonly FriendGroup group;

        private bool isExpanded;
        private bool isSelected;
        private bool isChecked;
        private Visibility toogleCheckbox = Visibility.Collapsed; //Default

        #endregion

        #region PROPERTY

        /// <summary>
        /// Thông tin nhóm
        /// </summary>
        public FriendGroup Group
        {
            get { return group; }
        }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public String GroupName
        {
            get { return group.Name; }
        }

        /// <summary>
        /// Danh sách bạn bè trong nhóm
        /// </summary>
        public ObservableCollection<FriendViewModel> Children
        {
            get { return children; }
        }

        /// <summary>
        /// Trạng thái đóng/mở của nhóm (Dựa theo thuộc tính IsExpanded của TreeViewItem
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
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

                    //Đánh dấu chọn cho tất cả các bạn bè trong nhóm
                    foreach (FriendViewModel child in children)
                        child.IsChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// Trạng thái ẩn/hiện của Checkbox (Bên tay trái của nhóm)
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

        #endregion

        /// <summary>
        /// Khởi tạo thông tin nhóm dùng cho Binding
        /// </summary>
        /// <param name="Group">Đối tượng chứa thông tin nhóm</param>
        public GroupViewModel(FriendGroup Group)
        {
            group = Group;

            children = new ObservableCollection<FriendViewModel>(
                    (from Friend in @group.Friends
                     select new FriendViewModel(Friend, this))
                     .ToList()
                ); 
        }

        #region MAIN METHOD

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
