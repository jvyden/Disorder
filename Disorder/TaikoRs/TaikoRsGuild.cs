using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using Disorder.Helpers;
using Disorder.TaikoRs.Packets;
using WebSocketSharp;
using Logger = Kettu.Logger;

namespace Disorder.TaikoRs; 

public class TaikoRsGuild : IGuild {
    public readonly TaikoRsChatClient ChatClient;
    private WebSocket client;

    public string Name { get; set; }
    public long Id { get; set; }
    public IEnumerable<IChannel> Channels { get; }
    
    public TaikoRsGuild(string uri, TaikoRsChatClient chatClient) {
        this.ChatClient = chatClient;
        this.Name = uri;

        this.client = new WebSocket(uri);
        this.client.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;

        
        #region Disable logging
        #if !DEBUG
        FieldInfo? field = this.client.Log.GetType().GetField("_output", BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(this.client.Log, new Action<LogData, string>((_, _) => {}));
        #endif
        #endregion
        
        this.client.OnOpen += delegate {
            Logger.Log("Connected to " + uri, LoggerLevelTaikoRsInfo.Instance);

            string username = Settings.Instance.TaikoRsUsername;
            string password = HashHelper.Sha512Hash(Encoding.UTF8.GetBytes(Settings.Instance.TaikoRsPassword));
            
            this.PacketQueue.Enqueue(new ClientUserLoginPacket(username, password));
        };
        this.client.OnMessage += delegate(object? _, MessageEventArgs args) {
            using MemoryStream ms = new(args.RawData);
            using TaikoRsReader reader = new(ms);
            
            this.HandlePacket(reader);
        };

        this.client.Connect();
    }

    public readonly ConcurrentQueue<TaikoRsPacket?> PacketQueue = new();

    public async Task Process() {
        // Run through everything currently in the packet queue
        while(this.PacketQueue.TryDequeue(out TaikoRsPacket? packet) && this.client.IsAlive && packet != null) {
            await using MemoryStream ms = new();
            await using TaikoRsWriter writer = new(ms);

            packet.WriteDataToStream(writer);
            this.client.Send(ms.ToArray());
        }
    }

    public void HandlePacket(TaikoRsReader reader) {
        TaikoRsPacketId packetId = reader.ReadPacketId();

        switch(packetId) {
            case TaikoRsPacketId.ServerLoginResponse: {
                ServerLoginResponsePacket packet = new();
                packet.ReadDataFromStream(reader);

                if(packet.LoginStatus != TaikoRsLoginStatus.Ok) {
                    string errorMessage = "Login failed: " + packet.LoginStatus;

                    Logger.Log(errorMessage, LoggerLevelTaikoRsError.Instance);
                    throw new Exception(errorMessage);
                }

                // Logged in and authenticated at this point
                Logger.Log("Login OK, user id is " + packet.UserId, LoggerLevelTaikoRsInfo.Instance);
                
                this.PacketQueue.Enqueue(new ClientStatusUpdatePacket(new UserAction(UserActionType.Idle, "yas shillin' in da house", 0)));
                break;
            }
            case TaikoRsPacketId.ServerSendMessage: {
                ServerSendMessagePacket packet = new();
                packet.ReadDataFromStream(reader);
                
                Logger.Log($"Got message: {packet.Channel}: <{packet.SenderId}>: {packet.Message}", LoggerLevelTaikoRsInfo.Instance);
                break;
            }
            default: {
                throw new NotImplementedException(packetId.ToString());
            }
        }
    }

    public void SendMessage(string channel, string content) {
        this.PacketQueue.Enqueue(new ClientSendMessagePacket(channel, content));
    }
        
    public event EventHandler<IChannel>? ChannelAdded;
}