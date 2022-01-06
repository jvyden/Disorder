namespace Disorder.Dummy;

public class DummyChatClient : IChatClient {
    public DummyChatClient() {
        this.OnLoggedIn?.Invoke(this, null);
    }

    public IEnumerable<IGuild> Guilds { get; } = new List<IGuild> {
        new DummyGuild(),
    };

    public IUser User { get; } = new DummyUser();
    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
}
