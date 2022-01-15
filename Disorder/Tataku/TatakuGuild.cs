using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using Disorder.Helpers;
using Disorder.Tataku.Packets;
using WebSocketSharp;
using Logger = Kettu.Logger;

namespace Disorder.Tataku; 

public class TatakuGuild : IGuild {
    public readonly TatakuChatClient ChatClient;
    private WebSocket client;

    public string Name { get; set; }
    public long Id { get; set; }
    public IEnumerable<IChannel> Channels { get; }
    
    public TatakuGuild(string uri, TatakuChatClient chatClient) {
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
            Logger.Log("Connected to " + uri, LoggerLevelTatakuInfo.Instance);

            string username = Settings.Instance.TatakuUsername;
            string password = HashHelper.Sha512Hash(Encoding.UTF8.GetBytes(Settings.Instance.TatakuPassword));
            
            this.PacketQueue.Enqueue(new ClientUserLoginPacket(username, password));
        };
        this.client.OnMessage += delegate(object? _, MessageEventArgs args) {
            using MemoryStream ms = new(args.RawData);
            using TatakuReader reader = new(ms);
            
            this.HandlePacket(reader);
        };

        this.client.Connect();
    }

    public readonly ConcurrentQueue<TatakuPacket?> PacketQueue = new();

    private long lastPing = 0;

    public async Task Process() {
        // Run through everything currently in the packet queue
        while(this.PacketQueue.TryDequeue(out TatakuPacket? packet) && this.client.IsAlive && packet != null) {
            await using MemoryStream ms = new();
            await using TatakuWriter writer = new(ms);

            packet.WriteDataToStream(writer);
            this.client.Send(ms.ToArray());
        }

        if(TimestampHelper.Timestamp - 5 > this.lastPing) {
            this.client.Ping();
            this.lastPing = TimestampHelper.Timestamp;
        }
    }

    public void HandlePacket(TatakuReader reader) {
        TatakuPacketId packetId = reader.ReadPacketId();

        switch(packetId) {
            case TatakuPacketId.ServerLoginResponse: {
                ServerLoginResponsePacket packet = new();
                packet.ReadDataFromStream(reader);

                if(packet.LoginStatus != TatakuLoginStatus.Ok) {
                    string errorMessage = "Login failed: " + packet.LoginStatus;

                    Logger.Log(errorMessage, LoggerLevelTatakuError.Instance);
                    throw new Exception(errorMessage);
                }

                // Logged in and authenticated at this point
                Logger.Log("Login OK, user id is " + packet.UserId, LoggerLevelTatakuInfo.Instance);
                
                this.PacketQueue.Enqueue(new ClientStatusUpdatePacket(new UserAction(UserActionType.Idle, "yas shillin' in da house", 0)));
                break;
            }
            case TatakuPacketId.ServerSendMessage: {
                ServerSendMessagePacket packet = new();
                packet.ReadDataFromStream(reader);
                
                Logger.Log($"Got message: {packet.Channel}: <{packet.SenderId}>: {packet.Message}", LoggerLevelTatakuInfo.Instance);
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