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

        public string TargetUser { get; private set; }
        public EndPoint ep { get; set; }
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
                _Client.SendCommand(new Command(CommandType.SendFileAccept, targetUser, new CommandMetadata(_Client.Name, id)));
            });
            _SendFilePanel.OnSendFileRejected += new SendFilePanel.SendFileRejectedHandler(id =>
            {
                _Client.SendCommand(new Command(CommandType.SendFileReject, targetUser, new CommandMetadata(_Client.Name, id)));
            });
            _ChatModule.OnSendMessage += new Chat.SendMessageHandler(message =>
            {
                _Client.SendCommand(new Command(CommandType.Chat, targetUser, new CommandMetadata(_Client.Name, message, _Client.Socket.LocalEndPoint)));
            });
            _ChatModule.OnSendFile += new Chat.SendFileHandler(fileSending =>
            {
                _SendFilePanel.Append(TargetUser, true, fileSending);
                _Client.SendCommand(new Command(CommandType.SendFileRequest, targetUser, new CommandMetadata(_Client.Name, fileSending)));
            });
        }

        public void ReceiveMessage(string fromUser, string message)
        {
            _ChatModule.ReceiveMessage(fromUser, message);
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
                    _Client.SendCommand(new Command(CommandType.SendFileProcess, TargetUser, new CommandMetadata(_Client.Name, id, data, chunk)));
                    sended += count;
                }
            }
            _Client.SendCommand(new Command(CommandType.SendFileSuccess, TargetUser, new CommandMetadata(_Client.Name, id)));
        }

        public void IsRejectFile(string id)
        {
            FileProcess fileProcess = _SendFilePanel.Find(id);
            fileProcess.Cancel(false);
            // do do do
        }
    }
}
