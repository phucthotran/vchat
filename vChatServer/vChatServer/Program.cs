using System;
using System.Collections.Generic;
using System.Linq;
using Core.Server;
using Core.Data;

namespace vChatServer
{
    public class Program
    {
        public static Server _SERVER { get; set; }
        private Controller _invoker = new Controller();
        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.Run();
            Console.ReadLine();
        }

        public void Run()
        {
            _SERVER = new Server();
            _SERVER.Invoker = _invoker;
            _SERVER.OnStartSuccess += new Server.StartSuccess(server_OnStartSuccess);
            _SERVER.OnStartFailed += new Server.StartFailed(server_OnStartFailed);
            _SERVER.OnStop += new Server.StopHandler(server_OnStop);
            _SERVER.OnCritical += new Server.Critical(server_OnCritical);
            _SERVER.OnClientReceived += new Server.ClientReceived(server_OnClientReceived);
            _SERVER.OnClientDisconnected += new Server.ClientDisconnected(server_OnClientDisconnected);
            _SERVER.Start();
        }

        void server_OnClientDisconnected(Core.Server.ClientManagement.Client client)
        {
            _SERVER.Logging(String.Format("{0}({1}) da ngat ket noi.", client.Name, client.Socket.RemoteEndPoint));
        }

        void server_OnClientReceived(Core.Server.ClientManagement.Client client, Core.Data.Command cmd)
        {
            if (cmd.Type == CommandType.LogIn)
            {
                _SERVER.Logging(String.Format("{0}({1}) da ket noi den server.", client.Name, client.Socket.RemoteEndPoint));
            }
            else
            {
                _SERVER.Logging(String.Format("{0}({1}) da gui: {2}", client.Name, client.Socket.RemoteEndPoint, cmd.ToString()));
            }
        }

        void server_OnCritical(string message)
        {
            _SERVER.Logging(message);
        }

        void server_OnStop()
        {
            _SERVER.Logging("Server da dung lai.");
        }

        void server_OnStartFailed()
        {
            _SERVER.Logging("Da co loi xay ra khi khoi dong server.");
        }

        void server_OnStartSuccess()
        {
            _SERVER.Logging("Server da khoi dong thanh cong.");
        }

    }
}
