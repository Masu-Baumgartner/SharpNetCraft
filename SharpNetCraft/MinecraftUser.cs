using System;
using System.Net;
using System.Threading;
using SharpNetCraft.API;
using SharpNetCraft.Events;
using SharpNetCraft.Pakets;
using SharpNetCraft.Pakets.Handshake;
using SharpNetCraft.Pakets.Login;
using SharpNetCraft.Pakets.Play;

namespace SharpNetCraft
{
    public class MinecraftUser : IPacketHandler
    {
        private string usernameoremail { get; set; }
        private string password { get; set; }
        private string clienttoken { get; set; }
        private string sessiontoken { get; set; }

        private IPEndPoint Endpoint;

        private byte[] SharedSecret = new byte[16];
        public bool Login(string u, string p)
        {
            return false;
        }
        public MinecraftState connectionState { get; private set; } = MinecraftState.Idle;

        private NetConnection Client;

        public IPacketHandler hook { get; set; }

        #region PaketHandlerOrigin
        void IPacketHandler.HandleHandshake(Packet packet)
        {
            if (hook != null)
                hook.HandleHandshake(packet);
        }
        void IPacketHandler.HandleStatus(Packet packet)
        {
            if (hook != null)
                hook.HandleStatus(packet);
        }
        void IPacketHandler.HandleLogin(Packet packet)
        {
            if (hook != null)
                hook.HandleLogin(packet);
            if (packet is DisconnectPacket disconnect)
            {
                HandleDisconnectPacket(disconnect);
            }
            else if (packet is EncryptionRequestPacket)
            {
                HandleEncryptionRequest((EncryptionRequestPacket)packet);
            }
            else if (packet is SetCompressionPacket compression)
            {
                HandleSetCompression(compression);
            }
            else if (packet is LoginSuccessPacket success)
            {
                HandleLoginSuccess(success);
            }
        }
        void IPacketHandler.HandlePlay(Packet packet)
        {
            if (hook != null)
                hook.HandlePlay(packet);
            switch (packet)
            {
                case KeepAlivePacket keepAlive:
                    Logger.GetLogger().Debug("Keepalive");
                    KeepAlivePacket response = KeepAlivePacket.CreateObject();
                    response.KeepAliveid = keepAlive.KeepAliveid;
                    response.PacketId = 0x0F;
                    SendPacket(response);
                    break;
            }
        }
        #endregion

        #region Handlers
        private void HandleLoginSuccess(LoginSuccessPacket success)
        {
            Client.ConnectionState = ConnectionState.Play;
            connectionState = MinecraftState.Play;
            Logger.GetLogger().Info("Sucessfully logged in");
        }
        private void HandleDisconnectPacket(DisconnectPacket packet)
        {
            connectionState = MinecraftState.Disconnected;
            Client.Stop();
            Logger.GetLogger().Info("Disconnected");
        }
        private void HandleEncryptionRequest(EncryptionRequestPacket packet)
        {
            new Random().NextBytes(SharedSecret);

            var serverId = packet.ServerId;
            var publicKey = packet.PublicKey;
            var verificationToken = packet.VerifyToken;

            string serverHash; //TODO: Implement Auth
            /*
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] ascii = Encoding.ASCII.GetBytes(serverId);
                ms.Write(ascii, 0, ascii.Length);
                ms.Write(SharedSecret, 0, 16);
                ms.Write(publicKey, 0, publicKey.Length);

                serverHash = JavaHexDigest(ms.ToArray());
            }

            VerifySession(serverHash).ContinueWith(
                x =>
                {
                    var authenticated = x.Result;

                    if (!authenticated)
                    {
                        ShowDisconnect("disconnect.loginFailedInfo.invalidSession", true);
                        return;
                    }

                    var cryptoProvider = RsaHelper.DecodePublicKey(publicKey);
                    //Log.Info($"Crypto: {cryptoProvider == null} Pub: {packet.PublicKey} Shared: {SharedSecret}");
                    var encrypted = cryptoProvider.Encrypt(SharedSecret, RSAEncryptionPadding.Pkcs1);

                    EncryptionResponsePacket response = EncryptionResponsePacket.CreateObject();
                    response.SharedSecret = encrypted;
                    response.VerifyToken = cryptoProvider.Encrypt(verificationToken, RSAEncryptionPadding.Pkcs1);

                    Client.InitEncryption(SharedSecret);

                    Client.SendPacket(response);// SendPacket(response);
                });//.Wait();

            */
            Logger.GetLogger().Info("Enabled encryption");
        }
        private void HandleSetCompression(SetCompressionPacket compression)
        {
            Client.CompressionThreshold = compression.Threshold;
            Client.CompressionEnabled = compression.Threshold > 0;
            Logger.GetLogger().Info("Enabled compression");
        }
        #endregion

        public void Connect(string host, int port)
        {
            Endpoint = IPEndPoint.Parse(host + ":" + port);
            Client = new NetConnection(Endpoint, CancellationToken.None);
            Client.PacketHandler = this;
            Client.OnConnectionClosed += OnConnectionClosed;
            Client.Initialize(CancellationToken.None);

            HandshakePacket handshake = HandshakePacket.CreateObject();
            handshake.NextState = ConnectionState.Login;
            handshake.ServerAddress = host;
            handshake.ServerPort = (ushort)Endpoint.Port;
            handshake.ProtocolVersion = JavaProtocol.ProtocolVersion;
            SendPacket(handshake);

            Client.ConnectionState = ConnectionState.Login;

            LoginStartPacket loginStart = LoginStartPacket.CreateObject();
            loginStart.Username = "Masusniper";
            SendPacket(loginStart);
        }

        private void SendPacket(Packet packet)
        {
            Client.SendPacket(packet);
        }

        private void OnConnectionClosed(object sender, ConnectionClosedEventArgs e)
        {
            Logger.GetLogger().Debug("Connection closed");
            connectionState = MinecraftState.Disconnected;
        }

        public NetConnection GetConnection()
        {
            return Client;
        }
    }
}
