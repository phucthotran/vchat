using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Data;
using System.Windows;
using System.Windows.Documents;
using System.Net;
using System.IO;
using vChat.Module.Chat.Parts;

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
                Message message = Message.LoadXamlContent(res.TargetUser, false, res.Params[0].ToString());       
                message.IsDisplayUser = true;
                FlowDocument messageDoc = new FlowDocument();
                messageDoc.Blocks.AddRange(message);
                string text = new TextRange(messageDoc.ContentStart, messageDoc.ContentEnd).Text;
                text = text.Replace("\r\n", " ");
                if (text.Length >= 30)
                    text = text.Replace(text.Substring(30, text.Length - 30), "...");
                if (chatWindow != null)
                {
                    chatWindow.Focus();
                    chatWindow.ReceiveMessage(message);
                }
                else
                {
                    chatWindow = new ChatWindow(res.TargetUser);
                    chatWindow.ReceiveMessage(message);
                    chatWindow.Show();
                }
                MessagePopup.Display(text, delegate
                {
                    chatWindow.BringToFront();
                }); 
                if (!chatWindow.IsActive)
                    chatWindow.FlashWindow();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { ChatListener(res); }));
            }
        }

        private delegate void RequestFileListenerDelegate(CommandResponse res);

        private void RequestFileListener(CommandResponse res)
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
                    chatWindow.IsRequestFile(res.TargetUser, res.Params[0]);
                }
                else
                {
                    chatWindow = new ChatWindow(res.TargetUser);
                    chatWindow.IsRequestFile(res.TargetUser, res.Params[0]);
                    chatWindow.Show();
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { RequestFileListener(res); }));
            }
        }

        private void AcceptFileListener(CommandResponse res)
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
                    chatWindow.IsAcceptFile(res.Params[0].ToString());
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { AcceptFileListener(res); }));
            }
        }

        private void RejectFileListener(CommandResponse res)
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
                    chatWindow.IsRejectFile(res.Params[0].ToString());
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { RejectFileListener(res); }));
            }
        }

        private void ProcessFileListener(CommandResponse res)
        {
            SendFile[res.Params[0].ToString()].Add((int)res.Params[2], (byte[])res.Params[1]);
        }

        private void SuccessFileListener(CommandResponse res)
        {
            SortedList<int, byte[]> SendFile = this.Get<Dictionary<string, SortedList<int, byte[]>>>("SendFile")[(string)res.Params[0]];
            byte[] fileDone = new byte[SendFile.ToArray().Sum(x => x.Value.Length)];
            int offset = 0;
            foreach (byte[] data in SendFile.ToArray().Select(x => x.Value))
            {
                Buffer.BlockCopy(data, 0, fileDone, offset, data.Length);
                offset += data.Length;
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(SendFilePath[res.Params[0].ToString()], FileMode.Create)))
            {
                writer.Write(fileDone);
                writer.Flush();
            }
        }

        private void CheckIPListener(CommandResponse res)
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
                    chatWindow.ClientEndPoint = res.Params[0] as EndPoint;
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { CheckIPListener(res); }));
            }
        }

        private void CheckOnlineListener(CommandResponse res)
        {
            if (_friendListModule.Dispatcher.CheckAccess())
            {
                /* 
                 * _friendListModule phải cung cấp method để lấy ra control chứa tên người dùng
                 * hoặc là method dùng để set online cho control đó                
                 */
                
                // lấy ra tên người dùng và trạng thái
                string user = res.TargetUser;
                bool isOnline = (bool)res.Params[0];

                // set online status
                // _friendListModule.SetStatus(user, isOnline);

                /*
                 * ...
                 */

                // cái này để popup ra ngoài, để dưới cùng
                if ((bool)res.Params[1])
                {
                    MessagePopup.Display(user + "đã online !!", delegate
                    {
                        ChatWindow chatWindow = new ChatWindow(user);
                        chatWindow.Show();
                        chatWindow.BringToFront();
                    });
                }
            }
            else
            {
                _friendListModule.Dispatcher.Invoke(new Action(() => { CheckOnlineListener(res); }));
            }
        }
    }
}
