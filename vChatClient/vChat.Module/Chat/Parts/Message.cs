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
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace vChat.Module.Chat.Parts
{
    public class Message : List<Paragraph>
    {
        private double _FontSizeUser = 14;
        private FontWeight _FontWeightUser = FontWeights.Bold;
        private Brush _ForegroundUser = Brushes.Black;

        private double _FontSizeMessage = 12;
        private FontWeight _FontWeightMessage = FontWeights.Regular;

        private bool _IsDisplayUser = false;
        public bool IsDisplayUser {
            get { return _IsDisplayUser; }
            set
            {
                if (value != _IsDisplayUser)
                {
                    _IsDisplayUser = value;
                    if (value)
                    {
                        Paragraph userPara = new Paragraph();
                        Run userRun = new Run(User);
                        userRun.FontSize = _FontSizeUser;
                        userRun.FontWeight = _FontWeightUser;
                        userRun.Foreground = _ForegroundUser;
                        userPara.Inlines.AddRange(new Inline[] { userRun, _AfterUsernameTimeRun, new Run(" :") });
                        this.Insert(0, userPara);
                    }
                }
            }
        }

        private DateTime _ReceivedTime;
        public DateTime ReceivedTime
        {
            get { return _ReceivedTime; }
            set
            {
                _ReceivedTime = value;
                string formatType = "dd/MM/yyyy hh:mm:ss tt";
                _AfterUsernameTimeRun.Tag = " (" + value.ToString(formatType) + ")";
                _AfterUsernameTimeRun.FontSize = _FontSizeUser;
                _AfterUsernameTimeRun.FontWeight = _FontWeightUser;
                _AfterUsernameTimeRun.Foreground = _ForegroundUser;
                _BeforeMessageTimeRun.Tag = "(" + value.ToString(formatType) + ") ";
                _BeforeMessageTimeRun.FontSize = _FontSizeMessage;
                _BeforeMessageTimeRun.FontWeight = _FontWeightMessage;
                _BeforeMessageTimeRun.Foreground = _ForegroundUser;
            }
        }

        private Run _AfterUsernameTimeRun = new Run();
        private Run _BeforeMessageTimeRun = new Run();
        private ReceivedTimeType _ReceivedTimeType = ReceivedTimeType.None;
        public ReceivedTimeType ReceivedTimeType
        {
            get { return _ReceivedTimeType; }
            set
            {
                _ReceivedTimeType = value;
                switch (_ReceivedTimeType)
                {
                    case Parts.ReceivedTimeType.None:
                        _BeforeMessageTimeRun.Text = "";
                        _AfterUsernameTimeRun.Text = "";
                        break;
                    case Parts.ReceivedTimeType.BeforeMessage:
                        _BeforeMessageTimeRun.Text = _BeforeMessageTimeRun.Tag.ToString();
                        break;
                    case Parts.ReceivedTimeType.AfterUsername:
                        _AfterUsernameTimeRun.Text = _AfterUsernameTimeRun.Tag.ToString();
                        break;
                    case Parts.ReceivedTimeType.All:
                        _BeforeMessageTimeRun.Text = _BeforeMessageTimeRun.Tag.ToString();
                        _AfterUsernameTimeRun.Text = _AfterUsernameTimeRun.Tag.ToString();
                        break;
                }
            }
        }

        private string _User = "Anonymous";
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }

        private bool _IsSelf = true;
        public bool IsSelf
        {
            get { return _IsSelf; }
            set
            {
                _IsSelf = value;
                if (!value)
                    _ForegroundUser = Brushes.DarkBlue;
            }
        }

        private FlowDocument _Content = new FlowDocument();
        public FlowDocument Content
        {
            get { return _Content; }
            private set { _Content = value; }
        }

        public Message() { }

        public void Init(string User, bool IsSelf, FlowDocument docContent)
        {
            this.IsSelf = IsSelf;
            this.User = User;
            this.ReceivedTime = DateTime.Now;
            Block[] tmpBlock = new Block[docContent.Blocks.Count];
            docContent.Blocks.ToArray().CopyTo(tmpBlock, 0);
            ((Paragraph)tmpBlock[0]).Inlines.InsertBefore(((Paragraph)tmpBlock[0]).Inlines.FirstInline, _BeforeMessageTimeRun);
            foreach (Block block in tmpBlock)
            {
                block.Foreground = _ForegroundUser;
            }
            this.AddRange(tmpBlock.Cast<Paragraph>());
        }

        public string GetXamlContent()
        {
        /*    string result = "";
            foreach (Paragraph para in Content)
            {
                result += XamlWriter.Save(para) + ";";
            }
            return result; */
            return XamlWriter.Save(Content);
        }

        public static Message LoadXamlContent(string User, bool IsSelf, string xamls)
        {
            Message thiz = new Message();
            StringReader stringReader = new StringReader(xamls);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            thiz.Init(User, IsSelf, (FlowDocument)XamlReader.Load(xmlReader));
            return thiz;
        }
    }

    public enum ReceivedTimeType
    {
        None,
        AfterUsername,
        All,
        BeforeMessage
    }
}
