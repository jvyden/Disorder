using System.Net;
using System.Net.Sockets;

namespace Disorder.IRC; 

public class IRCGuild : IGuild {
    public readonly string Uri;
    internal readonly TcpClient Client;

    private IRCStream? stream;
    internal IRCStream Stream {
        get {
            this.stream ??= new IRCStream(this.Client.GetStream());
            return this.stream;
        }
    }

    public IRCGuild(string uri, bool ssl = false, string? name = null) {
        this.Uri = uri;
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

        this.Stream.RunIRCCommand("NICK jvyden2");
        this.Stream.RunIRCCommand("USER jvyden2 * * :jvyden2");
    }
    public string Name { get; set; }
    public long Id { get; set; }

    public IEnumerable<IChannel> Channels => new List<IChannel>();

    private bool connected;
    public async Task Process() {
        string[] lines = this.Stream.ReadData().Split("\r\n");

        foreach(string line in lines) {
            if(line.StartsWith("PING")) {
                this.Stream.RunIRCCommand(line.Replace("PING", "PONG"));
                if(!connected) {
                    this.Stream.RunIRCCommand("JOIN #asdjhkg");
                    connected = true;
                }
            }
            Console.WriteLine("line: " + line);
        }
    }
}