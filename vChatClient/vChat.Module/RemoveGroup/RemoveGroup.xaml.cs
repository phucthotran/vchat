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
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.ComponentModel;

namespace vChat.Module.RemoveGroup
{
    /// <summary>
    /// Interaction logic for RemoveGroup.xaml
    /// </summary>
    public partial class RemoveGroup : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private FriendList.FriendsList integratedModule;
        private ObservableCollection<FriendGroup> groups;
        private FriendGroup groupToRemove;
        private int userId;
        private String newGroupName;
        private bool isRemoveContact;
        private bool isMoveContact = true; //Default

        #endregion

        #region PROPERTY

        /// <summary>
        /// Lấy/gán danh sách nhóm
        /// </summary>
        public ObservableCollection<FriendGroup> Groups
        {
            get { return groups; }
        }

        /// <summary>
        /// Lấy/gán tên nhóm mới
        /// </summary>
        public String NewGroupName
        {
            get { return newGroupName; }
            set
            {
                if (value != newGroupName)
                {
                    newGroupName = value;
                    this.OnPropertyChanged("NewGroupName");
                }
            }
        }

        /// <summary>
        /// Lấy/gán trạng thái người người dùng muốn xóa tất cả bạn bè hay không
        /// </summary>
        public bool IsRemoveContact
        {
            get { return isRemoveContact; }
            set
            {
                if (value != isRemoveContact)
                {
                    isRemoveContact = value;
                    this.OnPropertyChanged("IsRemoveContact");
                }
            }
        }

        /// <summary>
        /// Lấy/gán trạng thái người dùng muốn di chuyển bạn bè sang nhóm mới hay không
        /// </summary>
        public bool IsMoveContact
        {
            get { return isMoveContact; }
            set
            {
                if (value != isMoveContact)
                {
                    isMoveContact = value;
                    this.OnPropertyChanged("IsMoveContact");
                }
            }
        }

        #endregion

        public RemoveGroup()
        {
            InitializeComponent();
        }

        #region MAIN METHOD

        /// <summary>
        /// Cài đặt thông tin user cần thao tác
        /// </summary>
        /// <param name="UserID">ID của user</param>
        public void SetUser(int UserID)
        {
            userId = UserID;
        }

        /// <summary>
        /// Cài đặt danh sách nhóm
        /// </summary>
        /// <param name="Groups">Đối tượng chứa danh sách nhóm</param>
        public void SetGroups(ObservableCollection<FriendGroup> Groups)
        {
            groups = Groups;

            DataContext = this;
        }

        /// <summary>
        /// Cài đặt nhóm cần xóa
        /// </summary>
        /// <param name="GroupToRemove">Đối tượng chứa thông tin nhóm</param>
        public void SetGroupToRemove(FriendGroup GroupToRemove)
        {
            groupToRemove = GroupToRemove;            
        }

        /// <summary>
        /// Cài đặt cho việc tương tác với FriendList module (Sau khi xóa nhóm thì sẽ thực hiện cập nhập lại danh sách bạn bè trên FriendList)
        /// </summary>
        /// <param name="IntegratedModule">Đối tượng module FriendList</param>
        public void IntegratedWith(FriendList.FriendsList IntegratedModule)
        {
            integratedModule = IntegratedModule;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            FriendGroup GroupMoveTo = cbGroupMoveTo.SelectedItem as FriendGroup;  //Lấy nhóm sẽ chuyển bạn bè đến

            if (isRemoveContact && (GroupMoveTo != null && GroupMoveTo.Equals(groupToRemove))) //Kiểm tra trường hợp user muốn chuyển liên lạc. Tuy nhiên, nhóm chuyển tới trùng với nhóm user muốn xóa
            {
                MessageBox.Show("Nhóm bị xóa trùng với nhóm chuyển liên lạc đến. Vui lòng chọn nhóm khác");
                return;
            }

            int NewGroupID = 0;

            if (NewGroupName != null) //Nếu trường hợp user đã điền thông tin nhóm mới thì thực hiện thêm nhóm
                if (!AddNewGroup(userId, NewGroupName, ref NewGroupID)) //Trường hợp thêm nhóm không thành công sẽ báo lỗi
                    MessageBox.Show("Thêm nhóm mới không thành công");

            if (isMoveContact) //Trường hợp di chuyển bạn bè tới nhóm mới: nếu đã điền thông tin nhóm mới thì sẽ di chuyển sang nhóm mới, ngược lại di chuyển sang nhóm có sẵn
                integratedModule.DoRemoveGroup(RemoveContact: false, GroupToRemove: groupToRemove, GroupMoveTo: NewGroupID != 0 ? GetGroup(NewGroupID) : GroupMoveTo);
            else if (isRemoveContact) //Trường hợp xóa bạn bè: xóa nhóm và các bạn bè bên trong nhóm đó
                integratedModule.DoRemoveGroup(RemoveContact: true, GroupToRemove: groupToRemove);
        }
    }
}
