namespace Disorder.Dummy;

public class DummyChatClient : IChatClient {

    public IEnumerable<IGuild> Guilds { get; } = new List<IGuild> {
        new DummyGuild(),
    };

    public IUser User { get; } = new DummyUser();
    public event EventHandler? GuildsUpdated;
}