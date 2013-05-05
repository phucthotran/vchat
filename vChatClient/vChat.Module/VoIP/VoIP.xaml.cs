using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Core.Client;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Interaction logic for VoIP.xaml
    /// </summary>
    public partial class VoIP : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region CLASS MEMBER

        private String remoteIpAddress;
        private String clientName;

        private VoIPBase voip;

        private UdpClient callUdp;

        private EndPoint remoteCmdEndp;
        private IPEndPoint remoteCmdIpEndp;

        private EndPoint remoteCallEndp;
        private IPEndPoint remoteCallIpEndp;

        private EndPoint localCmdEndp;
        private IPEndPoint localCmdIpEndp;

        private EndPoint localCallEndp;
        private IPEndPoint localCallIpEndp;

        private Socket commandSocket;

        private const int CALL_PORT = 9000;
        private const int COMMAND_PORT = 9100;

        private byte[] localData = new byte[1024];
        private byte[] callData = new byte[800];
        private UncompressedPcmCodec pcmCodec;

        private volatile bool callActive;

        #endregion

        #region PROPERTY

        public String RemoteIPAddress
        {
            get { return remoteIpAddress; }
            set
            {
                if (value != remoteIpAddress)
                {
                    remoteIpAddress = value;
                    this.OnPropertyChanged("RemoteIPAddress");
                }
            }
        }

        #endregion

        public VoIP()
        {
            InitializeComponent();

            DataContext = this;            
        }

        #region MAIN METHOD

        public void Init()
        {
            clientName = this.Get<Client>().Name;

            localCmdIpEndp = new IPEndPoint(IPAddress.Any, COMMAND_PORT);
            localCmdEndp = (EndPoint)localCmdIpEndp;
            commandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            commandSocket.Bind(localCmdEndp);

            localCallIpEndp = new IPEndPoint(IPAddress.Any, CALL_PORT);
            localCallEndp = (EndPoint)localCallIpEndp;

            remoteCmdIpEndp = new IPEndPoint(IPAddress.Any, 0);
            remoteCmdEndp = (EndPoint)remoteCmdIpEndp;

            remoteCallIpEndp = new IPEndPoint(IPAddress.Any, 0);
            remoteCallEndp = (EndPoint)remoteCallIpEndp;

            commandSocket.BeginReceiveFrom(localData, 0, localData.Length, SocketFlags.None, ref remoteCmdEndp, new AsyncCallback(CommandSocket_OnReceiving), null);
        }

        #region COMMAND SOCKET

        private void CommandSocket_OnReceiving(IAsyncResult asyncResult)
        {
            commandSocket.EndReceiveFrom(asyncResult, ref remoteCmdEndp);

            DataPacket packetReceived = new DataPacket(localData);

            DataPacket packetToSend = new DataPacket();
            packetToSend.Name = clientName;

            switch (packetReceived.Command)
            {
                case CallCommand.Invite:
                    if (!callActive)
                    {
                        if (MessageBox.Show("Call coming from " + packetReceived.Name + ".\r\n\r\nAccept it?", "VoiceChat", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            packetToSend.Command = CallCommand.OK;
                            SendPacket(packetToSend, remoteCmdEndp);

                            remoteCmdIpEndp = (IPEndPoint)remoteCmdEndp;

                            remoteCallIpEndp = new IPEndPoint(remoteCmdIpEndp.Address, CALL_PORT);
                            remoteCallEndp = (EndPoint)remoteCallIpEndp;

                            InitCall();
                        }
                        else
                        {
                            packetToSend.Command = CallCommand.Busy;
                            SendPacket(packetToSend, remoteCmdEndp);
                        }
                    }
                    else
                    {
                        packetToSend.Command = CallCommand.Busy;
                        SendPacket(packetToSend, remoteCmdEndp);
                    }
                    break;

                case CallCommand.Busy:
                    MessageBox.Show("User busy");
                    break;

                case CallCommand.OK:
                    InitCall();
                    break;

                case CallCommand.Bye:
                    MessageBox.Show("User end call");
                    UninitCall();
                    break;
            }

            localData = new byte[1024];
            commandSocket.BeginReceiveFrom(localData, 0, localData.Length, SocketFlags.None, ref remoteCmdEndp, new AsyncCallback(CommandSocket_OnReceiving), null);
        }

        private void CommandSocket_OnSending(IAsyncResult asyncResult)
        {
            commandSocket.EndSendTo(asyncResult);
        }

        #endregion

        #region TRANSFER DATA

        /// <summary>
        /// Send a packet to an end point
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="endPoint"></param>
        private void SendPacket(DataPacket packet, EndPoint endPoint)
        {
            byte[] packetByte = packet.ToByte();

            commandSocket.BeginSendTo(packetByte, 0, packetByte.Length, SocketFlags.None, endPoint, new AsyncCallback(CommandSocket_OnSending), endPoint);
        }

        private void VoIP_OnRecording(object sender, RecordEventArgs e)
        {
            byte[] tmpData = e.RecordedData;
            callUdp.Send(tmpData, tmpData.Length, remoteCallIpEndp);
        }

        #endregion

        #region MAIN WORK FOR CALLING

        /// <summary>
        /// Init for a call
        /// </summary>
        private void InitCall()
        {
            callUdp = new UdpClient(CALL_PORT);
            callActive = true;
            pcmCodec = new UncompressedPcmCodec();

            Thread tRecording = new Thread(new ThreadStart(() =>
            {
                voip = new VoIPBase();
                voip.OnRecording += new VoIPBase.RecordEventHandler(VoIP_OnRecording);

                while (callActive) ; //Keep thread running and take "voip" far away from "NullReferenceException"
            }));
            tRecording.IsBackground = true;

            Thread tReceive = new Thread(new ThreadStart(() => {
                while (callActive)
                {
                    byte[] receivedData = callUdp.Receive(ref remoteCallIpEndp);

                PLAY_AGAIN:
                    try
                    {
                        voip.AsyncPlaying(receivedData, 0, receivedData.Length);
                    }
                    catch (NullReferenceException) //Fix strangle error by voi.AsyncPlaying
                    {
                        goto PLAY_AGAIN;
                    }
                }
            }));
            tReceive.IsBackground = true;

            tRecording.Start();
            tReceive.Start();
        }        

        /// <summary>
        /// Do call to a remote end point
        /// </summary>
        private void DoCall()
        {
            remoteCmdIpEndp = new IPEndPoint(IPAddress.Parse(remoteIpAddress), COMMAND_PORT);
            remoteCmdEndp = (EndPoint)remoteCmdIpEndp;

            remoteCallIpEndp = new IPEndPoint(IPAddress.Parse(remoteIpAddress), CALL_PORT);
            remoteCallEndp = (EndPoint)remoteCallIpEndp;

            DataPacket packetToSend = new DataPacket();
            packetToSend.Command = CallCommand.Invite;
            packetToSend.Name = localCmdIpEndp.Address.ToString();

            SendPacket(packetToSend, remoteCmdEndp);
        }

        /// <summary>
        /// Drop current call
        /// </summary>
        private void DropCall()
        {
            DataPacket packetToSend = new DataPacket();
            packetToSend.Command = CallCommand.Bye;
            packetToSend.Name = localCmdIpEndp.Address.ToString();

            SendPacket(packetToSend, remoteCmdEndp);
        }

        private void UninitCall()
        {
            callActive = false;
            voip.Dispose();

            commandSocket.Close();
            commandSocket.Dispose();
            pcmCodec.Dispose();
        }

        #endregion

        protected virtual void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region EVENT

        private void btnCall_Click(object sender, RoutedEventArgs e)
        {
            DoCall();
        }

        private void btnEndCall_Click(object sender, RoutedEventArgs e)
        {
            DropCall();
        }

        #endregion
    }
}
