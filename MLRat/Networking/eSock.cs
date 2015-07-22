using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Networking
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// eSock 2.0 by BahNahNah
    /// uid=2388291
    /// </summary>
    public static class eSock
    {
        #region " eSock Server "

        public class Server
        {

            #region " Delegates "

            public delegate void OnClientConnectCallback(Server sender, eSockClient client);
            public delegate void OnClientDisconnectCallback(Server sender, eSockClient client, SocketError ER);
            public delegate bool OnClientConnectingCallback(Server sender, Socket cSock);
            public delegate void OnDataRetrievedCallback(Server sender, eSockClient client, object[] data);

            #endregion

            #region " Callbacks "

            public event OnClientConnectCallback OnClientConnect;
            public event OnClientDisconnectCallback OnClientDisconnect;
            public event OnClientConnectingCallback OnClientConnecting;
            public event OnDataRetrievedCallback OnDataRetrieved;

            #endregion

            private Socket _globalSocket;
            private int _BufferSize = 1000000;
            public int BufferSize
            {
                get
                {
                    return _BufferSize;
                }
                set
                {
                    if (value < 1)
                        throw new ArgumentOutOfRangeException("BufferSize");
                    if (IsRunning)
                        throw new Exception("Cannot set buffer size while server is running.");
                    _BufferSize = value;
                }
            }
            public bool IsRunning { get; private set; }

            #region " Constructors "

            public Server()
            {
                _globalSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IsRunning = false;
            }
            public Server(AddressFamily SocketaddressFamily)
                : this()
            {
                _globalSocket = new Socket(SocketaddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }

            #endregion

            #region " Functions "

            public bool Start(int port)
            {
                if (IsRunning)
                    throw new Exception("Server is already running.");
                try
                {
                    _globalSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    _globalSocket.Listen(5);
                    _globalSocket.BeginAccept(AcceptCallback, null);
                    IsRunning = true;
                }
                catch
                {
                    IsRunning = false;
                }
                return IsRunning;
            }

            public bool Start(int port, int backlog)
            {
                if (IsRunning)
                    throw new Exception("Server is already running.");
                try
                {
                    _globalSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    _globalSocket.Listen(backlog);
                    _globalSocket.BeginAccept(AcceptCallback, null);
                    IsRunning = true;
                }
                catch
                {
                    return false;
                }
                return IsRunning;
            }

            public void Stop()
            {
                IsRunning = false;
                _globalSocket.Close();

            }
            #endregion

            #region " Callbacks "

            private void AcceptCallback(IAsyncResult AR)
            {
                if (!IsRunning)
                    return;
                Socket cSock = _globalSocket.EndAccept(AR);
                if (OnClientConnecting != null)
                {
                    if (!OnClientConnecting(this, cSock))
                        return;
                }
                eSockClient _client = new eSockClient(cSock, BufferSize);
                if (OnClientConnect != null)
                    OnClientConnect(this, _client);
                _client.NetworkSocket.BeginReceive(_client.Buffer, 0, _client.Buffer.Length, SocketFlags.None,
                    RetrieveCallback, _client);
                _globalSocket.BeginAccept(AcceptCallback, null);
            }

            private void RetrieveCallback(IAsyncResult AR)
            {
                if (!IsRunning)
                    return;
                eSockClient _client = (eSockClient)AR.AsyncState;
                SocketError SE;
                int packetLength = _client.NetworkSocket.EndReceive(AR, out SE);
                if (SE != SocketError.Success)
                {
                    if (OnClientDisconnect != null)
                        OnClientDisconnect(this, _client, SE);
                    return;
                }
                byte[] Packet = new byte[packetLength];
                Buffer.BlockCopy(_client.Buffer, 0, Packet, 0, packetLength);
                _client.NetworkSocket.BeginReceive(_client.Buffer, 0, _client.Buffer.Length, SocketFlags.None,
                    RetrieveCallback, _client);

                object[] RetrievedData = Formatter.Deserialize<object[]>(Packet);
                if (OnDataRetrieved != null)
                    OnDataRetrieved(this, _client, RetrievedData);

            }

            #endregion

            public class eSockClient
            {
                public byte[] Buffer { get; set; }
                public object Tag { get; set; }
                public Socket NetworkSocket { get; private set; }
                public eSockClient(Socket cSock)
                {
                    NetworkSocket = cSock;
                    Buffer = new byte[8192];
                }

                public eSockClient(Socket cSock, int bufferSize)
                {
                    NetworkSocket = cSock;
                    Buffer = new byte[bufferSize];
                }

                public void Send(params object[] args)
                {
                    try
                    {
                        byte[] serilisedData = Formatter.Serialize(args);
                        NetworkSocket.BeginSend(serilisedData, 0, serilisedData.Length, SocketFlags.None, EndSend, null);
                    }
                    catch
                    {
                        //Not connected
                    }
                }
                private void EndSend(IAsyncResult AR)
                {
                    SocketError SE;
                    NetworkSocket.EndSend(AR, out SE);
                }

            }
        }

        #endregion

        #region " eSock Client "

        public class Client
        {
            #region " Delegates "

            public delegate void OnConnectAsyncCallback(Client sender, bool success);
            public delegate void OnDisconnectCallback(Client sender, SocketError ER);
            public delegate void OnDataRetrievedCallback(Client sender, object[] data);

            #endregion

            #region " Callbacks "

            public event OnConnectAsyncCallback OnConnect;
            public event OnDisconnectCallback OnDisconnect;
            public event OnDataRetrievedCallback OnDataRetrieved;

            #endregion

            private Socket _globalSocket;
            private int _BufferSize = 1000000;
            public bool Connected { get; private set; }
            public byte[] PacketBuffer { get; private set; }
            public int BufferSize
            {
                get { return _BufferSize; }
                set
                {
                    if (Connected)
                        throw new Exception("Can not change buffer size while connected");
                    if (value < 1)
                        throw new ArgumentOutOfRangeException("BufferSize");
                    _BufferSize = value;
                }
            }

            #region " Constructor "

            public Client()
            {
                _globalSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Connected = false;
            }

            public Client(AddressFamily SocketAddressFamily)
                : this()
            {
                _globalSocket = new Socket(SocketAddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }

            #endregion"

            #region " Connect "

            public bool Connect(string IP, int port)
            {
                try
                {
                    _globalSocket.Connect(IP, port);
                    OnConnected();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool Connect(IPEndPoint endpoint)
            {
                try
                {
                    _globalSocket.Connect(endpoint);
                    OnConnected();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public void ConnectAsync(string IP, int port)
            {
                _globalSocket.BeginConnect(IP, port, OnConnectAsync, null);
            }

            public void ConnectAsync(IPEndPoint endpoint)
            {
                _globalSocket.BeginConnect(endpoint, OnConnectAsync, null);
            }

            private void OnConnectAsync(IAsyncResult AR)
            {
                try
                {
                    _globalSocket.EndConnect(AR);
                    if (OnConnect != null)
                        OnConnect(this, true);
                    OnConnected();
                }
                catch
                {
                    if (OnConnect != null)
                        OnConnect(this, false);
                }
            }

            private void OnConnected()
            {
                PacketBuffer = new byte[_BufferSize];
                _globalSocket.BeginReceive(PacketBuffer, 0, PacketBuffer.Length, SocketFlags.None, EndRetrieve, null);
            }

            #endregion

            #region " Functions "

            public void Send(params object[] data)
            {
                byte[] serilizedData = Formatter.Serialize(data);
                _globalSocket.BeginSend(serilizedData, 0, serilizedData.Length, SocketFlags.None, EndSend, null);
            }

            private void EndSend(IAsyncResult AR)
            {
                SocketError SE;
                _globalSocket.EndSend(AR, out SE);
            }

            #endregion

            #region " Callbacks "

            private void EndRetrieve(IAsyncResult AR)
            {
                SocketError SE;
                int packetLength = _globalSocket.EndReceive(AR, out SE);
                if (SE != SocketError.Success)
                {
                    if (OnDisconnect != null)
                        OnDisconnect(this, SE);
                    return;
                }
                byte[] Packet = new byte[packetLength];
                Buffer.BlockCopy(PacketBuffer, 0, Packet, 0, packetLength);
                _globalSocket.BeginReceive(PacketBuffer, 0, PacketBuffer.Length, SocketFlags.None, EndRetrieve, null);
                object[] data = Formatter.Deserialize<object[]>(Packet);
                if (OnDataRetrieved != null)
                    OnDataRetrieved(this, data);
            }

            #endregion
        }


        #endregion

        #region " eSock Formatter "

        public static class Formatter
        {
            public static byte[] Serialize(object input)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, input);
                    return Compress(ms.ToArray());
                }
            }

            public static t Deserialize<t>(byte[] input)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(Decompress(input)))
                {
                    return (t)bf.Deserialize(ms);
                }
            }

            public static byte[] Compress(byte[] input)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipStream _gz = new GZipStream(ms, CompressionMode.Compress))
                    {
                        _gz.Write(input, 0, input.Length);
                    }
                    return ms.ToArray();
                }
            }

            public static byte[] Decompress(byte[] input)
            {
                using (MemoryStream decompressed = new MemoryStream())
                {
                    using (MemoryStream ms = new MemoryStream(input))
                    {
                        using (GZipStream _gz = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            byte[] Bytebuffer = new byte[1024];
                            int bytesRead = 0;
                            while ((bytesRead = _gz.Read(Bytebuffer, 0, Bytebuffer.Length)) > 0)
                            {
                                decompressed.Write(Bytebuffer, 0, bytesRead);
                            }
                        }
                        return decompressed.ToArray();
                    }
                }
            }
        }

        #endregion
    }
}
