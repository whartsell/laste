using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace laste
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public byte[] Message { get; set; }
        public int ByteCount { get; set; }
        public IPEndPoint Sender { get; set; }
    }

    class Network
    {
        private static int BUFFER_SIZE = 256;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        private Socket listener;
        private int port;
        //private IPAddress address;
        private IPEndPoint groupEP;
        private bool isListening;
        private CancellationTokenSource tokenSource;
        private SocketAsyncEventArgs socketAsyncEventArgs;
        private bool receivePending;

        public Network(int aPort)
        {
            isListening = false;
            port = aPort;
            //address = LocalIPAddress();
            groupEP = new IPEndPoint(IPAddress.Any, port);
            tokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            if (!isListening)
            {


                listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                Task.Run(() => listen(tokenSource.Token), tokenSource.Token);
            }

        }

        public void Stop()
        {
            if (isListening)
            {
                tokenSource.Cancel();
                listener.Dispose();
                isListening = false;
            }

        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {

            EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
            if (handler != null)
                handler(this, e);
        }

        private void listen(CancellationToken token)
        {
            isListening = true;
            receivePending = false;
            IPEndPoint senderRemote = groupEP;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!receivePending)
                    {
                        socketAsyncEventArgs = new SocketAsyncEventArgs();
                        socketAsyncEventArgs.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);
                        socketAsyncEventArgs.RemoteEndPoint = senderRemote;
                        socketAsyncEventArgs.Completed += SocketAsyncEventArgs_Completed;
                        receivePending = listener.ReceiveFromAsync(socketAsyncEventArgs);
                        if (!receivePending)
                        {
                            SocketAsyncEventArgs_Completed(this, socketAsyncEventArgs);
                        }


                    }
                    else
                    {
                        Thread.Sleep(1000 / 120);
                    }
                }
                catch (SocketException)
                {

                    isListening = false;

                }
            }

        }

        private void SocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            MessageReceivedEventArgs args = new MessageReceivedEventArgs();
            args.Message = e.Buffer;
            args.ByteCount = e.BytesTransferred;
            args.Sender = (IPEndPoint)e.RemoteEndPoint;
            OnMessageReceived(args);
            receivePending = false;
            //throw new NotImplementedException();
        }


        public void sendMessage(IPEndPoint endpoint, string content)
        {
            UdpClient client = new UdpClient();
            byte[] dgram = Encoding.UTF8.GetBytes(content);
            client.Send(dgram, dgram.Length, endpoint);
        }

    }

    
}
