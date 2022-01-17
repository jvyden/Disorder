using System.Collections.Concurrent;
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

    private List<TatakuChannel> channels = new();
    public IEnumerable<IChannel> Channels => channels;
    public List<TatakuUser> Users { get; } = new();

    private readonly long beforeLogin;
    
    public TatakuGuild(string uri, TatakuChatClient chatClient) {
        this.ChatClient = chatClient;
        this.Name = uri;

        beforeLogin = TimestampHelper.TimestampMillis;

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
            Logger.Log($"Attempting to log in as {this.ChatClient.Username}", LoggerLevelTatakuInfo.Instance);

            string username = this.ChatClient.Username;
            string password = HashHelper.Sha512Hash(Encoding.UTF8.GetBytes(this.ChatClient.Password));
            
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
        while(this.PacketQueue.TryDequeue(out TatakuPacket? packet) && packet != null) {
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
                Logger.Log($"Login took {TimestampHelper.TimestampMillis - this.beforeLogin}ms", LoggerLevelTatakuInfo.Instance);

                this.ChatClient.User = new TatakuUser {
                    Id = packet.UserId,
                    Username = this.ChatClient.Username,
                };
                
                Users.Add((TatakuUser)this.ChatClient.User);

                TatakuChannel channel = new("#general", this);
                this.channels.Add(channel);
                this.ChannelAdded?.Invoke(this, channel);
                
                this.PacketQueue.Enqueue(new ClientStatusUpdatePacket(new UserAction(UserActionType.Idle, "yas shillin' in da house", 0)));
                break;
            }
            case TatakuPacketId.ServerSendMessage: {
                ServerSendMessagePacket packet = new();
                packet.ReadDataFromStream(reader);
                
                Logger.Log($"Got message: {packet.Channel}: <{packet.SenderId}>: {packet.Message}", LoggerLevelTatakuInfo.Instance);

                TatakuChannel? channel = this.channels.FirstOrDefault(c => c.Name == packet.Channel);
                if(channel == null) {
                    channel = new TatakuChannel(packet.Channel, this);
                    this.channels.Add(channel);
                    this.ChannelAdded?.Invoke(this, channel);
                }

                TatakuMessage message = new(Users.First(u => u.Id == packet.SenderId), packet.Message);
                
                channel.AddMessageToHistory(message);
                break;
            }
            case TatakuPacketId.ServerUserJoined: {
                ServerUserJoinedPacket packet = new();
                packet.ReadDataFromStream(reader);

                TatakuUser user = new() {
                    Id = packet.UserId,
                    Username = packet.Username,
                };
                Users.Add(user);
                
                Logger.Log($"User {user} logged in", LoggerLevelTatakuInfo.Instance);
                break;
            }
            case TatakuPacketId.ServerUserLeft: {
                ServerUserLeftPacket packet = new();
                packet.ReadDataFromStream(reader);

                TatakuUser user = Users.First(u => u.Id == packet.UserId);
                Users.Remove(user);
                Logger.Log($"User {user} logged out", LoggerLevelTatakuInfo.Instance);
                break;
            }
            default: {
                throw new NotImplementedException(packetId.ToString());
            }
        }
    }

    public void SendMessage(string channel, string message) {
        this.PacketQueue.Enqueue(new ClientSendMessagePacket(channel, message));
    }
        
    public event EventHandler<IChannel>? ChannelAdded;
}