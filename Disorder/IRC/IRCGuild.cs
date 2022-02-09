using System.Net.Sockets;
using Kettu;

namespace Disorder.IRC;

public class IRCGuild : IGuild {

    private readonly List<IRCChannel> channels = new();

    public readonly IRCChatClient ChatClient;
    internal readonly TcpClient Client;
    public readonly string Uri;

    private IRCStream? stream;

    public List<IRCUser> Users = new();

    public IRCGuild(string uri, IRCChatClient chatClient, bool ssl = false, string? name = null) {
        this.Uri = uri;
        this.ChatClient = chatClient;
        this.Name = name ?? uri;

        string[] uriSplit = this.Uri.Split(':');
        string address = uriSplit[0];
        int port;
        if(uriSplit.Length != 2) port = ssl ? 6697 : 6667;
        else port = int.Parse(uriSplit[1]);

        this.Client = new TcpClient(address, port);

        if(this.ChatClient.Password != null) {
            this.Stream.RunIRCCommand($"PASS {this.ChatClient.Password}");
        }

        this.Stream.RunIRCCommand($"NICK {this.ChatClient.User.Nickname}");
        this.Stream.RunIRCCommand($"USER {this.ChatClient.User.Username} * * :{this.ChatClient.User.Username}");
    }

    internal IRCStream Stream {
        get {
            this.stream ??= new IRCStream(this.Client.GetStream());
            return this.stream;
        }
    }

    public string Name { get; set; }
    public long Id { get; set; }
    public event EventHandler<IChannel>? ChannelAdded;

    public IEnumerable<IChannel> Channels => this.channels;

    public async Task Process() {
        List<string> lines = new();

        string? readLine;
        while((readLine = this.Stream.ReadLine()) != null) lines.Add(readLine);

        foreach(string line in lines) this.HandleLine(line);
    }

    private void autoJoin() {
        string[] autoJoinChannels = this.ChatClient.AutoJoinList.Split(',');

        foreach(string channel in autoJoinChannels) {
            Logger.Log("Auto-joining " + channel.Trim(), LoggerLevelIRCInfo.Instance);
            this.Stream.RunIRCCommand("JOIN " + channel.Trim());
        }
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

                Logger.Log($"({command}) {origin}: {trail}", LoggerLevelIRCInfo.Instance);
                break;
            }
            case "001": { // Registered
                this.ChatClient.InvokeLoggedIn();

//                this.ChatClient.User = IRCUser.FromCloak(trail.Substring(trail.LastIndexOf(' ') + 1));
                this.Users.Add((IRCUser)this.ChatClient.User);
                this.autoJoin();
                break;
            }
            case "432":
            case "433": { // ERR_NICKNAMEINUSE
                Logger.Log($"Nick invalid, changing... ({command} :{trail})", LoggerLevelIRCInfo.Instance);
                this.Stream.RunIRCCommand($"NICK {this.ChatClient.User.Nickname + new Random().Next(0, 999)}");
                Task.Factory.StartNew(() => {
                    Thread.Sleep(1000);
                    this.autoJoin();
                });
                break;
            }
            case "JOIN": {
                string channelName = split[2].TrimStart(':');

                IRCUser joinedUser = IRCUser.FromCloak(origin);
                IRCChannel? joinedChannel = this.channels.FirstOrDefault(c => c.Name == channelName);

                if(joinedChannel == null) {
                    joinedChannel = new IRCChannel(this) {
                        Name = channelName,
                    };

                    this.channels.Add(joinedChannel);
                    this.ChannelAdded?.Invoke(this, joinedChannel);
                }

                joinedChannel.Users.Add(joinedUser);

                if(this.Users.FirstOrDefault(u => Equals(u, joinedUser)) == null) this.Users.Add(joinedUser);

                Logger.Log($"{joinedUser} joined {joinedChannel}", LoggerLevelIRCInfo.Instance);
                break;
            }
            case "ERROR": {
                Logger.Log(trail, LoggerLevelIRCError.Instance);
                break;
            }
            case "NICK": {
                IRCUser oldUser = IRCUser.FromCloak(origin);

                IRCUser? userInGuild = this.Users.FirstOrDefault(u => u.Username == oldUser.Username);
                if(userInGuild == null) return;

                userInGuild.Nickname = trail;

                break;
            }
            default: {
                Logger.Log($"Unknown IRC command: '{command}'", LoggerLevelIRCWarning.Instance);
                break;
            }
        }
    }
}
