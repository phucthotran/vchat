using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using vChat.View.Windows;
using Core.Client;
using vChat.UserService;

namespace vChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserServiceClient UserService;
        public static Client Client;
        public static MainWindow MainView;
        public static string User;
        void App_Startup(object sender, StartupEventArgs e)
        {
            Elysium.Manager.Apply(this, Elysium.Theme.Light, Elysium.AccentBrushes.Blue, System.Windows.Media.Brushes.White);
            UserService = new UserServiceClient();
            Client = new Client();
            User = "";
            MainWindow mainWindow = new MainWindow();
            MainView = mainWindow;
            mainWindow.Show();
        }
    }
}
