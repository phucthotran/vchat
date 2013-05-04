using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChatServer.vChatService;
using Core.Server.ClientManagement;
using Core.Data;

namespace vChatServer
{
    public class Controller
    {
        private UserServicesClient _userServices = new UserServicesClient();
        public Controller()
        {
        }

        [Invoke(CommandType.LogIn)]
        public void UserLogIn(Client client, string user)
        {
            client.Name = user;
        }
    }
}
