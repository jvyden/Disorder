namespace Disorder.Tataku; 

public class TatakuChatClient : IChatClient {
//    public TatakuChatClient() {
//        this.guilds.Add(new TatakuGuild("wss://taikors.ayyeve.xyz", this));
//    }

    public string? Username { get; set; }

    public string? Password { get; set; }

    private readonly List<TatakuGuild> guilds = new(1);
    public IEnumerable<IGuild> Guilds => guilds;
    public IUser User { get; set; }
        
    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
}