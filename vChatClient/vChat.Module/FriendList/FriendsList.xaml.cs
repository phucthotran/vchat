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
using vChat.Model;
using System.ComponentModel;
using Core.Client;
using System.Threading;
using vChat.Module.AddFriend;
using MahApps.Metro.Controls;
using System.Windows.Threading;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Interaction logic for FriendsList.xaml
    /// </summary>
    public partial class FriendsList : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void MouseEventHandler(object sender, FriendArgs e);
        public event MouseEventHandler OnFriendDoubleClick;

        #region CLASS MEMBER

        private AddFriend.AddFriend addFriendModule;
        private MetroWindow addFriendWin;
        private RemoveGroup.RemoveGroup removeGroupModule;
        private MetroWindow removeGroupWin;
        private DispatcherTimer updateRequest;
        private GroupTreeViewModel groupTree;
        private RequestViewModel requestVM;
        private GroupFriendList groupFriend;
        private String requestNewGroupName;
        private String moveNewGroupName;
        private int userId;

        #endregion

        #region PROPERTY

        public GroupTreeViewModel GroupTree
        {
            get { return groupTree; }
            set
            {
                if (value != groupTree)
                {
                    groupTree = value;
                    this.OnPropertyChanged("GroupTree");
                }
            }
        }

        public RequestViewModel RequestVM
        {
            get { return requestVM; }
            set
            {
                if (value != requestVM)
                {
                    requestVM = value;
                    this.OnPropertyChanged("RequestVM");
                }
            }
        }

        public GroupFriendList GroupFriend
        {
            get { return groupFriend; }
            set
            {
                if (value != groupFriend)
                {
                    groupFriend = value;
                    this.OnPropertyChanged("GroupFriend");
                }
            }
        }

        public String RequestNewGroupName
        {
            get { return requestNewGroupName; }
            set
            {
                if (value != requestNewGroupName)
                {
                    requestNewGroupName = value;
                    this.OnPropertyChanged("RequestNewGroupName");
                }
            }
        }

        public String MoveNewGroupName
        {
            get { return moveNewGroupName; }
            set
            {
                if (value != moveNewGroupName)
                {
                    moveNewGroupName = value;
                    this.OnPropertyChanged("MoveNewGroupName");
                }
            }
        }

        #endregion

        public FriendsList()
        {
            InitializeComponent();
        }        

        #region MAIN METHOD

        public void Init(int UserID)
        {
            userId = UserID;

            groupFriend = FriendList(userId);
            List<Users> Requests = FriendRequests(userId);

            changeAvatarModule.ChangeAvatarFor(userId);

            addFriendModule = new AddFriend.AddFriend();
            addFriendModule.SetUser(userId);
            addFriendModule.SetGroups(GroupFriend.FriendGroups);
            addFriendModule.OnAddFriendSuccess += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendSuccess);
            addFriendModule.OnAddFriendError += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendError);

            removeGroupModule = new Module.RemoveGroup.RemoveGroup();
            removeGroupModule.SetUser(userId);
            removeGroupModule.SetGroups(GroupFriend.FriendGroups);
            removeGroupModule.IntegratedWith(this);

            groupTree = new GroupTreeViewModel(GroupFriend.FriendGroups);
            groupTree.OnMoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnMoveContact);
            groupTree.OnRemoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnRemoveContact);

            requestVM = new RequestViewModel(Requests);
            requestVM.OnAcceptRequest += new RequestViewModel.RequestHandler(RequestVM_OnAcceptRequest);
            requestVM.OnIgnoreRequest += new RequestViewModel.RequestHandler(RequestVM_OnIgnoreRequest);

            //Request online/offline status of user
            foreach (FriendGroup group in GroupFriend.FriendGroups)
                foreach (Users friend in group.Friends)
                    this.Get<Client>().SendCommand(Core.Data.CommandType.CheckOnline, "SERVER", friend.Username);

            base.DataContext = this;

            updateRequest = new DispatcherTimer();
            updateRequest.Interval = TimeSpan.FromMilliseconds(1000);
            updateRequest.Tick += new EventHandler(UpdateRequest_Tick);
            updateRequest.Start();
        }

        public void SetFriendStatus(String Username, bool IsOnline)
        {
            GroupTree.SetFriendStatus(Username, IsOnline);
        }

        //Call by RemoveGroup module
        public void DoRemoveGroup(bool RemoveContact, FriendGroup GroupToRemove, FriendGroup GroupMoveTo = null)
        {
            if (GroupToRemove.Friends.Count > 0 && RemoveContact)
            {
                RemoveGroup(GroupToRemove.GroupID, RemoveContact);
                GroupTree.RemoveGroup(GroupToRemove);
                treeFriend.UpdateLayout();
                removeGroupWin.Close();

                return;
            }
            else if (GroupToRemove.Friends.Count == 0)
            {
                RemoveGroup(GroupToRemove.GroupID, false);
                GroupTree.RemoveGroup(GroupToRemove);
                treeFriend.UpdateLayout();
                removeGroupWin.Close();

                return;
            }

            foreach (Users child in GroupToRemove.Friends)
            {
                MoveContact(userId, child.UserID, GroupMoveTo.GroupID);
                GroupTree.MoveFriend(child, GroupToRemove, GroupMoveTo);
            }

            GroupTree.RemoveGroup(GroupToRemove);

            treeFriend.UpdateLayout();
            removeGroupWin.Close();
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT PERFORM

        #region ADD FRIEND ZONE

        private void btnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            Helper.CreateWindow(ref addFriendWin, "Thêm Bạn", addFriendModule).ShowDialog();
        }

        private void AddFriendModule_OnAddFriendSuccess(object sender, AddFriendArgs e)
        {
            addFriendWin.Close();
            MessageBox.Show(String.Format("Đã thêm '{0}' vào nhóm {1} thành công. Vui lòng đợi '{0}' hồi đáp yêu cầu kết bạn của bạn", e.FriendName, e.GroupName));
        }

        private void AddFriendModule_OnAddFriendError(object sender, AddFriendArgs e)
        {
            MessageBox.Show(String.Format("Yêu cầu kết bạn với '{0}' không thành công. Vui lòng thử lại sau", e.FriendName));
        }

        #endregion

        #region REQUEST ZONE

        private void UpdateRequest_Tick(object sender, EventArgs e)
        {            
            int totalRequest = RequestVM.Requests.Count;

            requestTaskZone.Visibility = totalRequest == 0 ? Visibility.Collapsed : Visibility.Visible;

            List<Users> Requests = FriendRequests(userId).Skip(totalRequest).ToList(); //Skip amount of "totalRequest" request got before

            if (Requests.Count == 0)
                return;

            foreach (Users Friend in Requests)
                RequestVM.AppendRequest(Friend);

            friendRequestZone.UpdateLayout();
        }

        private void RequestVM_OnAcceptRequest(Users Friend)
        {
            FriendGroup AvailableGroup = cbRequestGroup.SelectedItem as FriendGroup;

            int NewGroupID = 0;

            if (AvailableGroup == null || RequestNewGroupName != null)
                if (RequestNewGroupName != null)
                    if (!AddNewGroup(userId, RequestNewGroupName, ref NewGroupID))
                        MessageBox.Show("Thêm nhóm mới không thành công");

            AcceptRequest(userId, Friend.UserID, NewGroupID != 0 ? GetGroup(NewGroupID).GroupID : AvailableGroup.GroupID);

            GroupTree.AppendFriend(Friend, NewGroupID != 0 ? GetGroup(NewGroupID) : AvailableGroup);

            RequestVM.RemoveRequest(Friend);

            friendRequestZone.UpdateLayout();
            treeFriend.UpdateLayout();
        }

        private void RequestVM_OnIgnoreRequest(Users Friend)
        {
            IgnoreRequest(userId, Friend.UserID);
            RequestVM.RemoveRequest(Friend);

            friendRequestZone.UpdateLayout();
        }

        private void chkRequestTaskDone_Checked(object sender, RoutedEventArgs e)
        {
            btnRequestTaskDone.IsEnabled = true;
        }

        private void chkRequestTaskDone_Unchecked(object sender, RoutedEventArgs e)
        {
            btnRequestTaskDone.IsEnabled = false;
        }

        private void btnRequestTaskDone_Click(object sender, RoutedEventArgs e)
        {
            String Tag = null;

            if (cbRequestTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbRequestTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "Accept":
                    requestVM.AcceptCommand.Execute(null);
                    break;
                case "Ignore":
                    requestVM.IgnoreCommand.Execute(null);
                    break;
            }

            chkRequestTaskDone.IsChecked = false;
        }

        #endregion

        #region GROUP TREE ZONE

        private void TreeFriend_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Object SelectedObj = (Object)treeFriend.SelectedItem;

            foreach (Object parentObj in treeFriend.Items)
            {
                TreeViewItem Group = treeFriend.ItemContainerGenerator.ContainerFromItem(parentObj) as TreeViewItem;
                TreeViewItem MatchItem = treeFriend.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem;

                if (MatchItem != null)
                {
                    MatchItem.ContextMenu = treeFriend.Resources["GroupContext"] as ContextMenu;
                    break;
                }

                TreeViewItem Friend = Group.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem;
                if (Friend != null)
                    Friend.ContextMenu = treeFriend.Resources["FriendContext"] as ContextMenu;
            }
        }

        private void TreeFriend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem DBClickItem = e.Source as TreeViewItem;

            if (DBClickItem == null)
                return;

            object Item = DBClickItem.Header;

            if (Item is FriendViewModel)
            {
                FriendViewModel SelectedFriend = treeFriend.SelectedItem as FriendViewModel;
                Users FriendArg = SelectedFriend.Friend;

                OnFriendDoubleClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GroupTree.SearchCommand.Execute(null);
        }

        private void mnuAddFriend_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = treeFriend.SelectedItem as GroupViewModel;

            addFriendModule.SetDefaultGroup(SelectedGroup.Group);

            Helper.CreateWindow(ref addFriendWin, "Thêm Bạn", addFriendModule).ShowDialog();
        }

        private void mnuRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = treeFriend.SelectedItem as GroupViewModel;

            removeGroupModule.SetGroupToRemove(SelectedGroup.Group);

            Helper.CreateWindow(ref removeGroupWin, "Xóa Nhóm", removeGroupModule).ShowDialog();
        }

        private void mnuFriendDetail_Click(object sender, RoutedEventArgs e)
        {
            FriendViewModel SelectedFriend = treeFriend.SelectedItem as FriendViewModel;
        }

        private void GroupTree_OnMoveContact(Users Friend, FriendGroup OldGroup)
        {
            FriendGroup NewGroup = cbNewGroup.SelectedItem as FriendGroup;

            int NewGroupID = 0;

            if (NewGroup == null || MoveNewGroupName != null)
                if (MoveNewGroupName != null)
                    if (!AddNewGroup(userId, MoveNewGroupName, ref NewGroupID))
                        MessageBox.Show("Thêm nhóm mới không thành công");

            MoveContact(userId, Friend.UserID, NewGroupID != 0 ? GetGroup(NewGroupID).GroupID : NewGroup.GroupID);

            GroupTree.MoveFriend(Friend, OldGroup, NewGroupID != 0 ? GetGroup(NewGroupID) : NewGroup);

            treeFriend.UpdateLayout();
        }

        private void GroupTree_OnRemoveContact(Users Friend, FriendGroup OldGroup)
        {
            RemoveContact(userId, Friend.UserID);

            GroupTree.RemoveFriend(Friend, OldGroup);

            treeFriend.UpdateLayout();
        }

        #endregion

        #region EDIT TASK ZONE

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {            
            btnSelectAll.Visibility = System.Windows.Visibility.Visible;
            btnDeselectAll.Visibility = System.Windows.Visibility.Visible;
            editTaskZone.Visibility = System.Windows.Visibility.Visible;
        }

        private void chkDone_Checked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = true;
        }

        private void chkDone_Unchecked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = false;
        }

        private void cbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String Tag = null;

            if (cbTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "MoveContact":
                    tblNewGroup.Visibility = System.Windows.Visibility.Visible;
                    cbNewGroup.Visibility = System.Windows.Visibility.Visible;
                    tblAddNewGroup.Visibility = System.Windows.Visibility.Visible;
                    tbAddNewGroup.Visibility = System.Windows.Visibility.Visible;
                    break;

                case "RemoveContact":
                    tblNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    cbNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    tblAddNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    tbAddNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            String Tag = null;

            if (cbTask.SelectedItem != null)
                Tag = ((ComboBoxItem)cbTask.SelectedItem).Tag.ToString();

            switch (Tag)
            {
                case "MoveContact":
                    GroupTree.MoveCommand.Execute(null);
                    break;

                case "RemoveContact":
                    GroupTree.RemoveCommand.Execute(null);
                    break;
            }

            chkDone.IsChecked = false;
            btnDone.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GroupTree.CancelEditCommand.Execute(null);
            btnSelectAll.Visibility = System.Windows.Visibility.Collapsed;
            btnDeselectAll.Visibility = System.Windows.Visibility.Collapsed;
            editTaskZone.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #endregion

        
    }    
}
