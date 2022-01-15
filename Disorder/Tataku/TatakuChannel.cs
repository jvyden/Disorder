namespace Disorder.Tataku; 

public class TatakuChannel : IChannel {
    public TatakuChannel(string name, TatakuGuild guild) {
        this.Name = name;
        this.guild = guild;
    }

    public string Name { get; set; }
    public long Id { get; set; }
    
    private TatakuGuild guild;
    
    public IGuild Guild {
        get => this.guild;
        set => this.guild = (TatakuGuild)value;
    }

    public List<TatakuMessage> MessageHistory { get; } = new();
    public List<TatakuUser> Users => this.guild.Users;

    public async Task<IMessage> SendMessage(string message) {
        this.guild.SendMessage(this.Name, message);

        return null;
    }
    
    public event EventHandler<IMessage>? MessageSent;
    public Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        return Task.FromResult<IEnumerable<IMessage>>((IEnumerable<IMessage>)this.MessageHistory.Take(limit));
    }

    public Task<IEnumerable<IUser>> FetchUsers() {
        return Task.FromResult<IEnumerable<IUser>>(this.Users);
    }

    public void AddMessageToHistory(TatakuMessage message) {
        this.MessageHistory.Add(message);
        this.MessageSent?.Invoke(this, message);
    }
}