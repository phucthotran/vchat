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
using Microsoft.Win32;
using System.Windows.Interop;
using Core.Data;

namespace vChat.Module.Chat.SendFilePanel
{
    /// <summary>
    /// Interaction logic for FileProcess.xaml
    /// </summary>
    public partial class FileProcess : UserControl
    {
        public delegate void SendFileAcceptedHandler(SaveFileDialog fileDialog, string id, long length);
        public event SendFileAcceptedHandler OnSendFileAccepted = delegate { };

        public delegate void SendFileRejectedHandler(string id);
        public event SendFileRejectedHandler OnSendFileRejected = delegate { };

        public string ID { get; private set; }

        private bool _IsSelf;
        public bool IsSelf
        {
            get { return _IsSelf; }
            private set
            {
                _IsSelf = value;
                if (value)
                {
                    IsnotSelfAction.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    IsSelfAction.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private string _TargetUser = "Anonymous";
        public string TargetUser
        {
            get { return _TargetUser; }
            private set
            {
                _TargetUser = value;
                if (IsSelf)
                {
                    this.Receiver.Text = value;
                    this.Receiver.Foreground = Brushes.Blue;
                }
                else
                {
                    this.Sender.Text = value;
                    this.Sender.Foreground = Brushes.Blue;
                }
            }
        }

        public string FilePath { get; private set; }
        public long FileLength { get; private set; }

        public FileProcess(string User, bool IsSelf, FileSending fileSending)
        {
            InitializeComponent();
            this.IsSelf = IsSelf;
            this.TargetUser = User;
            this.FileIcon.Source = Imaging.CreateBitmapSourceFromHIcon(
                            fileSending.ShellIcon.Handle,
                            new Int32Rect(0, 0, fileSending.ShellIcon.Width, fileSending.ShellIcon.Height),
                            BitmapSizeOptions.FromEmptyOptions());
            this.FileName.Text = fileSending.FullName;
            this.FileSize.Text = fileSending.SizeName;
            this.FilePath = fileSending.FullPath;
            this.FileLength = fileSending.Bytes;
            this.ID = fileSending.ID;
        }

        private void btAccept_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "All files (*.*)|*.*";
            fileDialog.FileName = this.FileName.Text;
            bool? result = fileDialog.ShowDialog();
            if (result.Value)
            {
                OnSendFileAccepted(fileDialog, ID, FileLength);
            }
        }

        private void btReject_Click(object sender, RoutedEventArgs e)
        {
            Cancel(true);
            OnSendFileRejected(ID);
        }

        public void Cancel(bool selfCancel)
        {
            IsSelfAction.Visibility = System.Windows.Visibility.Collapsed;
            IsnotSelfAction.Visibility = System.Windows.Visibility.Collapsed;
            if (selfCancel)
            {
                Message.Text = "Bạn đã hủy tiến trình gửi file.";
            }
            else
            {
                Message.Text = TargetUser + " đã huỷ tiến trình gửi file.";
            }
            Message.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
