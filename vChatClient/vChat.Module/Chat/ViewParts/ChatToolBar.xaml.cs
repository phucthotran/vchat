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
using System.IO;
using System.Drawing;
using Core.Data;

namespace vChat.Module.Chat.ViewParts
{
    /// <summary>
    /// Interaction logic for ChatToolBar.xaml
    /// </summary>
    public partial class ChatToolBar : UserControl
    {
        public delegate void SendFileHandler(FileSending fileSending);
        public event SendFileHandler SendFileEvent = delegate { };
        public ChatToolBar()
        {
            InitializeComponent();
        }

        private void btSendFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? result = fileDialog.ShowDialog();
            if (result.Value)
            {
                SendFileEvent(new FileSending(fileDialog.SafeFileName, fileDialog.FileName, new FileInfo(fileDialog.FileName).Length));
            }
        }
    }
}
