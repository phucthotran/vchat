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
using vChatClient;
using vChat.Model.Entities;
using vChat.Model;
using vChat.Module.Login;
using vChat.Module.SignUp;
using vChat.Module.FriendList;
using vChat.Module.AddFriend;
using Core.Client;
using Core.Data;
using System.ComponentModel;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private Login _loginModule;
        private SignUp _signUpModule;
        private FriendsList _friendListModule;

        public MainWindow()
        {
            InitializeComponent();
            this.InitTheme();
            InitLoginModule();
            InitClientListener();
            this.Width = 300;
            this.MinWidth = 300;
            this.Height = SystemParameters.WorkArea.Height;
        }

        private void InitClientListener()
        {
            Client client = this.Get<Client>();
            client.CommandBinding(CommandType.Chat, ChatListener);
        }

        private void InitLoginModule()
        {
            LogOut.Visibility = System.Windows.Visibility.Collapsed;
            _loginModule = Grid.LoadModule<Login>(new Login.LoginSuccessHandler(Login_OnLoginSuccess));
            _loginModule.OnLoginFailed += new Login.LoginFailedHandler(Login_OnLoginFailed);
            _loginModule.OnSignUpClicked += new Login.SignUpClickHandler(Login_OnSignUpClicked);            
        }

        private void InitFriendsListModule(int UserID)
        {
            _friendListModule = Grid.LoadModule<FriendsList>();
            _friendListModule.SetupData(UserID);
            _friendListModule.OnFriendDoubleClick += new FriendsList.MouseEventHandler(FriendList_OnFriendDoubleClick);

            vChat.Module.Upload.UploadImage UploadImageModule = new vChat.Module.Upload.UploadImage();
            UploadImageModule.UploadFor(UserID);

            MahApps.Metro.Controls.MetroWindow w = new MahApps.Metro.Controls.MetroWindow();
            w.InitTheme();
            w.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            w.Content = UploadImageModule;
            w.Show();
        }

        private void FriendList_OnFriendDoubleClick(object sender, FriendArgs e)
        {            
            new ChatWindow(e.Username).Show();
        }

        private void Login_OnLoginSuccess(Users UserLogged)
        {
            LogOut.Visibility = System.Windows.Visibility.Visible;
            InitFriendsListModule(UserLogged.UserID);          
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
            _signUpModule.OnLoginClicked += new SignUp.LoginClickHandler(delegate
            {
                InitLoginModule();
            });
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Get<Client>().Disconnect();
            Cookie.Instance.Unset("user", "pass", "expire");
            Cookie.Instance.Save();
            InitLoginModule();
            LogOut.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
