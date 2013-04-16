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
        private BackgroundWorker _ReceiveWorker = new BackgroundWorker();
        private BackgroundWorker _DoReceiveWorker = new BackgroundWorker();
        private BackgroundWorker _SendWorker = new BackgroundWorker();
        private BackgroundWorker _ConnectWorker = new BackgroundWorker();
        private IPEndPoint _ServerIP;
        public IPAddress ServerIP { get { return this._ServerIP.Address; } }
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
            _ServerIP = new IPEndPoint(serverIP, port);
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            this._ConnectWorker.DoWork+=new DoWorkEventHandler(connectToServer);
            this._ConnectWorker.RunWorkerAsync();
        }

        private void connectToServer(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Socket.Connect(this._ServerIP);
                this._Stream = new NetworkStream(this.Socket);
                this.OnConnected();
                this._ReceiveWorker.DoWork += new DoWorkEventHandler(ReceiveCommand);
                this._DoReceiveWorker.DoWork += new DoWorkEventHandler(doReceive);
                this._SendWorker.DoWork += new DoWorkEventHandler(sendCommand);
                this._ReceiveWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                this.OnCritical(ex.StackTrace);
                this.OnConnectFailed();
            }
        }

        private void ReceiveCommand(object sender, DoWorkEventArgs e)
        {
            while (this.Socket.Connected)
            {
                byte[] buffer = new byte[4];
                this._Stream.Read(buffer, 0, 4);
                buffer = new byte[BitConverter.ToInt32(buffer, 0)];
                this._Stream.Read(buffer, 0, buffer.Length);
                _DoReceiveWorker.RunWorkerAsync(buffer);
            }
        }

        private void doReceive(object sender, DoWorkEventArgs e)
        {
            try
            {
                byte[] buffer = (byte[])e.Argument;
                Command cmd = buffer.ConvertTo<Command>();
                InvokeCommand(cmd);
            }
            catch (Exception ex)
            {
                this.OnCritical(ex.StackTrace);
            }
        }

        private void InvokeCommand(Command cmd)
        {
            if (cmd.Invoker != null)
            {
                Type clazz = cmd.Invoker.GetType();
                MethodInfo method = clazz
                    .GetMethods().Where(m => m.GetCustomAttributes(typeof(InvokeAttribute), false).Length > 0
                                        && (m.GetCustomAttributes(typeof(InvokeAttribute), false)[0]
                                            as InvokeAttribute).CommandType == cmd.Type)
                    .SingleOrDefault();
                if (method != null)
                {
                    object[] @params = new object[cmd.Metadata.Datas.Length + 1];
                    @params[0] = cmd.FromUser;
                    cmd.Metadata.Datas.CopyTo(@params, 1);
                    method.Invoke(cmd.Invoker, @params);
                }
            }
        }

        public void SendCommand(object invoker, CommandType cmdType, string toUser, params object[] obj)
        {
            Command cmd = new Command(cmdType, new CommandMetadata(obj));
            cmd.ToUser = toUser;
            cmd.Invoker = invoker;
            this._SendWorker.RunWorkerAsync(cmd);
        }

        private void sendCommand(object sender, DoWorkEventArgs e)
        {
            Command cmd = (Command)e.Argument;
            byte[] buffer = new byte[4];
            byte[] cmdBuffer = cmd.ToBytes<Command>();
            buffer = BitConverter.GetBytes(cmdBuffer.Length);
            this._Stream.Write(buffer, 0, 4);
            this._Stream.Flush();
            this._Stream.Write(cmdBuffer, 0, cmdBuffer.Length);
            this._Stream.Flush();
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
