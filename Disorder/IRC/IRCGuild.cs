using System.Net;
using System.Net.Sockets;

namespace Disorder.IRC; 

public class IRCGuild : IGuild {
    public readonly string Uri;
    internal readonly TcpClient Client;

    public readonly IRCChatClient ChatClient;

    private IRCStream? stream;
    internal IRCStream Stream {
        get {
            this.stream ??= new IRCStream(this.Client.GetStream());
            return this.stream;
        }
    }

    public IRCGuild(string uri, IRCChatClient chatClient, bool ssl = false, string? name = null) {
        this.Uri = uri;
        this.ChatClient = chatClient;
        this.Name = name ?? uri;

        string[] uriSplit = Uri.Split(':');
        string address = uriSplit[0];
        int port;
        if(uriSplit.Length != 2) {
            port = ssl ? 6697 : 6667;
        }
        else {
            port = int.Parse(uriSplit[1]);
        }
        
        this.Client = new TcpClient(address, port);

        this.Stream.RunIRCCommand($"NICK {chatClient.User.Username}");
        this.Stream.RunIRCCommand($"USER {chatClient.User.Username} * * :{chatClient.User.Username}");
    }
    public string Name { get; set; }
    public long Id { get; set; }

    public IEnumerable<IChannel> Channels => new List<IChannel>();

    private bool connected;
    public async Task Process() {
        string[] lines = this.Stream.ReadData().Split("\r\n");

        foreach(string line in lines) this.HandleLine(line);
    }

    public void HandleLine(string line) {
        if(string.IsNullOrWhiteSpace(line)) return;
        string[] split = line.Split(" ");

        string command = split[0].StartsWith(':') ? split[1] : split[0];

        line = line.Substring(line.IndexOf(command, StringComparison.Ordinal));
//        string[] args = line.Split(" ");

        string trail = line.Substring(line.IndexOf(" :", StringComparison.Ordinal) + 1);
        
        Console.WriteLine("Got trail: " + trail);

        switch(command) {
            case "PING": {
                this.Stream.RunIRCCommand(line.Replace("PING", "PONG"));
                if(!connected) {
                    this.Stream.RunIRCCommand("JOIN #asdjhkg");
                    connected = true;
                }
                break;
            }
            default: {
                Console.WriteLine("Unknown command: " + command);
                break;
            }
        }
    }
}