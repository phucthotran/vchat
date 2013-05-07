using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Server.ClientManagement;
using Core.Data;
using Core.Server;

namespace vChatServer
{
    public class Controller
    {
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
                Program._SERVER.SendCommand(new Command(CommandType.CheckIP, client.Name, new CommandMetadata(user, clientCheck.Socket.RemoteEndPoint)), client);
        }

        [Invoke(CommandType.CheckOnline)]
        public void GetOnlineStatus(Client client, string user)
        {
            Client clientCheck = Program._SERVER.Clients.GetClient(user);
            bool isOnline = false;
            if (clientCheck != null)
            {
                isOnline = clientCheck.IsConnected;
                if (isOnline)
                {
                    Program._SERVER.SendCommand(new Command(CommandType.CheckOnline, user, new CommandMetadata(client.Name, true, true)), clientCheck);
                }
            }
            Program._SERVER.SendCommand(new Command(CommandType.CheckOnline, client.Name, new CommandMetadata(user, isOnline, false)), client);
        }
    }
}
