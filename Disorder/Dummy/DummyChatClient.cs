namespace Disorder.Dummy;

public class DummyChatClient : IChatClient {
    
    private IEnumerable<IGuild> guilds = new List<IGuild> {
        new DummyGuild(),
    };

    public IEnumerable<IGuild> Guilds => guilds;

    public IUser User { get; } = new DummyUser();
    public event EventHandler? GuildsUpdated;
}