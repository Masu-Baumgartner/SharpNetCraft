using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpNetCraft.API;
using SharpNetCraft.Events;
using SharpNetCraft.Pakets;
using SharpNetCraft.Pakets.Handshake;
using SharpNetCraft.Pakets.Login;
using SharpNetCraft.Pakets.Play;
using System.Security.Cryptography;
using SharpNetCraft.Utils;
using System.Numerics;
using Newtonsoft.Json;
using MojangSharp.Endpoints;
using MojangSharp.Responses;

namespace SharpNetCraft
{
    public class MinecraftClient : IPacketHandler
    {
        private string usernameoremail { get; set; }
        private string password { get; set; }
        private string clienttoken { get; set; }
        private string sessiontoken { get; set; }
        private string uuid { get; set; }
        private string username { get; set; }
        public ActionProvider actionProvider 
        { 
            get
            {
                return provider;
            }
        }

        private ActionProvider provider;
        private bool onlineMode { get; set; }

        private IPEndPoint Endpoint;

        private byte[] SharedSecret = new byte[16];
        public bool Login(string u, string p)
        {
            try
            {
                usernameoremail = u;
                password = p;
                onlineMode = true;

                AuthenticateResponse auth = new Authenticate(new Credentials() { Username = usernameoremail, Password = password }).PerformRequestAsync().Result;
                if (auth.IsSuccess)
                {
                    clienttoken = auth.ClientToken;
                    sessiontoken = auth.AccessToken;
                    uuid = auth.SelectedProfile.Value;
                    username = auth.SelectedProfile.PlayerName;
                    Logger.GetLogger().Info("UUID: " + auth.SelectedProfile.Value);
                    return true;
                }
                else
                {
                    Logger.GetLogger().Info("Auth failed. Maybe invalid credentials | " + auth.Error.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error("Login: " + ex.Message);
                return false;
            }
        }
        public void SetOfflineUsername(string username)
        {
            onlineMode = false;
            this.username = username;
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
                provider = new ActionProvider(Client);
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
                case DisconnectPacket disconnect:
                    Logger.GetLogger().Debug("Disconnected: " + disconnect.Message);
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
            Logger.GetLogger().Info($"Received encryption request.");
            new Random().NextBytes(SharedSecret);

            var serverId = packet.ServerId;
            var publicKey = packet.PublicKey;
            var verificationToken = packet.VerifyToken;

            string serverHash;

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
                        connectionState = MinecraftState.Disconnected;
                        return;
                    }

                    var cryptoProvider = RsaHelper.DecodePublicKey(publicKey);
                    var encrypted = cryptoProvider.Encrypt(SharedSecret, RSAEncryptionPadding.Pkcs1);

                    EncryptionResponsePacket response = EncryptionResponsePacket.CreateObject();
                    response.SharedSecret = encrypted;
                    response.VerifyToken = cryptoProvider.Encrypt(verificationToken, RSAEncryptionPadding.Pkcs1);

                    Client.InitEncryption(SharedSecret);

                    Client.SendPacket(response);
                });

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
            loginStart.Username = username;
            SendPacket(loginStart);
        }
        public void SendPacket(Packet packet)
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
        private static string JavaHexDigest(byte[] input)
        {
            var hash = new SHA1Managed().ComputeHash(input);
            // Reverse the bytes since BigInteger uses little endian
            Array.Reverse(hash);

            BigInteger b = new BigInteger(hash);
            if (b < 0)
            {
                return "-" + (-b).ToString("x").TrimStart('0');
            }
            else
            {
                return b.ToString("x").TrimStart('0');
            }
            /*
			var sha1 = SHA1.Create();
			byte[] hash = sha1.ComputeHash(input);
			bool negative = (hash[0] & 0x80) == 0x80;
			if (negative) // check for negative hashes
				hash = TwosCompliment(hash);
			// Create the string and trim away the zeroes
			string digest = GetHexString(hash).TrimStart('0');
			if (negative)
				digest = "-" + digest;
			return digest;*/
        }
        public sealed class JoinRequest
        {
            [JsonProperty("accessToken")]
            public string AccessToken;

            [JsonProperty("selectedProfile")]
            public string SelectedProfile;

            [JsonProperty("serverId")]
            public string ServerId;
        }
        private async Task<bool> VerifySession(string serverHash)
        {
            if (string.IsNullOrWhiteSpace(sessiontoken))
            {
                return false;
            }

            try
            {
                var baseAddress = "https://sessionserver.mojang.com/session/minecraft/join";

                var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new JoinRequest()
                {
                    ServerId = serverHash,
                    SelectedProfile = uuid,
                    AccessToken = sessiontoken
                }));

                using (Stream newStream = http.GetRequestStream())
                {
                    await newStream.WriteAsync(bytes, 0, bytes.Length);
                }

                WebResponse wr = await http.GetResponseAsync();
                
                using (var stream = wr.GetResponseStream())
				using (var sr = new StreamReader(stream))
				{
					var content = await sr.ReadToEndAsync();
				}
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsOnlineMode
        {
            get
            {
                return onlineMode;
            }
        }
    }
}