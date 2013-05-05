using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using vChat.Model.Entities;
using System.Windows;
using System.Windows.Media;

namespace vChat.Module.FriendList
{
    public partial class GroupTreeViewModel
    {
        public delegate void FriendHandler(Users Friend, FriendGroup OldGroup);
        public event FriendHandler OnMoveContact;
        public event FriendHandler OnRemoveContact;

        #region CLASS MEMBER

        private readonly ObservableCollection<GroupViewModel> groups;       
        private readonly ICommand searchCommand;
        private String searchText = String.Empty;
        private List<FriendViewModel> matchFriends;
        private Brush matchColor = Brushes.Red;

        private readonly ICommand editCommand;
        private readonly ICommand cancelEditCommand;
        private readonly ICommand selectCommand;
        private readonly ICommand deselectCommand;
        private readonly ICommand moveCommand;
        private readonly ICommand removeCommand;

        #endregion

        #region PROPERTY

        public ObservableCollection<GroupViewModel> Groups
        {
            get { return groups; }
        }

        public ICommand SearchCommand
        {
            get { return searchCommand; }
        }

        public String SearchText
        {
            get { return searchText; }
            set
            {
                if (value == searchText)
                    return;

                searchText = value;
                
                //Reset Search Result
                if(matchFriends != null)
                    foreach (FriendViewModel Friend in matchFriends)
                        Friend.MatchColor = System.Windows.Media.Brushes.Black;

                matchFriends = null;
            }
        }

        public ICommand EditCommand
        {
            get { return editCommand; }
        }

        public ICommand CancelEditCommand
        {
            get { return cancelEditCommand; }
        }

        public ICommand SelectCommand
        {
            get { return selectCommand; }
        }

        public ICommand DeselectCommand
        {
            get { return deselectCommand; }
        }

        public ICommand MoveCommand
        {
            get { return moveCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return removeCommand; }
        }

        #endregion

        public GroupTreeViewModel(ObservableCollection<FriendGroup> Groups)
        {
            groups = new ObservableCollection<GroupViewModel>();

            foreach (FriendGroup child in Groups)
                groups.Add(new GroupViewModel(child));

            searchCommand = new SearchTask(this);
            editCommand = new EditTask(this);
            selectCommand = new SelectTask(this);
            deselectCommand = new DeselectTask(this);
            moveCommand = new MoveTask(this);
            removeCommand = new RemoveTask(this);
            cancelEditCommand = new CancelEditTask(this);
        }

        #region MAIN METHOD

        public void AppendGroup(FriendGroup Group)
        {
            if(Group != null)
                groups.Add(new GroupViewModel(Group));
        }

        public void RemoveGroup(FriendGroup Group)
        {
            if (Group == null)
                return;

            GroupViewModel MatchGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));

            if(MatchGroup != null)
                groups.Remove(MatchGroup);
        }

        public void AppendFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));

            if (ParentGroup == null)
            {
                AppendGroup(Group);
                ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));
            }
            
            ParentGroup.Children.Add(new FriendViewModel(Friend, ParentGroup));
        }

        public void RemoveFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(Group));
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend));

            if (ParentGroup != null && MatchFriend != null)
                ParentGroup.Children.Remove(MatchFriend);
        }

        public void MoveFriend(Users Friend, FriendGroup OldGroup, FriendGroup NewGroup)
        {
            if (Friend == null || OldGroup == null || NewGroup == null)
                return;

            GroupViewModel NewParentGroup = groups.FirstOrDefault(g => g.Group.Equals(NewGroup));
            GroupViewModel ParentGroup = groups.FirstOrDefault(g => g.Group.Equals(OldGroup));
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend));

            FriendViewModel MatchFriendStateFull = new FriendViewModel(Friend, NewParentGroup);
            MatchFriendStateFull.ToogleCheckbox = MatchFriend.ToogleCheckbox;
            
            ParentGroup.Children.Remove(MatchFriend);
            NewParentGroup.Children.Add(MatchFriendStateFull);
        }

        public void SetFriendStatus(String Username, bool IsOnline)
        {
            foreach (GroupViewModel Parent in groups)
            {
                FriendViewModel MatchUser = Parent.Children.FirstOrDefault(f => f.Friend.Username.Equals(Username));

                if (MatchUser != null)
                {
                    MatchUser.IsOnline = IsOnline;
                    break;
                }
            }
        }

        #endregion

        #region COMMAND PREFORM

        private void PerformSearch()
        {
            if (String.IsNullOrWhiteSpace(searchText))
                return;

            matchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in groups)
            {
                matchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.NameContainsText(searchText))
                                      .ToList());
            }

            foreach (FriendViewModel Friend in matchFriends)
            {
                Friend.Parent.IsExpanded = true;
                Friend.MatchColor = matchColor;
            }
        }        

        private void PerformEdit()
        {
            foreach(GroupViewModel Parent in groups)
            {
                Parent.ToogleCheckbox = Visibility.Visible;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Visible;
            }
        }

        private void PerformCancelEdit()
        {
            foreach (GroupViewModel Parent in groups)
            {
                Parent.ToogleCheckbox = Visibility.Collapsed;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Collapsed;
            }
        }

        private void PerformSelect()
        {
            foreach (GroupViewModel Parent in groups)
            {
                Parent.IsChecked = true;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = true;
            }
        }

        private void PerformDeselect()
        {
            foreach (GroupViewModel Parent in groups)
            {
                Parent.IsChecked = false;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = false;
            }
        }

        private void PerformMove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.IsChecked == true)
                                      .ToList());
            }

            foreach (FriendViewModel child in MatchFriends)
                OnMoveContact(child.Friend, child.Parent.Group);
        }

        private void PerformRemove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(friend => friend.IsChecked == true)
                                      .ToList());
            }

            foreach (FriendViewModel child in MatchFriends)
                OnRemoveContact(child.Friend, child.Parent.Group);
        }

        #endregion
    }
}
