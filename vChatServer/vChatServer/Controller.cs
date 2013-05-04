﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChatServer.vChatService;
using Core.Server.ClientManagement;
using Core.Data;
using Core.Server;

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

        [Invoke(CommandType.CheckIP)]
        public void GetClientIP(Client client, string user)
        {
            Client clientCheck = Program._SERVER.Clients.GetClient(user);
            if (clientCheck != null)
                Program._SERVER.SendCommand(new Command(CommandType.CheckIP, client.Name, new CommandMetadata("SERVER", clientCheck.Socket.RemoteEndPoint)), client);
        }
    }
}
