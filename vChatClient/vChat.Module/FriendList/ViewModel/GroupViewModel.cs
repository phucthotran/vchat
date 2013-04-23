using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows.Controls;
using System.Windows;

namespace vChat.Module.FriendList
{
    public class GroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private readonly ObservableCollection<FriendViewModel> _Children;
        private readonly FriendGroup _Group;

        private bool _IsExpanded;
        private bool _IsSelected;
        private bool _IsChecked;
        private Visibility _ToogleCheckbox = Visibility.Collapsed; //Default

        #endregion

        #region PROPERTY

        public FriendGroup Group
        {
            get { return _Group; }
        }

        public String GroupName
        {
            get { return _Group.Name; }
        }

        public ObservableCollection<FriendViewModel> Children
        {
            get { return _Children; }
        }

        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
            }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;

                    //Set IsChecked property for all child
                    foreach (FriendViewModel child in _Children)
                        child.IsChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        public Visibility ToogleCheckbox
        {
            get { return _ToogleCheckbox; }
            set
            {
                if (value != _ToogleCheckbox)
                {
                    _ToogleCheckbox = value;
                    this.OnPropertyChanged("ToogleCheckbox");
                }
            }
        }

        #endregion

        public GroupViewModel(FriendGroup Group)
        {
            _Group = Group;

            _Children = new ObservableCollection<FriendViewModel>(
                    (from Friend in _Group.Friends
                     select new FriendViewModel(Friend, this))
                     .ToList()
                );      
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
