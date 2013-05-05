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
using System.Windows.Controls.Primitives;
using System.Net;

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
        public Dictionary<string, SortedList<int, byte[]>> SendFile;
        public Dictionary<string, string> SendFilePath;

        public MainWindow()
        {            
            this.Width = 300;
            this.MinWidth = 300;
            this.MaxWidth = 500;
            this.Height = SystemParameters.WorkArea.Height;
            this.SendFile = this.Get<Dictionary<string, SortedList<int, byte[]>>>("SendFile");
            this.SendFilePath = this.Get<Dictionary<string, string>>("SendFilePath");
            this.InitTheme();
            InitializeComponent();
            InitLoginModule();
            InitClientListener();
        }

        private void InitClientListener()
        {
            Client client = this.Get<Client>();
            client.CommandBinding(CommandType.Chat, ChatListener);
            client.CommandBinding(CommandType.SendFileAccept, AcceptFileListener);
            client.CommandBinding(CommandType.SendFileReject, RejectFileListener);
            client.CommandBinding(CommandType.SendFileRequest, RequestFileListener);
            client.CommandBinding(CommandType.SendFileProcess, ProcessFileListener);
            client.CommandBinding(CommandType.SendFileSuccess, SuccessFileListener);
            client.CommandBinding(CommandType.CheckIP, CheckIPListener);
            client.CommandBinding(CommandType.CheckOnline, CheckOnlineListener);
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
        }

        private void FriendList_OnFriendDoubleClick(object sender, FriendArgs e)
        {
            ChatWindow chatWindow = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(ChatWindow) && ((ChatWindow)window).TargetUser == e.Username)
                {
                    chatWindow = window as ChatWindow;
                    break;
                }
            }
            if (chatWindow == null)
            {
                chatWindow = new ChatWindow(e.Username);
                chatWindow.Show();
            }
            chatWindow.BringToFront();
            this.Get<Client>().SendCommand(CommandType.CheckIP, "SERVER", e.Username);
        }

        private void Login_OnLoginSuccess(Users UserLogged)
        {
            if (!this.IsFocused)
                this.FlashWindow();
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
            foreach (Window window in App.Current.Windows)
            {
                if (window.GetType() != typeof(MainWindow))
                {
                    window.CloseHandler(false);
                }
            }
            vChat.Module.Login.Cookie.Instance.Unset("user", "pass", "expire");
            vChat.Module.Login.Cookie.Instance.Save();
            InitLoginModule();
            LogOut.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            this.StopFlashingWindow();
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.CloseHandler(true, e);
        }
    }
}
