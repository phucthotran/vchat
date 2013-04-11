using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChatClient;
using vChat.UserService;
using Core.Data;

namespace vChat.Controllers
{
    public class LoginController
    {
        public bool Login(string user, string pass)
        {
            if (!App.Client.Socket.Connected)
                App.Client.Connect();
            MethodInvokeResult result = App.UserService.Login(user, pass);
            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
            {
                App.Client.SendCommand(null, CommandType.LogIn, "SERVER", user);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
