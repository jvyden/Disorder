using System.Collections.Concurrent;
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
    public TaikoRsGuild(string uri, TaikoRsChatClient chatClient) {
        this.ChatClient = chatClient;
        this.Name = uri;

        this.client = new WebSocket(uri);
        this.client.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
        
        this.client.OnOpen += delegate {
            Logger.Log("Connected to " + uri, LoggerLevelTaikoRsInfo.Instance);

            string username = Settings.Instance.TaikoRsUsername;
            string password = HashHelper.Sha512Hash(Encoding.UTF8.GetBytes(Settings.Instance.TaikoRsPassword));
            
            this.PacketQueue.Enqueue(new ClientUserLoginPacket(username, password));
        };
        this.client.OnMessage += delegate(object? _, MessageEventArgs args) {
            Logger.Log("Got data: " + args.Data, LoggerLevelTaikoRsInfo.Instance);
        };

        this.client.Connect();
    }
        
    public string Name { get; set; }
    public long Id { get; set; }
    public IEnumerable<IChannel> Channels { get; }

    public readonly ConcurrentQueue<TaikoRsPacket?> PacketQueue = new();

    private void packetSendMain() {
        if(this.PacketQueue.TryDequeue(out TaikoRsPacket? packet) && this.client.IsAlive) {
            if(packet == null) return;
            
            using MemoryStream s = new();
            using TaikoRsWriter w = new(s);
            
            packet.WriteDataToStream(w);
            this.client.Send(s.ToArray());
        }
    }

    public async Task Process() {
        this.packetSendMain();
    }
        
    public event EventHandler<IChannel>? ChannelAdded;
}