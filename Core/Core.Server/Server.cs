using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using Core.Server.ClientManagement;
using Core.Data;
using System.Threading.Tasks;

namespace Core.Server
{
    public class Server
    {
        #region Fields
        private bool _IsStarted;
        private Socket _Socket;
        private static FileStream _fsLog;
        private static StreamWriter _swLog;
        private Task logTasker;
        #endregion

        #region Properties
        public ServerConfig Config { get; private set; }
        public ClientManager ClientManager { get; private set; }
        public object Invoker { get; set; }
        #endregion

        #region Constructors
        public Server()
        {
            this.InitServer(true);
        }
        public Server(ServerConfig config)
        {
            this.Config = config;
            this.InitServer(false);
        }
        #endregion

        #region Private Methods
        private void InitServer(bool defaultConfig)
        {
            if (defaultConfig)
            {
                this.Config = new ServerConfig(true);
            }
            this._IsStarted = false;
            this.ClientManager = new ClientManager();
            if (!Directory.Exists("log"))
            {
                Directory.CreateDirectory("log");
            }
            string logToday = string.Format("log/server_{0:yyyy-MM-dd}.txt", DateTime.Today);
            if (!File.Exists(logToday))
            {
                File.Create(logToday).Dispose();
            }
            _fsLog = new FileStream(logToday, FileMode.Open, FileAccess.ReadWrite);
            _swLog = new StreamWriter(_fsLog);
            _swLog.AutoFlush = true;
        }
        private void ClientOnConnected(Client client)
        {
            this.OnClientConnected(client);
        }
        private void ClientOnDisconnected(Client client)
        {
            this.OnClientDisconnected(client);
            this.ClientManager.Remove(client);
        }

        private void ClientOnReceived(Client client, Command cmd)
        {
            this.invokeCommand(client, cmd);
            this.OnClientReceived(client, cmd);
        }

        private void invokeCommand(Client client, Command cmd)
        {
            if (this.Invoker != null)
            {
                try
                {
                    MethodInfo method = this.Invoker.GetType().GetMethods().Where(m =>
                                            (m.GetCustomAttributes(typeof(InvokeAttribute), false).Length > 0)
                                            && (m.GetCustomAttributes(typeof(InvokeAttribute), false)[0] as InvokeAttribute).CommandType == cmd.Type)
                                        .SingleOrDefault();
                    if (method != null)
                    {
                        object[] @params = new object[cmd.Metadata.Datas.Length];
                        cmd.Metadata.Datas.CopyTo(@params, 0);

                        if (method.GetParameters()[0].ParameterType == typeof(Client))
                        {
                            @params = new object[cmd.Metadata.Datas.Length + 1];
                            @params[0] = client;
                            cmd.Metadata.Datas.CopyTo(@params, 1);
                        }
                        method.Invoke(this.Invoker, @params);
                    }
                }
                catch (Exception ex)
                {
                    this.OnCritical(ex.StackTrace);
                }
            }
            if (!cmd.TargetUser.Equals("SERVER"))
            {
                Client target = this.ClientManager.GetClient(cmd.TargetUser);
                if (target != null)
                {
                    target.SendCommand(cmd);
                }
            }
        }

        private void listeningConnection()
        {
            if (_IsStarted)
            {
                _Socket.BeginAccept(ar =>
                {
                    listeningConnection();
                    Client client = new Client(_Socket.EndAccept(ar), new Client.ConnectedHandler(this.ClientOnConnected));
                    this.ClientManager.Add(client);
                    client.OnDisconnected += new Client.DisconnectedHandler(this.ClientOnDisconnected);
                    client.OnReceived += new Client.ReceivedHandler(this.ClientOnReceived);
                }, null);
            }
        }
        #endregion

        #region Public Methods
        public void Logging(string logSend)
        {
            Action<object> action = log =>
            {
                string logToday = string.Format("log/server_{0:yyyy-MM-dd}.txt", DateTime.Today);
                if (!File.Exists(logToday))
                {
                    File.Create(logToday).Dispose();
                    _fsLog = new FileStream(logToday, FileMode.Open, FileAccess.ReadWrite);
                    _swLog = new StreamWriter(_fsLog);
                    _swLog.AutoFlush = true;
                }
                string tmpLog = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - " + log;
                Console.WriteLine(tmpLog);
                _swLog.WriteLine(tmpLog);
            };
            if (logTasker == null || logTasker.IsCompleted)
                logTasker = Task.Factory.StartNew(action, logSend);
            else
                logTasker = logTasker.ContinueWith(t => action(logSend));
        }
        public void Restart()
        {
            this.Stop();
            this.Start();
        }
        public void Start()
        {
            try
            {
                if (!this._IsStarted)
                {
                    this._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this._Socket.Bind(this.Config.IP);
                    this._Socket.Listen(100);

                    this.OnStartSuccess();
                    _IsStarted = true;
                    listeningConnection();
                }
                else
                {
                    this.OnAlreadyStart();
                }
            }
            catch (Exception ex)
            {
                this.OnStartFailed();
                this.OnCritical(ex.StackTrace);
            }
        }
        public void Stop()
        {
            if (this._IsStarted)
            {
                this._IsStarted = false;
                this.ClientManager.Clear();
                this.OnStop();
            }
            else
            {
                this.OnAlreadyStop();
            }
        }
        #endregion

        #region Event Handler
        public delegate void StartFailed();
        public event StartFailed OnStartFailed = delegate { };

        public delegate void StartSuccess();
        public event StartSuccess OnStartSuccess = delegate { };

        public delegate void StopHandler();
        public event StopHandler OnStop = delegate { };

        public delegate void AlreadyStart();
        public event AlreadyStart OnAlreadyStart = delegate { };

        public delegate void AlreadyStop();
        public event AlreadyStop OnAlreadyStop = delegate { };

        public delegate void ClientConnected(Client client);
        public event ClientConnected OnClientConnected = delegate { };

        public delegate void ClientDisconnected(Client client);
        public event ClientDisconnected OnClientDisconnected = delegate { };

        public delegate void ClientReceived(Client client, Command cmd);
        public event ClientReceived OnClientReceived = delegate { };

        public delegate void Critical(string message);
        public event Critical OnCritical = delegate { };
        #endregion
    }
}