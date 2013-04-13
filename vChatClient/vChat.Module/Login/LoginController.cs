using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Data;
using System.Windows;
using Core.Client;
using System.Windows.Controls;
using vChat.Service.UserService;

namespace vChat.Module.Login
{
    public partial class Login : UserControl
    {
        public bool DoLogin(string user, string pass)
        {
            Client client = this.Get<Client>();
            if (!client.Socket.Connected)
                client.Connect();
            MethodInvokeResult result = this.Get<UserServiceClient>().Login(user, pass);
            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
            {
                client.SendCommand(null, CommandType.LogIn, "SERVER", user);
                client.User = user;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
