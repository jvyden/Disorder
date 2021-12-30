namespace Disorder.IRC; 

public class IRCChannel : IChannel {
    public IRCChannel(IRCGuild guild) {
        this.Guild = guild;
    }
    
    public string Name { get; set; }
    public long Id { get; set; }
    public IRCGuild Guild { get; set; }

    public List<IRCMessage> MessageHistory { get; } = new();
    public List<IRCUser> Users { get; } = new();

    public Task<IMessage> SendMessage(string message) {
        Guild.Stream.RunIRCCommand($"PRIVMSG {Name} :{message}");
        IRCMessage sentMessage = new(this.Guild.ChatClient.User, message);
        
        MessageHistory.Add(sentMessage);
        return Task.FromResult<IMessage>(sentMessage);
    }
    
    public Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        return Task.FromResult<IEnumerable<IMessage>>(this.MessageHistory.Take(limit));
    }
    
    public Task<IEnumerable<IUser>> FetchUsers() {
        return Task.FromResult<IEnumerable<IUser>>(this.Users);
    }
}