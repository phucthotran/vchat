using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using vChat.Model.Entities;
using System.Windows;

namespace vChat.Module.FriendList
{
    public partial class GroupTreeViewModel
    {
        public delegate void FriendHandler(Users Friend, FriendGroup OldGroup);
        public event FriendHandler OnMoveContact;
        public event FriendHandler OnRemoveContact;

        #region CLASS MEMBER

        private readonly ObservableCollection<GroupViewModel> _Groups;       
        private readonly ICommand _SearchCommand;
        private String _SearchText = String.Empty;
        private List<FriendViewModel> _MatchFriends;

        private readonly ICommand _EditCommand;
        private readonly ICommand _CancelEditCommand;
        private readonly ICommand _SelectCommand;
        private readonly ICommand _DeselectCommand;
        private readonly ICommand _MoveCommand;
        private readonly ICommand _RemoveCommand;

        #endregion

        #region PROPERTY

        public ObservableCollection<GroupViewModel> Groups
        {
            get { return _Groups; }
        }

        public ICommand SearchCommand
        {
            get { return _SearchCommand; }
        }

        public String SearchText
        {
            get { return _SearchText; }
            set
            {
                if (value == _SearchText)
                    return;

                _SearchText = value;
                
                //Reset Search Result
                if(_MatchFriends != null)
                    foreach (FriendViewModel Friend in _MatchFriends)
                        Friend.MatchColor = System.Windows.Media.Brushes.Black;

                _MatchFriends = null;
            }
        }

        public ICommand EditCommand
        {
            get { return _EditCommand; }
        }

        public ICommand CancelEditCommand
        {
            get { return _CancelEditCommand; }
        }

        public ICommand SelectCommand
        {
            get { return _SelectCommand; }
        }

        public ICommand DeselectCommand
        {
            get { return _DeselectCommand; }
        }

        public ICommand MoveCommand
        {
            get { return _MoveCommand; }
        }

        public ICommand RemoveCommand
        {
            get { return _RemoveCommand; }
        }

        #endregion

        public GroupTreeViewModel(ObservableCollection<FriendGroup> Groups)
        {
            _Groups = new ObservableCollection<GroupViewModel>();

            foreach (FriendGroup child in Groups)
                _Groups.Add(new GroupViewModel(child));

            _SearchCommand = new SearchTask(this);
            _EditCommand = new EditTask(this);
            _SelectCommand = new SelectTask(this);
            _DeselectCommand = new DeselectTask(this);
            _MoveCommand = new MoveTask(this);
            _RemoveCommand = new RemoveTask(this);
            _CancelEditCommand = new CancelEditTask(this);
        }

        #region MAIN METHOD

        public void AppendGroup(FriendGroup Group)
        {
            if(Group != null)
                _Groups.Add(new GroupViewModel(Group));
        }

        public void RemoveGroup(FriendGroup Group)
        {
            if (Group == null)
                return;

            GroupViewModel MatchGroup = _Groups.FirstOrDefault(g => g.Group.Equals(Group));

            if(MatchGroup != null)
                _Groups.Remove(MatchGroup);
        }

        public void AppendFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = _Groups.FirstOrDefault(g => g.Group.Equals(Group));

            if(ParentGroup != null)
                ParentGroup.Children.Add(new FriendViewModel(Friend, ParentGroup));
        }

        public void RemoveFriend(Users Friend, FriendGroup Group)
        {
            if (Friend == null || Group == null)
                return;

            GroupViewModel ParentGroup = _Groups.FirstOrDefault(g => g.Group.Equals(Group));
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend));

            if (ParentGroup != null && MatchFriend != null)
                ParentGroup.Children.Remove(MatchFriend);
        }

        public void MoveFriend(Users Friend, FriendGroup OldGroup, FriendGroup NewGroup)
        {
            if (Friend == null || OldGroup == null || NewGroup == null)
                return;

            GroupViewModel NewParentGroup = _Groups.FirstOrDefault(g => g.Group.Equals(NewGroup));
            GroupViewModel ParentGroup = _Groups.FirstOrDefault(g => g.Group.Equals(OldGroup));
            FriendViewModel MatchFriend = ParentGroup.Children.FirstOrDefault(f => f.Friend.Equals(Friend));

            FriendViewModel MatchFriendStateFull = new FriendViewModel(Friend, NewParentGroup);
            MatchFriendStateFull.ToogleCheckbox = MatchFriend.ToogleCheckbox;
            
            ParentGroup.Children.Remove(MatchFriend);
            NewParentGroup.Children.Add(MatchFriendStateFull);
        }

        #endregion

        #region COMMAND PREFORM

        private void PerformSearch()
        {
            if (String.IsNullOrWhiteSpace(_SearchText))
                return;

            _MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in _Groups)
            {
                _MatchFriends.AddRange(Parent.Children
                                      .Where(f => (f.Friend.FirstName + " " + f.Friend.LastName)
                                      .IndexOf(_SearchText, StringComparison.InvariantCultureIgnoreCase) > -1)
                                      .ToList());
            }

            foreach (FriendViewModel Friend in _MatchFriends)
            {
                Friend.Parent.IsExpanded = true;
                Friend.MatchColor = System.Windows.Media.Brushes.Red;
            }
        }        

        private void PerformEdit()
        {
            foreach(GroupViewModel Parent in _Groups)
            {
                Parent.ToogleCheckbox = Visibility.Visible;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Visible;
            }
        }

        private void PerformCancelEdit()
        {
            foreach (GroupViewModel Parent in _Groups)
            {
                Parent.ToogleCheckbox = Visibility.Collapsed;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.ToogleCheckbox = Visibility.Collapsed;
            }
        }

        private void PerformSelect()
        {
            foreach (GroupViewModel Parent in _Groups)
            {
                Parent.IsChecked = true;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = true;
            }
        }

        private void PerformDeselect()
        {
            foreach (GroupViewModel Parent in _Groups)
            {
                Parent.IsChecked = false;
                foreach (FriendViewModel Child in Parent.Children)
                    Child.IsChecked = false;
            }
        }

        private void PerformMove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in _Groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(f => f.IsChecked == true)
                                      .ToList());
            }

            foreach (FriendViewModel child in MatchFriends)
                OnMoveContact(child.Friend, child.Parent.Group);
        }

        private void PerformRemove()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel Parent in _Groups)
            {
                MatchFriends.AddRange(Parent.Children
                                      .Where(f => f.IsChecked == true)
                                      .ToList());
            }

            foreach (FriendViewModel child in MatchFriends)
                OnRemoveContact(child.Friend, child.Parent.Group);
        }

        #endregion
    }
}
