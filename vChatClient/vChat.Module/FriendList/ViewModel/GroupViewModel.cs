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

        private readonly ObservableCollection<FriendViewModel> children;
        private readonly FriendGroup group;

        private bool isExpanded;
        private bool isSelected;
        private bool isChecked;
        private Visibility toogleCheckbox = Visibility.Collapsed; //Default

        #endregion

        #region PROPERTY

        public FriendGroup Group
        {
            get { return group; }
        }

        public String GroupName
        {
            get { return group.Name; }
        }

        public ObservableCollection<FriendViewModel> Children
        {
            get { return children; }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value != isChecked)
                {
                    isChecked = value;

                    //Set IsChecked property for all child
                    foreach (FriendViewModel child in children)
                        child.IsChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        public Visibility ToogleCheckbox
        {
            get { return toogleCheckbox; }
            set
            {
                if (value != toogleCheckbox)
                {
                    toogleCheckbox = value;
                    this.OnPropertyChanged("ToogleCheckbox");
                }
            }
        }

        #endregion

        public GroupViewModel(FriendGroup Group)
        {
            group = Group;

            children = new ObservableCollection<FriendViewModel>(
                    (from Friend in @group.Friends
                     select new FriendViewModel(Friend, this))
                     .ToList()
                ); 
        }

        #region MAIN METHOD

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
