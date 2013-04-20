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
        private Queue<byte[]> _DoReceiveQueue = new Queue<byte[]>();
        private Queue<Command> _SendQueue = new Queue<Command>();
        private IPEndPoint _ServerIP;
        private CommandExecuter _Executer = new CommandExecuter();
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
            Connect(false);
        }

        public void Connect(bool doThrow)
        {
            try
            {
                this.Socket.Connect(this._ServerIP);
                this._Stream = new NetworkStream(this.Socket);
                this.OnConnected();
                this._ReceiveWorker.DoWork += new DoWorkEventHandler(ReceiveCommand);
                this._DoReceiveWorker.DoWork += new DoWorkEventHandler(doReceive);
                this._DoReceiveWorker.RunWorkerCompleted+=new RunWorkerCompletedEventHandler(doReceiveComplete);
                this._SendWorker.DoWork += new DoWorkEventHandler(sendCommand);
                this._ReceiveWorker.RunWorkerAsync();
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
            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.Socket.Disconnect(true);
        }

        public void CommandBinding(CommandType type, Action<CommandResponse> action)
        {
            _Executer.Set(type, action);
        }

        private void ReceiveCommand(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (this.Socket.Connected)
                {
                    byte[] buffer = new byte[4];
                    this._Stream.Read(buffer, 0, 4);
                    buffer = new byte[BitConverter.ToInt32(buffer, 0)];
                    this._Stream.Read(buffer, 0, buffer.Length);
                    _DoReceiveQueue.Enqueue(buffer);
                    if (!_DoReceiveWorker.IsBusy)
                    {
                        while (_DoReceiveQueue.Count > 0)
                        {
                            _DoReceiveWorker.RunWorkerAsync(_DoReceiveQueue.Dequeue());
                        }
                    }
                }
            }
            catch
            {
                this.ID = -1;
                this.Name = "";
            }
        }

        private void doReceive(object sender, DoWorkEventArgs e)
        {
            byte[] buffer = e.Argument as byte[];
            e.Result = buffer.ConvertTo<Command>();
        }

        private void doReceiveComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Command cmd = e.Result as Command;
            _Executer[cmd.Type].DynamicInvoke(new CommandResponse(cmd));
        }

        /*
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
        } */

        public void SendCommand(Command cmd)
        {
            _SendQueue.Enqueue(cmd);
            if (!_SendWorker.IsBusy)
                _SendWorker.RunWorkerAsync();
        }

        private void sendCommand(object sender, DoWorkEventArgs e)
        {
            while (_SendQueue.Count > 0)
            {
                Command cmd = _SendQueue.Dequeue();
                byte[] buffer = new byte[4];
                byte[] cmdBuffer = cmd.ToBytes<Command>();
                buffer = BitConverter.GetBytes(cmdBuffer.Length);
                this._Stream.Write(buffer, 0, 4);
                this._Stream.Flush();
                this._Stream.Write(cmdBuffer, 0, cmdBuffer.Length);
                this._Stream.Flush();
            }
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
