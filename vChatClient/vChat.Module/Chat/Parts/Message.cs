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

namespace vChat.Module.Chat.Parts
{
    public class Message : Paragraph
    {
        private string _User = "Anonymous";
        public string User
        {
            get {
                Run userDefined = this.Inlines.Where(i => i.Tag.ToString() == "User").FirstOrDefault() as Run;
                if (userDefined == null)
                    return _User;
                else
                    return userDefined.Text;
            }
            set
            {
                _User = value;
                Inline userInline = this.Inlines.Where(i => i.Tag.ToString() == "User").FirstOrDefault();
                if (userInline == null)
                {
                    Run userDefined = new Run(value+": ");
                    userDefined.Tag = "User";
                    userDefined.FontSize = 14;
                    userDefined.FontWeight = FontWeights.Bold;
                    if (_IsSelf)
                    {
                        userDefined.Foreground = Brushes.Black;
                    }
                    else
                    {
                        userDefined.Foreground = Brushes.DarkBlue;
                    }
                    if (this.Inlines.Count == 0)
                        this.Inlines.Add(userDefined);
                    else
                        this.Inlines.InsertBefore(this.Inlines.FirstInline, userDefined);
                }
                else
                {
                    ((Run)this.Inlines.FirstInline).Text = value+": ";
                }
            }
        }

        private bool _IsSelf = true;
        public bool IsSelf
        {
            get { return _IsSelf; }
            set { _IsSelf = value; }
        }

        private UIElement _Content;
        public UIElement Content
        {
            get { return ((InlineUIContainer)this.Inlines.Where(i => i.GetType() == typeof(InlineUIContainer)).FirstOrDefault()).Child; }
            set
            {
                _Content = value;
                InlineUIContainer contentBlock = (InlineUIContainer)this.Inlines.Where(i => i.GetType() == typeof(InlineUIContainer)).FirstOrDefault();
                if (contentBlock == null)
                    this.Inlines.Add(new InlineUIContainer(value));
                else
                    contentBlock.Child = value;
            }
        }

        static Message()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Message), new FrameworkPropertyMetadata(typeof(Message)));
        }

        protected void Init(string User, bool IsSelf)
        {
            Init(User, IsSelf, null);
        }

        protected void Init(string User, bool IsSelf, UIElement Content)
        {
            this.IsSelf = IsSelf;
            this.User = User;
            if (Content != null)
                this.Content = Content;
        }

        public string GetXaml()
        {
            return XamlWriter.Save(this);
        }

        public static Message LoadXaml(string User, bool IsSelf, string xaml)
        {
            Message thiz = new Message();
            StringReader stringReader = new StringReader(xaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            thiz.Init(User, IsSelf, (UIElement)XamlReader.Load(xmlReader));
            return thiz;
        }
    }
}
