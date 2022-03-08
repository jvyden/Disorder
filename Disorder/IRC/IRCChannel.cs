namespace Disorder.IRC;

public class IRCChannel : IChannel {
    public IRCChannel(IRCGuild guild) {
        this.guild = guild;
        this.Guild = guild;
    }
    private IRCGuild guild { get; }

    public List<IRCMessage> MessageHistory { get; } = new();
    public List<IRCUser> Users { get; } = new();

    public string Name { get; set; }
    public long Id { get; set; }

    public IGuild Guild { get; set; }

    public Task<IMessage> SendMessage(string message) {
        this.guild.Stream.RunIRCCommand($"PRIVMSG {this.Name} :{message}");
        IRCMessage sentMessage = new(this.guild.ChatClient.User, message);

        this.AddMessageToHistory(sentMessage);

        return Task.FromResult<IMessage>(sentMessage);
    }

    public event EventHandler<IMessage>? MessageSent;

    public Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        return Task.FromResult<IEnumerable<IMessage>>((IEnumerable<IMessage>)this.MessageHistory.Take(limit));
    }

    public Task<IEnumerable<IUser>> FetchUsers() {
        return Task.FromResult<IEnumerable<IUser>>(this.Users);
    }

    public void AddMessageToHistory(IRCMessage message) {
        this.MessageHistory.Add(message);
        this.MessageSent?.Invoke(this, message);
    }

    public bool IsNSFW => Name.ToLower().Contains("nsfw");
}