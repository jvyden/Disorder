using Newtonsoft.Json;

namespace Disorder.Tataku; 

public class TatakuChatClient : IChatClient {
    [ConfigurableProperty("Username")]
    public string Username { get; set; }
    
    [ConfigurableProperty("Password", true)]
    public string Password { get; set; }
    
    [ConfigurableProperty("Server Url")]
    public string ServerUrl { get; set; }

    private readonly List<TatakuGuild> guilds = new(1);
    public IEnumerable<IGuild> Guilds => guilds;
    public IUser User { get; set; }
        
    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
    
    public void Initialize() {
        this.guilds.Add(new TatakuGuild(ServerUrl, this));
        
        this.GuildsUpdated?.Invoke(this, null);
    }
    public override string ToString() {
        return $"{nameof(TatakuChatClient)} (url: {this.ServerUrl})";
    }
}