using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using vChat.View.Windows;
using Core.Client;
using vChat.Service.UserService;

namespace vChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
      //      Elysium.Manager.Apply(this, Elysium.Theme.Light, Elysium.AccentBrushes.Blue, System.Windows.Media.Brushes.White);
            Resources.Add("UserServiceClient", new UserServiceClient());
            Resources.Add("Client", new Client());
            Resources.Add("SendFile", new Dictionary<string, SortedList<int, byte[]>>());
            Resources.Add("SendFilePath", new Dictionary<string, string>());
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
