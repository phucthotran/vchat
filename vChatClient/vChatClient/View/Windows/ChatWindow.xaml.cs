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
using Core.Data;
using vChat.Module.Chat;
using Core.Client;
using System.ComponentModel;
using MahApps.Metro.Controls;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : MetroWindow
    {
        private Client _Client;
        private Chat _ChatModule;

        public string TargetUser { get; private set; }

        public ChatWindow(string targetUser)
        {
            Init(targetUser);
        }

        public void Init(string targetUser)
        {
            InitializeComponent();
            this.Title = targetUser;
            this.InitTheme();
            _Client = this.Get<Client>();
            TargetUser = targetUser;
            _ChatModule = MainPanel.LoadModule<Chat>(targetUser);
            _ChatModule.OnSendMessage += new Chat.SendMessageHandler(message =>
            {
                _Client.SendCommand(new Command(CommandType.Chat, targetUser, new CommandMetadata(_Client.Name, message)));
            });
        }

        public void ReceiveMessage(string fromUser, string message)
        {
            _ChatModule.ReceiveMessage(fromUser, message);
        }
    }
}
