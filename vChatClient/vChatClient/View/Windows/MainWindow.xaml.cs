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
using System.Windows.Shapes;
using System.IO;
using Elysium;
using vChatClient;
using vChat.Model.Entities;
using vChat.Model;
using vChat.Module.Login;
using vChat.Module.SignUp;
using vChat.Module.FriendList;
using vChat.Module.AddFriend;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        private int UserID;
        private Login _loginModule;
        private SignUp _signUpModule;
        private FriendsList _friendListModule;
        private AddFriend _addFriendModule;

        public MainWindow()
        {
            InitializeComponent();
            InitLoginModule();            
        }

        private void InitLoginModule()
        {
            _loginModule = Grid.LoadModule<Login>();
            _loginModule.OnLoginSuccess += new Login.LoginSuccessHandler(Login_OnLoginSuccess);
            _loginModule.OnLoginFailed += new Login.LoginFailedHandler(Login_OnLoginFailed);
            _loginModule.OnSignUpClicked += new Login.SignUpClickHandler(Login_OnSignUpClicked);            
        }

        private void InitFriendsListModule()
        {
            _friendListModule = Grid.LoadModule<FriendsList>();
            //_friendListModule.OnGroupItemClick += new FriendsList.GroupItems(FriendList_OnGroupItemClick);
            //_friendListModule.OnFriendItemClick += new FriendsList.FriendItemHandler(FriendList_OnFriendItemClick);
        }

        private void InitAddFriendModule()
        {
            _addFriendModule = Grid.LoadModule<AddFriend>();
            _addFriendModule.OnAddingFriend += new AddFriend.AddingFriend(AddFriend_OnAddingFriend);
        }

        private void Login_OnLoginSuccess(Users UserLogged)
        {
            UserID = UserLogged.UserID;
            _friendListModule = Grid.LoadModule<FriendsList>();
            _friendListModule.SetupData(UserLogged.UserID);
            /*
            Grid.Children.Clear();

            Users u = ((UserServiceClient)App.Current.FindResource("UserServiceClient")).FindName(userLogged);
            UserID = u.UserID;
            GroupFriendList f = ((UserServiceClient)App.Current.FindResource("UserServiceClient")).FriendList(u.UserID);

            AddFriendModule addFriendModule = new AddFriendModule();
            addFriendModule.SetupData(f.FriendGroups);
            addFriendModule.OnAddingFriend += new AddFriendModule.AddingFriend(addFriendModule_OnAddingFriend);

            FriendListModule friendListModule = new FriendListModule();
            friendListModule.SetupData(f);
            friendListModule.OnFriendItemClick += new FriendListModule.FriendItems(friendListModule_OnFriendItemClick);
            friendListModule.OnGroupItemClick += new FriendListModule.GroupItems(friendListModule_OnGroupItemClick);

            StackPanel Container = new StackPanel();
            Container.Children.Add(friendListModule);
            Container.Children.Add(addFriendModule);

            Grid.Children.Add(Container);
            */
        }

        private void FriendList_OnGroupItemClick(GroupInfo e)
        {
            MessageBox.Show(String.Format("ID: {0}, Name: {1}", e.ID, e.Name));
        }

        void AddFriend_OnAddingFriend(AddedInfo e)
        {
            //Users u = App.UserService.FindName(e.Value);
            //MethodInvokeResult result = App.UserService.AddFriend(UserID, e.Value, e.Group.GroupID);

            //if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
            //{
            //    MessageBox.Show(result.Message);
            //}
            //else
            //{
            //    MessageBox.Show(result.Message);
            //}
        }

        private void FriendList_OnFriendItemClick(FriendInfo e)
        {
            MessageBox.Show(String.Format("ID: {0}, Name: {1}", e.ID, e.Name));
        }

        private void Login_OnLoginFailed()
        {
            MessageBox.Show("fail");
        }

        private void Login_OnSignUpClicked()
        {
            _signUpModule = Grid.LoadModule<SignUp>();
            _signUpModule.OnSignUpSuccess += new SignUp.SignUpSuccessHandler(delegate
            {
                InitLoginModule();
            });
        }
    }
}
