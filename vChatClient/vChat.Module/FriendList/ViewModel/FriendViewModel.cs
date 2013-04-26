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

namespace vChat.Module.FriendList
{
    public class FriendViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private readonly GroupViewModel _Parent;
        private readonly Users _Friend;

        private bool _IsSelected;
        private bool _IsChecked;
        private Visibility _ToogleCheckbox = Visibility.Collapsed; //Default
        private Brush _MatchColor = Brushes.Black; //Default (Not yet search)

        #endregion

        #region PROPERTY

        public GroupViewModel Parent
        {
            get { return _Parent; }
        }

        public Users Friend
        {
            get { return _Friend; }
        }

        public String FriendName
        {
            get { return (String.Format("{0} {1}", _Friend.FirstName, _Friend.LastName)); }
        }

        public ImageSource ProfilePicture
        {
            get
            {
                if (_Friend.Picture == null)
                    return null;
                                
                MemoryStream msImage = new MemoryStream(_Friend.Picture);
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = msImage;
                bitmap.EndInit();                                

                return bitmap as ImageSource;
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

                    //If all child element was checked, Parent would be checked
                    int TotalChild = _Parent.Children.Count;
                    int TotalChecked = _Parent.Children.Where(p => p.IsChecked == value).Count();

                    if (TotalChecked == TotalChild)
                        _Parent.IsChecked = value;

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

        public Brush MatchColor
        {
            get { return _MatchColor; }
            set
            {
                if (value != _MatchColor)
                {
                    _MatchColor = value;
                    this.OnPropertyChanged("MatchColor");
                }
            }
        }

        #endregion

        public FriendViewModel(Users Friend, GroupViewModel Parent)
        {
            _Friend = Friend;
            _Parent = Parent;
        }

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
    }
}
