using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net.Sockets;
using Core.Data;

namespace Core.Server.ClientManagement
{
    public class Client
    {
        private BackgroundWorker _ReceiveWorker = new BackgroundWorker();
        private BackgroundWorker _DoReceiveWorker = new BackgroundWorker();
        private BackgroundWorker _SendWorker = new BackgroundWorker();
        public string User { get; set; }
        public Socket Socket { get; private set; }
        public NetworkStream Stream { get; private set; }

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
            this.Stream = new NetworkStream(this.Socket);
            this.OnConnected += handler;
            this.OnConnected(this);
            this._ReceiveWorker.DoWork += new DoWorkEventHandler(this.receiveCommand);
            this._DoReceiveWorker.DoWork+=new DoWorkEventHandler(this.doReceive);
            this._SendWorker.DoWork += new DoWorkEventHandler(this.sendCommand);
            this._ReceiveWorker.RunWorkerAsync();
        }

        private void receiveCommand(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (this.Socket.Connected)
                {
                    byte[] buffer = new byte[4];
                    this.Stream.Read(buffer, 0, 4);
                    buffer = new byte[BitConverter.ToInt32(buffer, 0)];
                    this.Stream.Read(buffer, 0, buffer.Length);
                    _DoReceiveWorker.RunWorkerAsync(buffer);
                }
            }
            catch (Exception ex)
            {
                this.OnCritical(ex.Message, this);
                this.OnDisconnected(this);
            }
        }

        private void doReceive(object sender, DoWorkEventArgs e)
        {
            byte[] buffer = (byte[])e.Argument;
            Command cmd = buffer.ConvertTo<Command>();
            cmd.FromUser = this.User;

            this.OnReceived(this, cmd);
        }

        private void sendCommand(object sender, DoWorkEventArgs e)
        {
            Command cmd = (Command)e.Argument;
            byte[] buffer = new byte[4];
            byte[] cmdBuffer = cmd.ToBytes<Command>();
            buffer = BitConverter.GetBytes(cmdBuffer.Length);
            this.Stream.Write(buffer, 0, 4);
            this.Stream.Flush();
            this.Stream.Write(cmdBuffer, 0, cmdBuffer.Length);
            this.Stream.Flush();
        }

        public void SendCommand(Command cmd)
        {
            this._SendWorker.RunWorkerAsync(cmd);
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
