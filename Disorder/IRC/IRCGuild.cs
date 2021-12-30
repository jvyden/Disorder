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

        this.Stream.RunIRCCommand($"NICK {this.ChatClient.User.Nickname}");
        this.Stream.RunIRCCommand($"USER {this.ChatClient.User.Username} * * :{this.ChatClient.User.Username}");
    }
    
    public string Name { get; set; }
    public long Id { get; set; }
    public event EventHandler? OnLoggedIn;
    public event EventHandler<IChannel>? ChannelAdded;

    private readonly List<IRCChannel> channels = new();

    public IEnumerable<IChannel> Channels => this.channels;
    
    public async Task Process() {
        List<string> lines = new();
        
        string? readLine;
        while((readLine = this.Stream.ReadLine()) != null) lines.Add(readLine);

        foreach(string line in lines) this.HandleLine(line);
    }

    public void HandleLine(string line) {
        if(string.IsNullOrWhiteSpace(line) || line.StartsWith((char)0x00)) return;
        string[] split = line.Split(" ");

        string command = split[0].StartsWith(':') ? split[1] : split[0];
        if(string.IsNullOrWhiteSpace(command)) return;

        line = line.Substring(line.IndexOf(command, StringComparison.Ordinal));

        string trail = line.Substring(line.IndexOf(" :", StringComparison.Ordinal) + 1);
        if(trail.StartsWith(':')) trail = trail.Substring(1);

        string origin = string.Empty;
        if(split[0].StartsWith(':')) origin = split[0].Substring(1);

        switch(command) {
            case "PING": {
                this.Stream.RunIRCCommand(line.Replace("PING", "PONG"));
                break;
            }
            case "002": // RPL_YOURHOST
            case "003": // RPL_CREATED
            case "004": // RPL_MYINFO
            case "005": // RPL_BOUNCE
            case "372": // RPL_MOTD
            case "251": // RPL_LUSERCLIENT
            case "PRIVMSG":
            case "NOTICE": {
                if(command == "PRIVMSG") {
                    IRCChannel? channel = this.channels.FirstOrDefault(c => c.Name == split[2]);
                    if(channel != null) {
                        IRCUser userFromMessage = IRCUser.FromCloak(origin);
                        IRCUser? userInChannel = channel.Users.FirstOrDefault(u => u.Nickname == userFromMessage.Nickname);
                        
                        if(userInChannel == null) channel.Users.Add(userInChannel = userFromMessage);
                        
                        channel.AddMessageToHistory(new IRCMessage(userInChannel, trail));

                        if(trail.StartsWith("!say ")) channel.SendMessage(trail.Substring(5));
                    }
                }
                
                Console.WriteLine($"({command}) {origin}: {trail}");
                break;
            }
            case "001": { // Registered
                this.OnLoggedIn?.Invoke(this, null);
                this.Stream.RunIRCCommand("JOIN #asdjhkg");
                break;
            }
            case "432":
            case "433": { // ERR_NICKNAMEINUSE
                Console.WriteLine($"Nick invalid, changing... ({command} :{trail})");
                this.Stream.RunIRCCommand($"NICK {this.ChatClient.User.Nickname + new Random().Next(0, 999)}");
                this.Stream.RunIRCCommand("JOIN #asdjhkg");
                break;
            }
            case "JOIN": {
                string channelName = split[2].TrimStart(':');

                IRCUser joinedUser = IRCUser.FromCloak(origin);
                IRCChannel joinedChannel = new(this) {
                    Name = channelName,
                };
                
                joinedChannel.Users.Add(joinedUser);

                this.channels.Add(joinedChannel);
                this.ChannelAdded?.Invoke(this, joinedChannel);

                Console.WriteLine($"{joinedUser} joined {joinedChannel}");
                break;
            }
            case "ERROR": {
                Console.WriteLine("!!!!! IRC ERROR !!!!!");
                Console.WriteLine($"{trail}");
                break;
            }
            default: {
                Console.WriteLine($"Unknown IRC command: '{command}'");
                break;
            }
        }
    }
}