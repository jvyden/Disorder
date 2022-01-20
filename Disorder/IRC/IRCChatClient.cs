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

    public string ServerUrl { get; set; } = "localhost";

    public string Username { get; set; } = Environment.UserName;

    public string AutoJoinList { get; set; } = "#general";

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
}
