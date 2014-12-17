using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net.Sockets;
using Core.Data;
using System.Threading.Tasks;
using System.IO;

namespace Core.Server.ClientManagement
{
    public class Client
    {
        private Task sendTasker;

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
        private CommandExecuter _Executer = new CommandExecuter();

        public Client(Socket socket)
        {
            this.Init(socket, null);
        }
        public Client(Socket socket, ConnectedHandler handler)
        {
            this.Init(socket, handler);
        }

        // Methods
        private void Init(Socket socket, ConnectedHandler handler)
        {
            this.Socket = socket;
            Socket.ReceiveBufferSize = 1024 * 1024 * 100;
            Socket.SendBufferSize = 1024 * 1024 * 100;
            this.OnConnected += handler;
            this.OnConnected(this);
            byte[] buffer = new byte[4];
            Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCommand), buffer);
        }

        private void receiveCommand(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = (byte[])ar.AsyncState;
                if (buffer.Length == 4)
                {
                    int bufferSize = BitConverter.ToInt32(buffer, 0);
                    buffer = new byte[bufferSize];
                }
                else
                {
                    Command cmd = buffer.ConvertTo<Command>();
                    if (cmd == null)
                        throw new Exception();
                    this.OnReceived(this, cmd);
                    buffer = new byte[4];
                }
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCommand), buffer);
            }
            catch (Exception ex)
            {
                this.OnCritical(ex.Message, this);
                this.OnDisconnected(this);
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
                sendTasker = sendTasker.ContinueWith(t => SendCommandProcess(cmd));
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

        public delegate void ConnectedHandler(Client client);

        public delegate void CriticalHandler(string message, Client client);

        public delegate void DisconnectedHandler(Client client);

        public delegate void ReceivedHandler(Client client, Command command);
        public event ConnectedHandler OnConnected = delegate { };
        public event CriticalHandler OnCritical = delegate { };
        public event DisconnectedHandler OnDisconnected = delegate { };
        public event ReceivedHandler OnReceived = delegate { };
    }
}