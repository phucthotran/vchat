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
using vChat.View.Controls;
using System.IO;
using Elysium;
using vChatClient;
using FriendList;
using vChat.Model.Entities;
using vChat.Model;
using AddFriend;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int UserID;

        private void Login_OnLoginSuccess(string userLogged)
        {
            Grid.Children.Clear();

            Users u = App.UserService.FindName(userLogged);
            UserID = u.UserID;
            GroupFriendList f = App.UserService.FriendList(u.UserID);

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
        }

        private void friendListModule_OnGroupItemClick(GroupInfo e)
        {
            MessageBox.Show(String.Format("ID: {0}, Name: {1}", e.ID, e.Name));
        }

        void addFriendModule_OnAddingFriend(AddedInfo e)
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

        private void friendListModule_OnFriendItemClick(FriendInfo e)
        {
            MessageBox.Show(String.Format("ID: {0}, Name: {1}", e.ID, e.Name));
        }

        private void Login_OnLoginFailed()
        {
            MessageBox.Show("fail");
        }

        private void Login_OnSignUpClicked()
        {
            Grid.Children.Remove(LoginUC);
            SignUp signUpUC = new SignUp();
            signUpUC.OnSignUpSuccess += new SignUp.SignUpSuccessHandler(delegate
            {
                Grid.Children.Remove(signUpUC);
                Grid.Children.Add(LoginUC);
            });
            Grid.Children.Add(signUpUC);
        }

        private static readonly string Windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        private static readonly string SegoeUI = Windows + @"\Fonts\SegoeUI.ttf";
        private static readonly string Verdana = Windows + @"\Fonts\Verdana.ttf";

        private void ThemeGlyphInitialized(object sender, EventArgs e)
        {
            ThemeGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void AccentGlyphInitialized(object sender, EventArgs e)
        {
            AccentGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void ContrastGlyphInitialized(object sender, EventArgs e)
        {
            ContrastGlyph.FontUri = new Uri(File.Exists(SegoeUI) ? SegoeUI : Verdana);
        }

        private void LightClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Light);
        }

        private void DarkClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(Theme.Dark);
        }

        private void AccentClick(object sender, RoutedEventArgs e)
        {
            var item = e.Source as MenuItem;
            if (item != null)
            {
                var accentBrush = (SolidColorBrush)((Rectangle)item.Icon).Fill;
                Application.Current.Apply(accentBrush, null);
            }
        }

        private void WhiteClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.White);
        }

        private void BlackClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Apply(null, Brushes.Black);
        }
    }
}
