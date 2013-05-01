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
using vChat.Module.Chat.Parts;
using vChat.Module.Chat.ViewParts;

namespace vChat.Module.Chat
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        public delegate void SendMessageHandler(string blocks);
        public event SendMessageHandler OnSendMessage = delegate { };

        public delegate void SendFileHandler(FileSending fileSending);
        public event SendFileHandler OnSendFile = delegate { };

        private string TargetUser { get; set; }

        public Chat(string targetUser)
        {
            this.TargetUser = targetUser;
            this.MinHeight = 250;
            this.MinWidth = 400;
            InitializeComponent();
            MessageView.Document.Blocks.Remove(MessageView.Document.Blocks.FirstBlock);
            ChatToolBar.SendFileEvent += new ViewParts.ChatToolBar.SendFileHandler(SendFile_Handler);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            TextBlock messageBlock = new TextBlock();
            messageBlock.Inlines.AddRange(((Paragraph)MessageInput.Document.Blocks.FirstBlock).Inlines.ToList());
            StyledText styledText = new StyledText(this.Get<Client>().Name, true, messageBlock);
            if (messageBlock.Inlines.Count > 0)
            {
                MessageView.Document.Blocks.Add(styledText);
                MessageView.ScrollToEnd();
                OnSendMessage(XamlWriter.Save(messageBlock));
            }
        }

        private void SendFile_Handler(FileSending fileSending)
        {
            OnSendFile(fileSending);
        }

        private void MessageView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Submit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SubmitIcon.Fill = Brushes.White;
        }

        private void Submit_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SubmitIcon.Fill = Brushes.Black;
        }

        private void SoundMessageIncome_MediaEnded(object sender, RoutedEventArgs e)
        {
            SoundMessageIncome.Stop();
        }
    }
}
