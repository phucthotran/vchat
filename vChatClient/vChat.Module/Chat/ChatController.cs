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
using System.Windows;

namespace vChat.Module.Chat
{
    public partial class Chat : UserControl
    {
        MessagePopup popup = new MessagePopup();
        public void ReceiveMessage(Message message)
        {
            if (_recentIsSelf == true || _recentIsSelf == null)
            {
                _recentIsSelf = false;
                message.IsDisplayUser = true;
            }
            else
            {
                message.IsDisplayUser = false;
            }
            message.ReceivedTimeType = (ReceivedTimeType)_timeType;
            SoundMessageIncome.Play();
            _messagesAppended.Add(message);
            MessageView.Document.Blocks.AddRange(message);
            MessageView.ScrollToEnd();
        }
    }
}
