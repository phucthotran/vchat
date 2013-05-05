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
    public partial class FriendsList : UserControl
    {
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
        private int _UserID;

        #endregion

        public FriendsList()
        {
            InitializeComponent();
        }        

        #region MAIN METHOD

        public void SetupData(int UserID)
        {
            _UserID = UserID;

            GroupFriendList GroupFriend = FriendList(UserID);
            List<Users> Requests = FriendRequests(UserID);

            ChangeAvatarModule.ChangeAvatarFor(UserID);

            _AddFriendModule = new AddFriend.AddFriend();
            _AddFriendModule.SetupData(UserID);
            _AddFriendModule.IntegratedWith(this);
            _AddFriendModule.OnAddFriendSuccess += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendSuccess);
            _AddFriendModule.OnAddFriendError += new AddFriend.AddFriend.AddingHandler(AddFriendModule_OnAddFriendError);

            _RemoveGroupModule = new Module.RemoveGroup.RemoveGroup();
            _RemoveGroupModule.SetupData(GroupFriend.FriendGroups);
            _RemoveGroupModule.IntegratedWith(this);

            _GroupTree = new GroupTreeViewModel(GroupFriend.FriendGroups);
            _GroupTree.OnMoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnMoveContact);
            _GroupTree.OnRemoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnRemoveContact);

            _RequestVM = new RequestViewModel(Requests);
            _RequestVM.OnAcceptRequest += new RequestViewModel.RequestHandler(RequestVM_OnAcceptRequest);
            _RequestVM.OnIgnoreRequest += new RequestViewModel.RequestHandler(RequestVM_OnIgnoreRequest);

            base.DataContext = _GroupTree;
            friendRequestZone.DataContext = _RequestVM;
            cbRequestGroup.ItemsSource = GroupFriend.FriendGroups;
            cbNewGroup.ItemsSource = GroupFriend.FriendGroups;

            _UpdateRequest = new DispatcherTimer();
            _UpdateRequest.Interval = TimeSpan.FromMilliseconds(1000);
            _UpdateRequest.Tick += new EventHandler(UpdateRequest_Tick);
            _UpdateRequest.Start();
        }                

        private void CreateAddFriendWindow()
        {
            _AddFriendWin = new MetroWindow();
            _AddFriendWin.Title = "Thêm Bạn";
            _AddFriendWin.SizeToContent = SizeToContent.WidthAndHeight;
            _AddFriendWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _AddFriendWin.ResizeMode = ResizeMode.NoResize;
            _AddFriendWin.ShowInTaskbar = false;
            _AddFriendWin.Content = _AddFriendModule;
            _AddFriendWin.InitTheme();
        }

        private void CreateRemoveGroupWindow()
        {
            _RemoveGroupWin = new MetroWindow();
            _RemoveGroupWin.Title = "Xóa Nhóm";
            _RemoveGroupWin.SizeToContent = SizeToContent.WidthAndHeight;
            _RemoveGroupWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _RemoveGroupWin.ResizeMode = ResizeMode.NoResize;
            _RemoveGroupWin.ShowInTaskbar = false;
            _RemoveGroupWin.Content = _RemoveGroupModule;
            _RemoveGroupWin.InitTheme();
        }

        public void DoAddFriend(Users Friend, FriendGroup Group)
        {
            _GroupTree.AppendFriend(Friend, Group);
            TreeFriend.UpdateLayout();
        }

        public void DoRemoveGroup(bool RemoveContact, FriendGroup GroupMoveTo = null)
        {
            GroupViewModel SelectedGroup = TreeFriend.SelectedItem as GroupViewModel;

            if (SelectedGroup.Children.Count > 0 && RemoveContact)
            {
                RemoveGroup(SelectedGroup.Group.GroupID, RemoveContact);
                _GroupTree.RemoveGroup(SelectedGroup.Group);
                TreeFriend.UpdateLayout();

                return;
            }
            else if (SelectedGroup.Children.Count == 0)
            {
                RemoveGroup(SelectedGroup.Group.GroupID, false);
                _GroupTree.RemoveGroup(SelectedGroup.Group);
                TreeFriend.UpdateLayout();

                return;
            }

            List<FriendViewModel> Friends = SelectedGroup.Children.ToList();

            foreach (FriendViewModel child in Friends)
            {
                MoveContact(_UserID, child.Friend.UserID, GroupMoveTo.GroupID);
                _GroupTree.MoveFriend(child.Friend, SelectedGroup.Group, GroupMoveTo);
            }

            _GroupTree.RemoveGroup(SelectedGroup.Group);

            TreeFriend.UpdateLayout();
        }        

        #endregion

        #region EVENT PERFORM

        private void UpdateRequest_Tick(object sender, EventArgs e)
        {            
            int totalRequest = _RequestVM.Requests.Count;

            requestTaskZone.Visibility = totalRequest == 0 ? Visibility.Collapsed : Visibility.Visible;

            List<Users> Requests = FriendRequests(_UserID).Skip(totalRequest).ToList(); //Skip amount of "totalRequest" request got before

            if (Requests.Count == 0)
                return;

            foreach (Users Friend in Requests)
                _RequestVM.AppendRequest(Friend);

            friendRequestZone.UpdateLayout();
        }

        private void AddFriendModule_OnAddFriendSuccess(object sender, AddFriendArgs e)
        {
            MessageBox.Show(String.Format("Add Success - FriendName: {0}, GroupName: {1}", e.FriendName, e.GroupName));
        }

        private void AddFriendModule_OnAddFriendError(object sender, AddFriendArgs e)
        {
            MessageBox.Show(String.Format("Add Error - FriendName: {0}, GroupName: {1}", e.FriendName, e.GroupName ?? "NULL"));
        }
        
        private void GroupTree_OnMoveContact(Users Friend, FriendGroup OldGroup)
        {
            FriendGroup NewGroup = (FriendGroup)cbNewGroup.SelectedItem;

            MoveContact(_UserID, Friend.UserID, NewGroup.GroupID);

            _GroupTree.MoveFriend(Friend, OldGroup, NewGroup);

            TreeFriend.UpdateLayout();
        }

        private void GroupTree_OnRemoveContact(Users Friend, FriendGroup OldGroup)
        {
            RemoveContact(_UserID, Friend.UserID);

            _GroupTree.RemoveFriend(Friend, OldGroup);

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
            FriendGroup SelectedGroup = (FriendGroup)cbRequestGroup.SelectedItem;

            AcceptRequest(_UserID, Friend.UserID, SelectedGroup.GroupID);

            _GroupTree.AppendFriend(Friend, SelectedGroup);

            _RequestVM.RemoveRequest(Friend);

            friendRequestZone.UpdateLayout();
            TreeFriend.UpdateLayout();
        }

        private void RequestVM_OnIgnoreRequest(Users Friend)
        {
            IgnoreRequest(_UserID, Friend.UserID);
            _RequestVM.RemoveRequest(Friend);

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
            CreateAddFriendWindow();
            _AddFriendWin.ShowDialog();
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
                    break;

                case "RemoveContact":
                    tblNewGroup.Visibility = System.Windows.Visibility.Collapsed;
                    cbNewGroup.Visibility = System.Windows.Visibility.Collapsed;
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
            CreateAddFriendWindow();
            _AddFriendWin.ShowDialog();
        }

        private void mnuRemoveGroup_Click(object sender, RoutedEventArgs e)
        {            
            CreateRemoveGroupWindow();
            _RemoveGroupWin.ShowDialog();
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
