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
using vChat.Service.UserService;
using vChat.Model.Entities;

namespace vChat.Module.Chat
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        public delegate void SendMessageHandler(int chatID, string blocks);
        public event SendMessageHandler OnSendMessage = delegate { };

        public delegate void SendFileHandler(FileSending fileSending);
        public event SendFileHandler OnSendFile = delegate { };

        private string _TargetUser { get; set; }
        private int _TargetID;
        private bool? _recentIsSelf = null;
        private List<Message> _messagesAppended = new List<Message>();
        private int _timeType = 0;
        private ArrayList _messageSended = new ArrayList();
        private int _messageSendedIndex = 0;
        private Conversation[] _previousChats;
        private int _currentIndex;
        private int _total;
        private int _buffer = 20;

        private UserServiceClient _userSerivce;
        private Client _client;

        public Chat(string targetUser)
        {
            _userSerivce = this.Get<UserServiceClient>();
            _client = this.Get<Client>();
            this._TargetUser = targetUser;
            _TargetID = _userSerivce.FindName(targetUser).UserID;
            _previousChats = _userSerivce.GetConversationBetween(_client.ID, _TargetID);
            _currentIndex = _previousChats.Length;
            this.MinHeight = 250;
            this.MinWidth = 400;
            InitializeComponent();
            MessageView.Document.Blocks.Remove(MessageView.Document.Blocks.FirstBlock);

            Paragraph para = new Paragraph();
            Run run = new Run("Nhấn vào để xem những tin nhắn gần đây (F3).");
            run.Foreground = Brushes.Blue;
            run.TextDecorations = TextDecorations.Underline;
            run.FontStyle = FontStyles.Italic;
            run.Cursor = Cursors.Hand;
            para.Inlines.Add(run);

            ChatToolBar.SendFileEvent += new ViewParts.ChatToolBar.SendFileHandler(SendFile_Handler);
        }

        private void loadPreviousMessage()
        {
            if (_currentIndex == 1)
            {
                MessageBox.Show("Không còn lược sử tin nhắn để hiển thị.");
                return;
            }
            if (_currentIndex < _buffer)
                _buffer = _currentIndex - 1;
            Conversation[] list = _userSerivce.GetConversationBetweenByRange(_client.ID, _TargetID, _currentIndex - _buffer, _currentIndex);
            generateMessage(list);
            _currentIndex -= _buffer;
        }

        private void generateMessage(Conversation[] list)
        {
            bool? recentSelf = null;
            List<Message> listMessage = new List<Message>();
            foreach (Conversation conversation in list.Reverse())
            {
                Message msg = Message.LoadXamlContent(conversation.SentBy.Username, (conversation.SentBy.UserID == _client.ID), conversation.Time, conversation.Message);
                if (msg.IsSelf == recentSelf)
                {
                    msg.IsDisplayUser = true;
                    recentSelf = !msg.IsSelf;
                }
                foreach (Paragraph para in msg.Reverse<Paragraph>())
                    MessageView.Document.Blocks.InsertAfter(MessageView.Document.Blocks.FirstBlock, para);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Document.Blocks.Count > 0 && ((Paragraph)MessageInput.Document.Blocks.FirstBlock).Inlines.Count > 0)
            {
                _messageSended.Add(XamlWriter.Save(MessageInput.Document));
                _messageSendedIndex = 0;

                string content = XamlWriter.Save(MessageInput.Document);
                int chatID = 0;
                this.Get<UserServiceClient>().SaveConversation(this.Get<Client>().ID, _TargetID, content, ref chatID);
                OnSendMessage(chatID, content);
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
            else if (e.Key == Key.F3)
            {
                loadPreviousMessage();
            }
        }

        private Task keyDownTask;

        private void keyDownTasker(object eventObj)
        {
            if (MessageInput.Document.CheckAccess())
            {
                KeyEventArgs e = eventObj as KeyEventArgs;
                switch (e.Key)
                {
                    case Key.Up:
                        MessageInput.Document.Blocks.Clear();
                        if (_messageSendedIndex < _messageSended.Count)
                        {
                            _messageSendedIndex++;
                            MessageInput.Document.Blocks.AddRange(LoadXaml(_messageSended[_messageSended.Count - _messageSendedIndex].ToString()).Blocks.ToList());
                            MessageInput.Selection.Select(MessageInput.Document.ContentEnd, MessageInput.Document.ContentEnd);
                        }
                        break;
                    case Key.Down:
                        MessageInput.Document.Blocks.Clear();
                        if (_messageSendedIndex > 1)
                        {
                            _messageSendedIndex--;
                            MessageInput.Document.Blocks.AddRange(LoadXaml(_messageSended[_messageSended.Count - _messageSendedIndex].ToString()).Blocks.ToList());
                            MessageInput.Selection.Select(MessageInput.Document.ContentEnd, MessageInput.Document.ContentEnd);
                        }
                        break;
                }
                if ((e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)) && e.Key == Key.A)
                    MessageInput.SelectAll();
            }
            else
            {
                MessageInput.Document.Dispatcher.Invoke(new Action(() => { keyDownTasker(eventObj); }));
            }
        }

        private void MessageInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownTask == null || keyDownTask.IsCompleted)
            {
                keyDownTask = Task.Factory.StartNew(keyDownTasker, e);
            }
            else
            {
                keyDownTask = keyDownTask.ContinueWith(t => { keyDownTasker(e); });
            }
        }
    }
}
