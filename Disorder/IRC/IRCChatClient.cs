using Newtonsoft.Json;

namespace Disorder.IRC;

public class IRCChatClient : IChatClient {
    private List<IRCGuild> guilds;

    internal void InvokeLoggedIn() {
        this.OnLoggedIn?.Invoke(this, null);
    }

    public IEnumerable<IGuild> Guilds => this.guilds;
    
    [JsonIgnore]
    public IUser User { get; internal set; }
    
    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
    
    [ConfigurableProperty("Server Url")]
    public string ServerUrl { get; set; } = "localhost";

    [ConfigurableProperty("Nickname")]
    public string Username { get; set; } = Environment.UserName;

    [ConfigurableProperty("Auto-join list")]
    public string AutoJoinList { get; set; } = "#general";

    [ConfigurableProperty("Password (leave blank for none)")]
    public string? Password { get; set; } = null;

    public void Initialize() {
        this.User = new IRCUser {
            Username = Username,
            Nickname = Username,
        };
        
        this.guilds = new List<IRCGuild>(1) {
            new(ServerUrl, this),
        };

        this.GuildsUpdated?.Invoke(this, null);
    }

    public override string ToString() {
        return $"{nameof(IRCChatClient)} (url: {this.ServerUrl})";
    }
}
