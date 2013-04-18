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

namespace vChat.Module.Chat
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        private BackgroundWorker _SendWorker = new BackgroundWorker();

        private Queue<SendMetadata> _SendQueue = new Queue<SendMetadata>();

        private Client _Client;
        private string TargetUser { get; private set; }

        public Chat(string targetUser)
        {
            InitializeComponent();
            this.TargetUser = targetUser;
            _Client = this.Get<Client>();
            _SendWorker.DoWork += new DoWorkEventHandler(_SendWorker_DoWork);
        }

        void _SendWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SendMetadata data = e.Argument as SendMetadata;
            _Client.SendCommand(this, data.Type, data.ToUser, data.Data);
            while (_SendQueue.Count > 0)
            {
                data = _SendQueue.Dequeue();
                _Client.SendCommand(this, data.Type, data.ToUser, data.Data);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SendMetadata data = new SendMetadata(CommandType.Chat, TargetUser, ExportXaml(MessageInput));
            if (_SendWorker.IsBusy)
            {
                _SendQueue.Enqueue(data);
            }
            else
            {
                _SendWorker.RunWorkerAsync(data);
            }
        }

        private string ExportXaml(RichTextBox rt)
        {
            TextRange range = new TextRange(rt.Document.ContentStart, rt.Document.ContentEnd);
            using (MemoryStream stream = new MemoryStream())
            {
                range.Save(stream, DataFormats.Xaml);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private FlowDocument ImportXaml(string xaml)
        {
            StringReader stringReader = new StringReader(xaml);
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                Section sec = System.Windows.Markup.XamlReader.Load(xmlReader) as Section;
                FlowDocument doc = new FlowDocument();
                while (sec.Blocks.Count > 0)
                    doc.Blocks.Add(sec.Blocks.FirstBlock);
                return doc;
            }
        }
    }

    public class SendMetadata
    {
        public CommandType Type { get; private set; }
        public string ToUser { get; private set; }
        public object[] Data { get; private set; }
        public SendMetadata(CommandType type, string toUser, params object[] data)
        {
            this.Type = type;
            this.ToUser = toUser;
            this.Data = data;
        }
    }
}
