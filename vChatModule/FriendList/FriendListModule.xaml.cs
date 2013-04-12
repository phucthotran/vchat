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
using vChat.Model;

namespace FriendList
{
    public class FriendGroup
    {
        private List<Group> _FriendGroups;

        public List<Group> FriendGroups
        {
            get
            {
                if (_FriendGroups == null)
                {
                    _FriendGroups = new List<Group>();
                    return _FriendGroups;
                }

                return _FriendGroups;
            }            
        }
    }

    public class Group
    {
        private List<Friend> _FriendList;

        public int ID { get; set; }
        public String Name { get; set; }
        public List<Friend> FriendList
        {
            get
            {
                if (_FriendList == null)
                {
                    _FriendList = new List<Friend>();
                    return _FriendList;
                }

                return _FriendList;
            }
            set { _FriendList = value; }
        }
    }

    public class Friend
    {
        public int ID { get; set; }
        public String Name { get; set; }        
    }

    /// <summary>
    /// Interaction logic for FriendListModule.xaml
    /// </summary>
    public partial class FriendListModule : UserControl
    {
        public delegate void FriendItems(FriendInfo e);
        public event FriendItems OnFriendItemClick;

        public delegate void GroupItems(GroupInfo e);
        public event GroupItems OnGroupItemClick;

        public FriendListModule()
        {
            InitializeComponent();
        }

        private void TreeFriend_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        public void SetupData(GroupFriendList Friends)
        {
            TreeFriend.ItemsSource = Friends.FriendGroups;
        }

        private void GroupItem_Click(object source, MouseButtonEventArgs e)
        {
            if (source is GroupItem)
            {
                GroupItem ItemClicked = (GroupItem)source;

                GroupInfo g = new GroupInfo
                {
                    ID = ItemClicked.GroupID,
                    Name = ItemClicked.GroupName
                };

                OnGroupItemClick(g);
            }
        }

        private void FriendItem_Click(object source, MouseButtonEventArgs e)
        {
            if (source is FriendItem)
            {
                FriendItem ItemClicked = (FriendItem)source;

                FriendInfo f = new FriendInfo 
                { 
                    ID = ItemClicked.UserID,
                    Name = ItemClicked.Username
                };

                OnFriendItemClick(f);
            }
        }
    }
}
