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
    public class GroupTreeViewModel
    {
        private ReadOnlyCollection<GroupViewModel> _Groups;       
        private readonly ICommand _SearchCommand;
        private String _SearchText = String.Empty;
        private IEnumerator<FriendViewModel> _MatchingFriendEnumerator;

        private readonly ICommand _EditCommand;
        private readonly ICommand _SelectAllCommand;
        private readonly ICommand _UnselectAllCommand;

        public ReadOnlyCollection<GroupViewModel> Groups
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
                _MatchingFriendEnumerator = null;
            }
        }

        public ICommand EditCommand
        {
            get { return _EditCommand; }
        }

        public ICommand SelectAllCommand
        {
            get { return _SelectAllCommand; }
        }

        public ICommand UnselectAllCommand
        {
            get { return _UnselectAllCommand; }
        }

        public GroupTreeViewModel(List<FriendGroup> Groups)
        {
            _Groups = new ReadOnlyCollection<GroupViewModel>(
                    (from child in Groups
                     select new GroupViewModel(child))
                     .ToList()
                );

            _SearchCommand = new SearchFriendCommand(this);
            _EditCommand = new EditFLCommand(this);
            _SelectAllCommand = new SelectAllFLCommand(this);
            _UnselectAllCommand = new UnselectAllFLCommand(this);
        }

        private void PerformSearch()
        {
            if (_MatchingFriendEnumerator == null || !_MatchingFriendEnumerator.MoveNext())
                this.VertifyMatchingUserEnumerator();

            var user = _MatchingFriendEnumerator.Current;

            if (user == null)
                return;

            if (user.Parent != null)
                user.Parent.IsExpanded = true;

            user.IsSelected = true;
        }

        private void VertifyMatchingUserEnumerator()
        {
            List<FriendViewModel> MatchFriends = new List<FriendViewModel>();

            foreach (GroupViewModel gvm in _Groups)
                foreach (FriendViewModel uvm in gvm.Children)
                    if (uvm.NameContainsText(_SearchText))
                        MatchFriends.Add(uvm);

            _MatchingFriendEnumerator = MatchFriends.GetEnumerator();

            if (!_MatchingFriendEnumerator.MoveNext())
            {
                MessageBox.Show(
                    "No matching names were found",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
        }

        private void PerformEdit()
        {
            foreach(GroupViewModel gvm in _Groups)
            {
                gvm.ToogleCheckbox = Visibility.Visible;
                foreach (FriendViewModel uvm in gvm.Children)
                    uvm.ToogleCheckbox = Visibility.Visible;
            }
        }

        private void PerformSelectAll()
        {
            foreach (GroupViewModel gvm in _Groups)
            {
                gvm.IsChecked = true;
                foreach (FriendViewModel uvm in gvm.Children)
                    uvm.IsChecked = true;
            }
        }

        private void PerformUnselectAll()
        {
            foreach (GroupViewModel gvm in _Groups)
            {
                gvm.IsChecked = false;
                foreach (FriendViewModel uvm in gvm.Children)
                    uvm.IsChecked = false;
            }
        }

        private IEnumerable<FriendViewModel> FindMatches(String SearchText, FriendViewModel Friend)
        {
            if (Friend.NameContainsText(SearchText))
                yield return Friend;
        }

        private class SearchFriendCommand : ICommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public SearchFriendCommand(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public bool CanExecute(object Parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object Parameter)
            {
                _GroupTree.PerformSearch();
            }
        }

        private class EditFLCommand : ICommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public EditFLCommand(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public bool CanExecute(object Parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object Parameter)
            {
                _GroupTree.PerformEdit();
            }
        }

        private class SelectAllFLCommand : ICommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public SelectAllFLCommand(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public bool CanExecute(object Parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object Parameter)
            {
                _GroupTree.PerformSelectAll();
            }
        }

        private class UnselectAllFLCommand : ICommand
        {
            private readonly GroupTreeViewModel _GroupTree;

            public UnselectAllFLCommand(GroupTreeViewModel GroupTree)
            {
                _GroupTree = GroupTree;
            }

            public bool CanExecute(object Parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object Parameter)
            {
                _GroupTree.PerformUnselectAll();
            }
        }
    }
}
