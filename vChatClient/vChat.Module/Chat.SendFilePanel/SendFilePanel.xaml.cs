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
using Core.Client;
using Core.Data;
using Microsoft.Win32;

namespace vChat.Module.Chat.SendFilePanel
{
    /// <summary>
    /// Interaction logic for SendFilePanel.xaml
    /// </summary>
    public partial class SendFilePanel : UserControl
    {
        public delegate void SendFileAcceptedHandler(SaveFileDialog fileDialog, string id, long length);
        public event SendFileAcceptedHandler OnSendFileAccepted = delegate { };

        public delegate void SendFileRejectedHandler(string id);
        public event SendFileRejectedHandler OnSendFileRejected = delegate { };

        public SendFilePanel()
        {
            InitializeComponent();
        }

        public void Append(string target, bool isSelf, FileSending fileSending)
        {
            FileProcess fileProcess = new FileProcess(target, isSelf, fileSending);
            this.FilePanel.Children.Add(fileProcess);
            fileProcess.OnSendFileAccepted += new FileProcess.SendFileAcceptedHandler((fileDialog, id, length) =>
            {
                OnSendFileAccepted(fileDialog, id, length);
            });
            fileProcess.OnSendFileRejected += new FileProcess.SendFileRejectedHandler(id =>
            {
                OnSendFileRejected(id);
            });
        }

        public FileProcess Find(string id)
        {
            return this.FilePanel.Children.Cast<FileProcess>().Where(e => e.ID.Equals(id)).ElementAt(0);
        }
    }
}
