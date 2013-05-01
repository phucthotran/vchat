using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;
using System.Configuration;
using Core.Data;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Core.Client
{
    public class Client
    {
        public Socket Socket { get; private set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsConnected
        {
            get
            {
                return (Socket != null) ? Socket.Connected : false;
            }
        }
        private NetworkStream _Stream;
        private CommandExecuter _Executer = new CommandExecuter();
        private Task sendTasker;
        public IPAddress ServerIP { get; private set; }
        public int Port { get; private set; }
        public Client()
        {
            InitClient(IPAddress.Parse(ConfigurationManager.AppSettings["Server IP"]), Int32.Parse(ConfigurationManager.AppSettings["Port"]));
        }

        public Client(IPAddress serverIP, int port)
        {
            InitClient(serverIP, port);
        }

        private void InitClient(IPAddress serverIP, int port)
        {
            this.ServerIP = serverIP;
            this.Port = port;
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            Connect(false);
        }

        public void Connect(bool doThrow)
        {
            try
            {
                this.Socket.Connect(new IPEndPoint(ServerIP, Port));
                Socket.ReceiveBufferSize = 1024 * 1024 * 100;
                Socket.SendBufferSize = 1024 * 1024 * 100;
                this._Stream = new NetworkStream(this.Socket);
                this.OnConnected();
                byte[] buffer = new byte[4];
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCommand), buffer);
            }
            catch (Exception ex)
            {
                this.OnCritical(ex.StackTrace);
                this.OnConnectFailed();
                if (doThrow)
                    throw ex;
            }
        }

        public void Disconnect()
        {
            this.Socket.BeginDisconnect(false, ar =>
            {
                this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }, null);
        }

        public void CommandBinding(CommandType type, Action<CommandResponse> action)
        {
            _Executer.Set(type, action);
        }

        private void receiveCommand(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[])ar.AsyncState; 
                Socket.EndReceive(ar);
                if (buffer.Length == 4)
                {
                    int bufferSize = BitConverter.ToInt32(buffer, 0);
 
                    buffer = new byte[bufferSize];
                    Socket.Receive(buffer);
                }
           //     else
          //      {
                    Command cmd = buffer.ConvertTo<Command>();
                    _Executer[cmd.Type].DynamicInvoke(new CommandResponse(cmd));
                    buffer = new byte[4];
          //      }
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCommand), buffer);
            }
            catch (Exception e)
            {
                throw e;
                this.ID = -1;
                this.Name = "";
            }
        }
        public void SendCommand(Command cmd)
        {
            if (sendTasker == null || sendTasker.IsCompleted)
            {
                sendTasker = Task.Factory.StartNew(SendCommandProcess, cmd);
            }
            else
            {
                sendTasker = sendTasker.ContinueWith(t => { SendCommandProcess(cmd); });
            }
        }

        private void SendCommandProcess(object cmd)
        {
            byte[] buffer = new byte[4];
            byte[] cmdBuffer = ((Command)cmd).ToBytes();
            buffer = BitConverter.GetBytes(cmdBuffer.Length);
            this.Socket.Send(buffer);
            this.Socket.Send(cmdBuffer);
        }

        public delegate void ConnectedHandler();
        public event ConnectedHandler OnConnected = delegate { };

        public delegate void ConnectFailedHandler();
        public event ConnectFailedHandler OnConnectFailed = delegate { };

        public delegate void DisconnectedHandler();
        public event DisconnectedHandler OnDisconnected = delegate { };

        public delegate void Critical(string message);
        public event Critical OnCritical = delegate { };
    }
}
