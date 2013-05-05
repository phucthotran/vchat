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
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Net;

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

        private string _TargetUser { get; set; }
        private bool? _recentIsSelf = null;
        private List<Message> _messagesAppended = new List<Message>();
        private int _timeType = 0;
        private ArrayList _messageSended = new ArrayList();
        private int _messageSendedIndex = 0;

        public Chat(string targetUser)
        {
            this._TargetUser = targetUser;
            this.MinHeight = 250;
            this.MinWidth = 400;
            InitializeComponent();
            MessageView.Document.Blocks.Remove(MessageView.Document.Blocks.FirstBlock);
            ChatToolBar.SendFileEvent += new ViewParts.ChatToolBar.SendFileHandler(SendFile_Handler);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Document.Blocks.Count > 0 && ((Paragraph)MessageInput.Document.Blocks.FirstBlock).Inlines.Count > 0)
            {
                _messageSended.Add(XamlWriter.Save(MessageInput.Document));
                _messageSendedIndex = 0;
                OnSendMessage(XamlWriter.Save(MessageInput.Document));
                Message message = new Message();
                message.Init(this.Get<Client>().Name, true, MessageInput.Document);
                message.ReceivedTimeType = (ReceivedTimeType)_timeType;
                if (_recentIsSelf == false || _recentIsSelf == null)
                {
                    _recentIsSelf = true;
                    message.IsDisplayUser = true;
                }
                _messagesAppended.Add(message);
                MessageView.Document.Blocks.AddRange(message);
                MessageView.ScrollToEnd();
            }
        }

        public FlowDocument LoadXaml(string xamls)
        {
            StringReader stringReader = new StringReader(xamls);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (FlowDocument)XamlReader.Load(xmlReader);
        }

        private void SendFile_Handler(FileSending fileSending)
        {
            OnSendFile(fileSending);
        }

        private void MessageView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F2 && e.Key != Key.Tab)
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                _timeType++;
                if (_timeType > 2)
                    _timeType = 0;
                foreach (Message message in _messagesAppended)
                {
                    message.ReceivedTimeType = (ReceivedTimeType)_timeType;
                }
            }
        }

        private Task keyDownTask;

        private void keyDownTasker(object key)
        {
            if (MessageInput.Document.CheckAccess())
            {
                switch ((Key)key)
                {
                    case Key.Up:
                        MessageInput.Document.Blocks.Clear();
                        if (_messageSendedIndex < _messageSended.Count)
                        {
                            _messageSendedIndex++;
                            MessageInput.Document.Blocks.AddRange(LoadXaml(_messageSended[_messageSended.Count - _messageSendedIndex].ToString()).Blocks.ToList());
                        }
                        break;
                    case Key.Down:
                        MessageInput.Document.Blocks.Clear();
                        if (_messageSendedIndex > 1)
                        {
                            _messageSendedIndex--;
                            MessageInput.Document.Blocks.AddRange(LoadXaml(_messageSended[_messageSended.Count - _messageSendedIndex].ToString()).Blocks.ToList());
                        }
                        break;
                }
                MessageInput.Selection.Select(MessageInput.Document.ContentEnd, MessageInput.Document.ContentEnd);
            }
            else
            {
                MessageInput.Document.Dispatcher.Invoke(new Action(() => { keyDownTasker(key); }));
            }
        }

        private void MessageInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownTask == null || keyDownTask.IsCompleted)
            {
                keyDownTask = Task.Factory.StartNew(keyDownTasker, e.Key);
            }
            else
            {
                keyDownTask = keyDownTask.ContinueWith(t => { keyDownTasker(e.Key); });
            }
        }
    }
}
