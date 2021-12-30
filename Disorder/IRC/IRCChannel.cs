namespace Disorder.IRC; 

public class IRCChannel : IChannel {
    public IRCChannel(IRCGuild guild) {
        this.guild = guild;
        this.Guild = guild;
    }
    
    public string Name { get; set; }
    public long Id { get; set; }
    private IRCGuild guild { get; set; }
    
    public IGuild Guild { get; set; }

    public List<IRCMessage> MessageHistory { get; } = new();
    public List<IRCUser> Users { get; } = new();

    public Task<IMessage> SendMessage(string message) {
        guild.Stream.RunIRCCommand($"PRIVMSG {Name} :{message}");
        IRCMessage sentMessage = new(this.guild.ChatClient.User, message);
        
        AddMessageToHistory(sentMessage);
        
        return Task.FromResult<IMessage>(sentMessage);
    }

    public void AddMessageToHistory(IRCMessage message) {
        MessageHistory.Add(message);
        MessageSent?.Invoke(this, message);
    }
    
    public event EventHandler<IMessage>? MessageSent;

    public Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        return Task.FromResult<IEnumerable<IMessage>>(this.MessageHistory.Take(limit));
    }
    
    public Task<IEnumerable<IUser>> FetchUsers() {
        return Task.FromResult<IEnumerable<IUser>>(this.Users);
    }
}