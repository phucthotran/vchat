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
using vChat.Module.Chat.SendFilePanel;
using vChat.Module.Chat.ViewParts;
using System.IO;
using System.Net;
using vChat.Module.Chat.Parts;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : MetroWindow
    {
        private Client _Client;
        private Chat _ChatModule;
        private SendFilePanel _SendFilePanel;
        private String _IpAddress;

        private EndPoint _ClientEndPoint;
        public EndPoint ClientEndPoint
        {
            get { return _ClientEndPoint; }
            set
            {
                _ClientEndPoint = value;
                _IpAddress = ((IPEndPoint)value).Address.ToString();
                VoIPModule.RemoteIPAddress = _IpAddress;
                VoIPModule.Init();
            }
        }

        public string TargetUser { get; private set; }
        public ChatWindow(string targetUser)
        {
            Init(targetUser);
        }

        public void Init(string targetUser)
        {
            InitializeComponent();
            this.Activate();
            this.Focus();
            this.Title = targetUser;
            this.Width = 400;
            this.Height = 300;
            this.InitTheme();
            _Client = this.Get<Client>();
            TargetUser = targetUser;
            _ChatModule = MainPanel.LoadModule<Chat>(targetUser);
            _SendFilePanel = FilePanel.LoadModule<SendFilePanel>();
            _SendFilePanel.OnSendFileAccepted += new SendFilePanel.SendFileAcceptedHandler((fileDialog, id, length) =>
            {
                Dictionary<string, SortedList<int, byte[]>> SendFile = this.Get<Dictionary<string, SortedList<int, byte[]>>>("SendFile");
                Dictionary<string, string> SendFilePath = this.Get<Dictionary<string, string>>("SendFilePath");
                SendFile[id] = new SortedList<int, byte[]>();
                SendFilePath[id] = fileDialog.FileName;
                _Client.SendCommand(CommandType.SendFileAccept, targetUser, id);
            });
            _SendFilePanel.OnSendFileRejected += new SendFilePanel.SendFileRejectedHandler(id =>
            {
                _Client.SendCommand(CommandType.SendFileReject, targetUser, id);
            });
            _ChatModule.OnSendMessage += new Chat.SendMessageHandler((chatID, message) =>
            {
                _Client.SendCommand(CommandType.Chat, targetUser, chatID, message);
            });
            _ChatModule.OnSendFile += new Chat.SendFileHandler(fileSending =>
            {
                _SendFilePanel.Append(TargetUser, true, fileSending);
                _Client.SendCommand(CommandType.SendFileRequest, targetUser, fileSending);
            });       
        }

        public void ReceiveMessage(Message message)
        {
            _ChatModule.ReceiveMessage(message);
        }

        public void IsRequestFile(string fromUser, object fileSending)
        {
            _SendFilePanel.Append(fromUser, false, (FileSending)fileSending);
        }

        public void IsAcceptFile(string id)
        {
            FileProcess fileProcess = _SendFilePanel.Find(id);
            int buffer = Convert.ToInt32(fileProcess.FileLength); // van chua tach file ra chunk dc
            int count = 0;
            long sended = 0;
            int chunk = 0;
            using (BinaryReader reader = new BinaryReader(File.Open(fileProcess.FilePath, FileMode.Open)))
            {
                while (sended < fileProcess.FileLength)
                {
                    chunk++;
                    byte[] data = new byte[buffer];
                    count = reader.Read(data, 0, buffer);
                    if (count < buffer)
                        data = data.Take(count).ToArray();
                    _Client.SendCommand(CommandType.SendFileProcess, TargetUser, id, data, chunk);
                    sended += count;
                }
            }
            _Client.SendCommand(CommandType.SendFileSuccess, TargetUser, id);
        }

        public void IsRejectFile(string id)
        {
            FileProcess fileProcess = _SendFilePanel.Find(id);
            fileProcess.Cancel(false);
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            this.StopFlashingWindow();
        }
    }
}
