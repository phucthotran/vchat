using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Core.Data;
using System.Windows.Documents;
using vChat.Control;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using vChat.Module.Chat.Parts;

namespace vChat.Module.Chat
{
    public partial class Chat : UserControl
    {
        public void ReceiveMessage(string fromUser, string message)
        {
            MessageView.Document.Blocks.Add(StyledText.LoadXaml(fromUser, false, message));
            MessageView.ScrollToEnd();
        }
    }
}
