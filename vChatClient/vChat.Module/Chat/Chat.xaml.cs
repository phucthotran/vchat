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
using System.ComponentModel;
using Core.Client;
using Core.Data;
using System.IO;
using System.Xml;
using vChat.Control;
using System.Windows.Markup;

namespace vChat.Module.Chat
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        public delegate void SendMessageHandler(string blocks);
        public event SendMessageHandler OnSendMessage = delegate { };
        private string TargetUser { get; set; }

        public Chat(string targetUser)
        {
            this.TargetUser = targetUser;
            InitializeComponent();
            MessageView.Document.Blocks.Remove(MessageView.Document.Blocks.FirstBlock);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Block block = MessageInput.Document.Blocks.FirstBlock;
            if (block != null)
            {
                MessageView.Document.Blocks.Add(block);
                OnSendMessage(XamlWriter.Save(block));
                Paragraph p = MessageView.Document.Blocks.LastBlock as Paragraph;
                p.Inlines.InsertBefore(p.Inlines.FirstInline, new UserDefined().SetText(this.Get<Client>().Name, true));
            }
        }
    }
}
