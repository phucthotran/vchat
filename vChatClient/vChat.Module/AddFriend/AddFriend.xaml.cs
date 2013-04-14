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

namespace vChat.Module.AddFriend
{
    /// <summary>
    /// Interaction logic for AddFriend.xaml
    /// </summary>
    public partial class AddFriend : UserControl
    {
        public delegate void AddingFriend(AddedInfo e);
        public event AddingFriend OnAddingFriend;

        public AddFriend()
        {
            InitializeComponent();
        }        

        public void SetupData(List<FriendGroup> Group)
        {
            lstFriend.ItemsSource = Group;
            lstFriend.DisplayMemberPath = "Name";
            lstFriend.SelectedValuePath = "GroupID";
            lstFriend.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FriendGroup SelectedGroup = (FriendGroup)lstFriend.SelectionBoxItem;

            AddedInfo AddedInfo = new AddedInfo
            {
                Value = txtFriendName.Text,
                Group = SelectedGroup
            };

            OnAddingFriend(AddedInfo);
        }       
    }
}
