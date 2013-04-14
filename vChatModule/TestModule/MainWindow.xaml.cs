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

namespace TestModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FriendGroup> Group;

        public MainWindow()
        {
            Group = new List<FriendGroup>();

            FriendGroup NIITGroup = new FriendGroup { GroupID = 2, Name = "NIIT" };
            FriendGroup EliteGroup = new FriendGroup { GroupID = 3, Name = "Elite" };
            FriendGroup FriendGroup = new FriendGroup { GroupID = 4, Name = "Friend" };

            Group.Add(NIITGroup);
            Group.Add(EliteGroup);
            Group.Add(FriendGroup);

            //FriendList.Friend f1 = new FriendList.Friend { ID = 1, Name = "itexplore" };
            //FriendList.Friend f2 = new FriendList.Friend { ID = 2, Name = "tuan_anh" };
            //FriendList.Friend f3 = new FriendList.Friend { ID = 3, Name = "huyen_trang" };

            //List<FriendList.Friend> NIIT_Group = new List<FriendList.Friend>();
            //NIIT_Group.Add(f1);
            //NIIT_Group.Add(f2);

            //List<FriendList.Friend> Elite_Group = new List<FriendList.Friend>();
            //Elite_Group.Add(f3);

            //FriendList.Group g1 = new FriendList.Group { ID = 5, Name = "NIIT", FriendList = NIIT_Group };
            //FriendList.Group g2 = new FriendList.Group { ID = 6, Name = "Elite", FriendList = Elite_Group };

            //FriendList.FriendGroup fg = new FriendList.FriendGroup();
            //fg.FriendGroups.Add(g1);
            //fg.FriendGroups.Add(g2);

            Users u1 = new Users { UserID = 1, Username = "itexplore", Password = "123" };
            Users u2 = new Users { UserID = 2, Username = "tuan_anh", Password = "123" };
            Users u3 = new Users { UserID = 3, Username = "huyentrang", Password = "123" };

            List<Users> NIIT_Friend = new List<Users>();
            NIIT_Friend.Add(u1);
            NIIT_Friend.Add(u2);

            List<Users> Elite_Friend = new List<Users>();
            Elite_Friend.Add(u3);

            FriendGroup NIIT_Group = new FriendGroup { GroupID = 5, Name = "NIIT", Friends = NIIT_Friend };
            FriendGroup Elite_Group = new FriendGroup { GroupID = 6, Name = "Elite", Friends = Elite_Friend };

            GroupFriendList gf = new GroupFriendList();
            gf.FriendGroups.Add(NIIT_Group);
            gf.FriendGroups.Add(Elite_Group);

            InitializeComponent();

            AddFriendModule.SetupData(Group);
            AddFriendModule.OnAddingFriend += new AddFriend.AddFriendModule.AddingFriend(AddFriendModule_OnAddingFriend);

            FriendListModule.SetupData(gf);
            FriendListModule.OnFriendItemClick += new vChat.FriendListModule.FriendItems(FriendListModule_OnFriendItemClick);
            FriendListModule.OnGroupItemClick += new vChat.FriendListModule.GroupItems(FriendListModule_OnGroupItemClick);
        }

        private void FriendListModule_OnGroupItemClick(vChat.GroupInfo e)
        {
            status.Text = (String.Format("GroupID: {0}, GroupName: {1}", e.ID, e.Name));
        }

        private void FriendListModule_OnFriendItemClick(vChat.FriendInfo e)
        {
            status.Text = status.Text = (String.Format("UserID: {0}, Username: {1}", e.ID, e.Name));
        }        

        private void AddFriendModule_OnAddingFriend(AddFriend.AddedInfo e)
        {
            status.Text = (String.Format("Value: {0}, Group: {1}, {2}", e.Value, e.Group.Name, e.Group.GroupID));
        }
    }
}
