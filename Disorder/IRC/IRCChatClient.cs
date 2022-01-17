namespace Disorder.IRC;

public class IRCChatClient : IChatClient {
    private readonly List<IRCGuild> guilds;

    public IRCChatClient(string uri) {
        this.guilds = new List<IRCGuild>(1) {
            new(uri, this),
        };

        this.GuildsUpdated?.Invoke(this, null);
    }

    internal void InvokeLoggedIn() {
        this.OnLoggedIn?.Invoke(this, null);
    }

    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; internal set; } = new IRCUser();
    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
    
    public void Initialize() {}
}
