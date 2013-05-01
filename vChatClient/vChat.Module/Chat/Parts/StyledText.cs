using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace vChat.Module.Chat.Parts
{
    public class StyledText : Message
    {
        public StyledText(string User, bool IsSelf, TextBlock messageBlock)
        {
            Init(User, IsSelf, messageBlock);
        }
    }
}
