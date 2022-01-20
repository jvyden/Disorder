using Newtonsoft.Json;

namespace Disorder.Tataku; 

public class TatakuChatClient : IChatClient {
    public string Username { get; set; }
    
    public string Password { get; set; }
    
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
}