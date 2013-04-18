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

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Interaction logic for FriendsList.xaml
    /// </summary>
    public partial class FriendsList : UserControl
    {
        public delegate void AddFriendButtonHandler(object sender, RoutedEventArgs e);
        public event AddFriendButtonHandler OnAddFriendClick;

        public delegate void GroupClickHandler(object sender, GroupArgs e);
        public event GroupClickHandler OnGroupClick;

        public delegate void FriendClickHandler(object sender, FriendArgs e);
        public event FriendClickHandler OnFriendClick;

        #region CLASS MEMBER

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

            _GroupTree = new GroupTreeViewModel(GroupFriend.FriendGroups);
            _GroupTree.OnMoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnMoveContact);
            _GroupTree.OnRemoveContact += new GroupTreeViewModel.FriendHandler(GroupTree_OnRemoveContact);

            _RequestVM = new RequestViewModel(FriendRequests(UserID));
            _RequestVM.OnAcceptRequest += new RequestViewModel.FriendRequestHandler(RequestVM_OnAcceptRequest);
            _RequestVM.OnIgnoreRequest += new RequestViewModel.FriendRequestHandler(RequestVM_OnIgnoreRequest);

            base.DataContext = _GroupTree;
            friendRequestZone.DataContext = _RequestVM;
            cbRequestGroup.ItemsSource = GroupFriend.FriendGroups;
            cbNewGroup.ItemsSource = GroupFriend.FriendGroups;
        }

        #endregion

        #region EVENT PERFORM

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
            if (TreeFriend.SelectedItem is FriendViewModel)
            {
                FriendViewModel SelectedFriend = TreeFriend.SelectedItem as FriendViewModel;
                Users FriendArg = SelectedFriend.Friend;

                OnFriendClick(this, new FriendArgs(
                        FriendArg.UserID,
                        FriendArg.Username,
                        FriendArg.FirstName,
                        FriendArg.LastName
                    ));
            }
            else if (TreeFriend.SelectedItem is GroupViewModel)
            {
                GroupViewModel SelectedGroup = TreeFriend.SelectedItem as GroupViewModel;
                FriendGroup GroupArg = SelectedGroup.Group;

                OnGroupClick(this, new GroupArgs(
                        GroupArg.GroupID,
                        GroupArg.Name
                    ));
            }
        }

        private void RequestVM_OnAcceptRequest(Users Friend)
        {
            FriendGroup SelectedGroup = (FriendGroup)cbRequestGroup.SelectedItem;

            AcceptRequest(_UserID, Friend.UserID, SelectedGroup.GroupID);

            _GroupTree.AppendFriend(Friend, SelectedGroup);

            _RequestVM.RemoveRequest(Friend);

            if (_RequestVM.Requests.Count == 0)
            {
                requestTaskZone.Visibility = System.Windows.Visibility.Collapsed;
                chkRequestTaskDone.IsEnabled = false;
            }

            friendRequestZone.UpdateLayout();
            TreeFriend.UpdateLayout();
        }

        private void RequestVM_OnIgnoreRequest(Users Friend)
        {
            IgnoreRequest(_UserID, Friend.UserID);
            _RequestVM.RemoveRequest(Friend);

            if (_RequestVM.Requests.Count == 0)
            {
                requestTaskZone.Visibility = System.Windows.Visibility.Collapsed;
                chkRequestTaskDone.IsEnabled = false;
            }

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
        }

        private void btnAddFriend_Click(object sender, RoutedEventArgs e)
        {
            OnAddFriendClick(sender, e);
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

        #endregion                
    }    
}
