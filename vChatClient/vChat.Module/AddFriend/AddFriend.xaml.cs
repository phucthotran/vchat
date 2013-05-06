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

        public ObservableCollection<FriendGroup> Groups
        {
            get { return groups; }
        }

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

        public void SetUser(int UserID)
        {
            userId = UserID;
        }

        public void SetGroups(ObservableCollection<FriendGroup> Groups)
        {
            groups = Groups;

            DataContext = this;
        }

        public void SetDefaultGroup(FriendGroup SelectGroup)
        {
            if (SelectGroup == null)
                return;

            int pos = cbGroup.Items.IndexOf(SelectGroup);
            cbGroup.SelectedIndex = pos;
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
            if (GetUser(userId).Username.Equals(FriendName)) //Prevent from add yourself to friend list
            {
                MessageBox.Show("Bạn không thể tự thêm chính mình vào danh sách bạn bè");
                return;
            }           

            Users user = GetUser(userId);
            Users friend = FindUser(FriendName);

            if (AlreadyAdded(user.UserID, friend.UserID)) //Friend is already added in your friend list
            {
                MessageBox.Show(String.Format("'{0}' đã được thêm vào danh sách bạn bè của bạn trước đó", FriendName));
                return;
            }

            if (cbGroup.SelectedItem == null && NewGroupName == null)
            {
                MessageBox.Show("Hãy nhập tên nhóm bạn cần thêm bạn bè vào");
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));
                return;
            }

            FriendGroup SelectedGroup = (FriendGroup)cbGroup.SelectedItem;
            int NewGroupID = 0;

            if (NewGroupName != null)
                if (!AddNewGroup(userId, NewGroupName, ref NewGroupID))
                    OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName));

            if (AddNewFriend(userId, FriendName, NewGroupID != 0 ? NewGroupID : SelectedGroup.GroupID))
                OnAddFriendSuccess(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));            
            else
                OnAddFriendError(this, new AddFriendArgs(FriendName, NewGroupName != null ? NewGroupName : SelectedGroup.Name));
        }

        #endregion
    }
}
