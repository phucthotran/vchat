using System;
using System.Collections.Generic;
using System.Linq;
using Core.Server;
using Core.Data;

namespace vChatServer
{
    public class Program
    {
        private Server _server;
        private Controller _invoker = new Controller();
        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.Run();
            Console.ReadLine();
        }

        public void Run()
        {
            _server = new Server();
            _server.Invoker = _invoker;
            _server.OnStartSuccess += new Server.StartSuccess(server_OnStartSuccess);
            _server.OnStartFailed += new Server.StartFailed(server_OnStartFailed);
            _server.OnStop += new Server.StopHandler(server_OnStop);
            _server.OnCritical += new Server.Critical(server_OnCritical);
            _server.OnClientReceived += new Server.ClientReceived(server_OnClientReceived);
            _server.OnClientDisconnected += new Server.ClientDisconnected(server_OnClientDisconnected);
            _server.Start();
        }

        void server_OnClientDisconnected(Core.Server.ClientManagement.Client client)
        {
            _server.Logging(String.Format("{0}({1}) da ngat ket noi.", client.Name, client.Socket.RemoteEndPoint));
        }

        void server_OnClientReceived(Core.Server.ClientManagement.Client client, Core.Data.Command cmd)
        {
            if (cmd.Type == CommandType.LogIn)
            {
                _server.Logging(String.Format("{0}({1}) da ket noi den server.", client.Name, client.Socket.RemoteEndPoint));
            }
            else
            {
                _server.Logging(String.Format("{0}({1}) da gui: {2}", client.Name, client.Socket.RemoteEndPoint, cmd.ToString()));
            }
        }

        void server_OnCritical(string message)
        {
            _server.Logging(message);
        }

        void server_OnStop()
        {
            _server.Logging("Server da dung lai.");
        }

        void server_OnStartFailed()
        {
            _server.Logging("Da co loi xay ra khi khoi dong server.");
        }

        void server_OnStartSuccess()
        {
            _server.Logging("Server da khoi dong thanh cong.");
        }

    }
}
