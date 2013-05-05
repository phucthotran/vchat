using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using Core.Data;
using System.Net;
using Core.Client;
using vChat.Lib;

namespace vChat.Module.FriendList
{
    public class FriendViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private readonly GroupViewModel parent;
        private readonly Users friend;

        private bool isSelected;
        private bool isChecked;
        private bool isOnline;
        private Visibility toogleCheckbox = Visibility.Collapsed; //Default
        private Brush matchColor = Brushes.Black; //Default (Not yet search)

        #endregion

        #region PROPERTY

        public GroupViewModel Parent
        {
            get { return parent; }
        }

        public Users Friend
        {
            get { return friend; }
        }

        public String FriendName
        {
            get { return (String.Format("{0} {1}", friend.FirstName, friend.LastName)); }
        }

        public ImageSource ProfilePicture
        {
            get
            {
                if (friend.Picture == null)
                    return null;                                
               
                return ImageByteConverter.GetFromBytes(friend.Picture);                
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

                    //If all child element was checked, Parent would be checked
                    int TotalChild = parent.Children.Count;
                    int TotalChecked = parent.Children.Where(p => p.IsChecked == value).Count();

                    if (TotalChecked == TotalChild)
                        parent.IsChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                if (isOnline != value)
                {
                    isOnline = value;
                    this.OnPropertyChanged("IsOnline");
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

        public Brush MatchColor
        {
            get { return matchColor; }
            set
            {
                if (value != matchColor)
                {
                    matchColor = value;
                    this.OnPropertyChanged("MatchColor");
                }
            }
        }

        #endregion

        public FriendViewModel(Users Friend, GroupViewModel Parent)
        {
            friend = Friend;
            parent = Parent;
        }

        #region MAIN METHOD

        public bool NameContainsText(String Text)
        {
            if (String.IsNullOrWhiteSpace(FriendName) && String.IsNullOrWhiteSpace(Text))
                return false;

            return FriendName.IndexOf(Text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
