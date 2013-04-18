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
using System.Windows.Shapes;
using Core.Data;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public string TargetUser { get; set; }

        public ChatWindow()
        {
            InitializeComponent();
        }
        /*
        [Invoke(CommandType.Chat)]
        public void ReceiveMessage(string fromUser, string message)
        {
            Window wdChat = App.WindowList.Find(f => f.GetType() == typeof(WDChat) && ((WDChat)f).TargetUser == fromUser);
            if (wdChat == null)
            {
                Thread thread = new Thread(() =>
                {
                    wdChat = new WDChat(fromUser, message);
                    App.WindowList.Add(wdChat);
                    System.Windows.Threading.Dispatcher.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
            {
                ((WDChat)wdChat).ReceiveMessage(message);
            }
        }*/
    }
}
