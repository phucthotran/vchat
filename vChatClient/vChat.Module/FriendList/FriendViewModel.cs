using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using vChat.Model.Entities;
using System.Windows;

namespace vChat.Module.FriendList
{
    public class FriendViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly GroupViewModel _Parent;
        private readonly Users _Friend;

        //private bool _IsExpanded;
        private bool _IsSelected;
        private bool _IsChecked;
        private Visibility _ToogleCheckbox = Visibility.Collapsed; //Default

        public GroupViewModel Parent
        {
            get { return _Parent; }
        }

        public String FriendName
        {
            get { return _Friend.Username; }
        }

        //public bool IsExpanded
        //{
        //    get { return _IsExpanded; }
        //    set
        //    {
        //        if (value != _IsExpanded)
        //        {
        //            _IsExpanded = value;
        //            this.OnPropertyChanged("IsExpanded");
        //        }
        //    }
        //}

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

        public FriendViewModel(Users Friend, GroupViewModel Parent)
        {
            _Friend = Friend;
            _Parent = Parent;
        }

        public bool NameContainsText(String Text)
        {
            if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(this.FriendName))
                return false;

            return this.FriendName.IndexOf(Text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
