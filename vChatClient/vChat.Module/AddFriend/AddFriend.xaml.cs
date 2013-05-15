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
using vChat.Model.Entities;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model;

namespace vChat.Module.AddFriend
{
    /// <summary>
    /// Interaction logic for AddFriend.xaml
    /// </summary>
    public partial class AddFriend : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void AddingHandler(object sender, AddFriendArgs e);
        public event AddingHandler OnAddFriendSuccess;
        public event AddingHandler OnAddFriendError;

        #region CLASS MEMBER

        private int userId;
        private ObservableCollection<FriendGroup> groups;
        private String friendName;
        private String newGroupName;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Lấy ra danh sách nhóm
        /// </summary>
        public ObservableCollection<FriendGroup> Groups
        {
            get { return groups; }
        }

        /// <summary>
        /// Lấy/gán tên bạn bè
        /// </summary>
        public String FriendName
        {
            get { return friendName; }
            set
            {
                if (value != friendName)
                {
                    friendName = value;
                    this.OnPropertyChanged("FriendName");
                }
            }
        }

        /// <summary>
        /// Lấy/gán tên của nhóm mới
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

        #endregion

        public AddFriend()
        {
            InitializeComponent();
        }

        #region MAIN METHOD

        /// <summary>
        /// Cài đặt cho user thực hiện thêm bạn
        /// </summary>
        /// <param name="UserID">ID của user cần thêm bạn</param>
        public void SetUser(int UserID)
        {
            userId = UserID;
        }

        /// <summary>
        /// Cài đặt danh sách nhóm
        /// </summary>
        /// <param name="Groups"></param>
        public void SetGroups(ObservableCollection<FriendGroup> Groups)
        {
            groups = Groups;

            DataContext = this;
        }

        /// <summary>
        /// Cài đặt nhóm mặc định khi module được nạp. Thường dùng cho thêm bạn vào một nhóm đã chọn trước đó
        /// </summary>
        /// <param name="SelectGroup">Đối tượng chứa thông tin nhóm</param>
        public void SetDefaultGroup(FriendGroup SelectGroup)
        {
            if (SelectGroup == null)
                return;

            cbGroup.SelectedValue = SelectGroup.GroupID;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (FindUser(FriendName) == null)
            {
                MessageBox.Show("Người dùng không tồn tại. Vui lòng kiểm tra lại thông tin");
                return;
            }

            Users user = GetUser(userId);

            if (user.Username.Equals(FriendName))
            {
                MessageBox.Show("Bạn không thể tự thêm chính mình vào danh sách bạn bè");
                return;
            }

            Users friend = FindUser(FriendName);

            if (AlreadyAdded(user.UserID, friend.UserID)) //Kiểm tra trường hợp đã kết bạn trước đó
            {
                MessageBox.Show(String.Format("'{0}' đã được thêm vào danh sách bạn bè của bạn trước đó", FriendName));
                return;
            }

            if (cbGroup.SelectedItem == null && NewGroupName == null) //Nếu nhóm có sẵn không tồn tại và nhóm mới không được nhập thông tin thì...
            {
                MessageBox.Show("Hãy nhập tên nhóm bạn cần thêm bạn bè vào");
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));
                return;
            }

            FriendGroup SelectedGroup = (FriendGroup)cbGroup.SelectedItem; //Lấy ra nhóm được chọn từ combobox
            int NewGroupID = 0;

            if (NewGroupName != null) //Nếu thông tin nhóm mới không bị bỏ trống
                if (!AddNewGroup(userId, NewGroupName, ref NewGroupID)) //Trong trường hợp không tạo nhóm mới thành công thì thông báo lỗi
                    OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));

            if (AddNewFriend(userId, FriendName, NewGroupID != 0 ? NewGroupID : SelectedGroup.GroupID)) //Nếu thông tin nhóm mới bỏ trống thì thực hiện thêm bạn bè vào nhóm có sẵn
                OnAddFriendSuccess(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));            
            else //Thông báo lỗi khi thêm bạn không thành công
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));
        }

        #endregion
    }
}
