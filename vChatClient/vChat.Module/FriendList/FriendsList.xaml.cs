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

        private AddFriend.AddFriend _AddFriendModule;
        private MetroWindow _AddFriendWin;
        private RemoveGroup.RemoveGroup _RemoveGroupModule;
        private MetroWindow _RemoveGroupWin;
        private DispatcherTimer _UpdateRequest;
        private GroupTreeViewModel _GroupTree;
        private RequestViewModel _RequestVM;
        private GroupFriendList _GroupFriend;
        private String _RequestNewGroupName;
        private String _MoveNewGroupName;
        private int _UserID;

        #endregion

        public GroupTreeViewModel GroupTree
        {
            get { return _GroupTree; }
            set
            {
                if (value != _GroupTree)
                {
                    _GroupTree = value;
                    this.OnPropertyChanged("GroupTree");
                }
            }
        }

        public RequestViewModel RequestVM
        {
            get { return _RequestVM; }
            set
            {
                if (value != _RequestVM)
                {
                    _RequestVM = value;
                    this.OnPropertyChanged("RequestVM");
                }
            }
        }

        public GroupFriendList GroupFriend
        {
            get { return _GroupFriend; }
            set
            {
                if (value != _GroupFriend)
                {
                    _GroupFriend = value;
                    this.OnPropertyChanged("GroupFriend");
                }
            }
        }

        public String RequestNewGroupName
        {
            get { return _RequestNewGroupName; }
            set
            {
                if (value != _RequestNewGroupName)
                {
                    _RequestNewGroupName = value;
                    this.OnPropertyChanged("RequestNewGroupName");
                }
            }
        }

        public String MoveNewGroupName
        {
            get { return _MoveNewGroupName; }
            set
            {
                if (value != _MoveNewGroupName)
                {
                    _MoveNewGroupName = value;
                    this.OnPropertyChanged("MoveNewGroupName");
                }
            }
        }

        public FriendsList()
        {
            InitializeComponent();
        }        

        #region MAIN METHOD

        public void Init(int UserID)
        {
            _UserID = UserID;

            _GroupFriend = FriendList(UserID);
            List<Users> Requests = FriendRequests(UserID);

            ChangeAvatarModule.ChangeAvatarFor(UserID);

            _AddFriendModule = new AddFriend.AddFriend();
            _AddFriendModule.SetUser(UserID);
            _AddFriendModule.SetGroups(_GroupFriend.FriendGroups);
            _AddFriendModule.IntegratedWith(this);
            _AddFriendModule.OnAddFriendSuccess += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendSuccess);
            _AddFriendModule.OnAddFriendError += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendError);

            _RemoveGroupModule = new Module.RemoveGroup.RemoveGroup();
            _RemoveGroupModule.SetUser(UserID);
            _RemoveGroupModule.SetGroups(_GroupFriend.FriendGroups);
            _RemoveGroupModule.IntegratedWith(this);

            _GroupTree = new GroupTreeViewModel(_GroupFriend.FriendGroups);
            _GroupTree.OnMoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnMoveContact);
            _GroupTree.OnRemoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnRemoveContact);

            _RequestVM = new RequestViewModel(Requests);
            _RequestVM.OnAcceptRequest += new RequestViewModel.RequestHandler(RequestVM_OnAcceptRequest);
            _RequestVM.OnIgnoreRequest += new RequestViewModel.RequestHandler(RequestVM_OnIgnoreRequest);

            base.DataContext = this;
            //friendRequestZone.DataContext = _RequestVM;
            //cbRequestGroup.ItemsSource = _GroupFriend.FriendGroups;
            //cbNewGroup.ItemsSource = _GroupFriend.FriendGroups;

            _UpdateRequest = new DispatcherTimer();
            _UpdateRequest.Interval = TimeSpan.FromMilliseconds(1000);
            _UpdateRequest.Tick += new EventHandler(UpdateRequest_Tick);
            _UpdateRequest.Start();
        }                

        public void DoRemoveGroup(bool RemoveContact, FriendGroup GroupToRemove, FriendGroup GroupMoveTo = null)
        {
            if (GroupToRemove.Friends.Count > 0 && RemoveContact)
            {
                RemoveGroup(GroupToRemove.GroupID, RemoveContact);
                GroupTree.RemoveGroup(GroupToRemove);
                TreeFriend.UpdateLayout();
                _RemoveGroupWin.Close();

                return;
            }
            else if (GroupToRemove.Friends.Count == 0)
            {
                RemoveGroup(GroupToRemove.GroupID, false);
                GroupTree.RemoveGroup(GroupToRemove);
                TreeFriend.UpdateLayout();
                _RemoveGroupWin.Close();

                return;
            }

            foreach (Users child in GroupToRemove.Friends)
            {
                MoveContact(_UserID, child.UserID, GroupMoveTo.GroupID);
                GroupTree.MoveFriend(child, GroupToRemove, GroupMoveTo);
            }

            GroupTree.RemoveGroup(GroupToRemove);

            TreeFriend.UpdateLayout();
            _RemoveGroupWin.Close();
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT PERFORM

        private void UpdateRequest_Tick(object sender, EventArgs e)
        {            
            int totalRequest = RequestVM.Requests.Count;

            requestTaskZone.Visibility = totalRequest == 0 ? Visibility.Collapsed : Visibility.Visible;

            List<Users> Requests = FriendRequests(_UserID).Skip(totalRequest).ToList(); //Skip amount of "totalRequest" request got before

            if (Requests.Count == 0)
                return;

            foreach (Users Friend in Requests)
                RequestVM.AppendRequest(Friend);

            friendRequestZone.UpdateLayout();
        }

        private void AddFriendModule_OnAddFriendSuccess(object sender, AddFriendArgs e)
        {
            _AddFriendWin.Close();
            MessageBox.Show(String.Format("Đã thêm '{0}' vào nhóm {1} thành công. Vui lòng đợi '{0}' hồi đáp yêu cầu kết bạn của bạn", e.FriendName, e.GroupName));
        }

        private void AddFriendModule_OnAddFriendError(object sender, AddFriendArgs e)
        {
            MessageBox.Show(String.Format("Yêu cầu kết bạn với '{0}' không thành công. Vui lòng thử lại sau", e.FriendName));
        }
        
        private void GroupTree_OnMoveContact(Users Friend, FriendGroup OldGroup)
        {
            FriendGroup NewGroup = cbNewGroup.SelectedItem as FriendGroup;

            int NewGroupID = 0;

            if(NewGroup == null || _MoveNewGroupName != null)
                if (_MoveNewGroupName != null)
                    if (!AddNewGroup(_UserID, _MoveNewGroupName, ref NewGroupID))
                        MessageBox.Show("Thêm nhóm mới không thành công");

            MoveContact(_UserID, Friend.UserID, NewGroupID != 0 ? GetGroup(NewGroupID).GroupID : NewGroup.GroupID);

            GroupTree.MoveFriend(Friend, OldGroup, NewGroupID != 0 ? GetGroup(NewGroupID) : NewGroup);

            TreeFriend.UpdateLayout();
        }

        private void GroupTree_OnRemoveContact(Users Friend, FriendGroup OldGroup)
        {
            RemoveContact(_UserID, Friend.UserID);

            GroupTree.RemoveFriend(Friend, OldGroup);

            TreeFriend.UpdateLayout();
        }

        private void TreeFriend_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Object SelectedObj = (Object)TreeFriend.SelectedItem;

            foreach (Object parentObj in TreeFriend.Items)
            {
                TreeViewItem Group = TreeFriend.ItemContainerGenerator.ContainerFromItem(parentObj) as TreeViewItem;
                TreeViewItem MatchItem = TreeFriend.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem;

                if (MatchItem != null)
                {
                    MatchItem.ContextMenu = TreeFriend.Resources["GroupContext"] as ContextMenu;
                    break;
                }

                TreeViewItem Friend = Group.ItemContainerGenerator.ContainerFromItem(SelectedObj) as TreeViewItem;
                if(Friend != null)
                    Friend.ContextMenu = TreeFriend.Resources["FriendContext"] as ContextMenu;
            }          
        }

        private void RequestVM_OnAcceptRequest(Users Friend)
        {
            FriendGroup AvailableGroup = cbRequestGroup.SelectedItem as FriendGroup;

            int NewGroupID = 0;

            if (AvailableGroup == null || _RequestNewGroupName != null)
                if (_RequestNewGroupName != null)
                    if (!AddNewGroup(_UserID, _RequestNewGroupName, ref NewGroupID))
                        MessageBox.Show("Thêm nhóm mới không thành công");

            AcceptRequest(_UserID, Friend.UserID, NewGroupID != 0 ? GetGroup(NewGroupID).GroupID : AvailableGroup.GroupID);

            GroupTree.AppendFriend(Friend, NewGroupID != 0 ? GetGroup(NewGroupID) : AvailableGroup);

            RequestVM.RemoveRequest(Friend);

            friendRequestZone.UpdateLayout();
            TreeFriend.UpdateLayout();
        }

        private void RequestVM_OnIgnoreRequest(Users Friend)
        {
            IgnoreRequest(_UserID, Friend.UserID);
            RequestVM.RemoveRequest(Friend);

            friendRequestZone.UpdateLayout();
        }        

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _GroupTree.SearchCommand.Execute(null);
        }
  
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {            
            btnSelectAll.Visibility = System.Windows.Visibility.Visible;
            btnDeselectAll.Visibility = System.Windows.Visibility.Visible;
            completeTask.Visibility = System.Windows.Visibility.Visible;
        }

        private void chkDone_Checked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = true;
        }

        private void chkDone_Unchecked(object sender, RoutedEventArgs e)
        {
            btnDone.IsEnabled = false;
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
                    _RequestVM.AcceptCommand.Execute(null);
                    break;
                case "Ignore":
                    _RequestVM.IgnoreCommand.Execute(null);
                    break;
            }

            chkRequestTaskDone.IsChecked = false;
        }

        private void btnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            Helper.CreateWindow(ref _AddFriendWin, "Thêm Bạn", _AddFriendModule).ShowDialog();
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
                    _GroupTree.MoveCommand.Execute(null);
                    break;

                case "RemoveContact":
                    _GroupTree.RemoveCommand.Execute(null);
                    break;
            }

            chkDone.IsChecked = false;
            btnDone.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _GroupTree.CancelEditCommand.Execute(null);
            btnSelectAll.Visibility = System.Windows.Visibility.Collapsed;
            btnDeselectAll.Visibility = System.Windows.Visibility.Collapsed;
            completeTask.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void mnuAddFriend_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = TreeFriend.SelectedItem as GroupViewModel;

            _AddFriendModule.SetDefaultGroup(SelectedGroup.Group);

            Helper.CreateWindow(ref _AddFriendWin, "Thêm Bạn", _AddFriendModule).ShowDialog();
        }

        private void mnuRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupViewModel SelectedGroup = TreeFriend.SelectedItem as GroupViewModel;
                        
            _RemoveGroupModule.SetGroupToRemove(SelectedGroup.Group);

            Helper.CreateWindow(ref _RemoveGroupWin, "Xóa Nhóm", _RemoveGroupModule).ShowDialog();
        }

        private void mnuFriendDetail_Click(object sender, RoutedEventArgs e)
        {
            FriendViewModel SelectedFriend = TreeFriend.SelectedItem as FriendViewModel;
        }

        private void TreeFriend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem DBClickItem = e.Source as TreeViewItem;

            if (DBClickItem == null)
                return;

            object Item = DBClickItem.Header;

            if (Item is FriendViewModel)
            {
                FriendViewModel SelectedFriend = TreeFriend.SelectedItem as FriendViewModel;
                Users FriendArg = SelectedFriend.Friend;

                OnFriendDoubleClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
            }
        }

        #endregion

        
    }    
}
