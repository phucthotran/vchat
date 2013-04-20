using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Data;
using System.Windows;
using System.Windows.Documents;

namespace vChat.View.Windows
{
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private delegate void ChatListenerDelegate(CommandResponse res);

        private void ChatListener(CommandResponse res)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                ChatWindow chatWindow = null;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(ChatWindow) && ((ChatWindow)window).TargetUser == res.TargetUser)
                    {
                        chatWindow = window as ChatWindow;
                        break;
                    }
                }
                if (chatWindow != null)
                {
                    chatWindow.Focus();
                    chatWindow.ReceiveMessage(res.TargetUser, res.Params[0] as string);
                }
                else
                {
                    chatWindow = new ChatWindow(res.TargetUser);
                    chatWindow.ReceiveMessage(res.TargetUser, res.Params[0] as string);
                    chatWindow.Show();
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new ChatListenerDelegate(ChatListener), res);
            }
        }
    }
}
