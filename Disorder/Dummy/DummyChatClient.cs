namespace Disorder.Dummy;

public class DummyChatClient : IChatClient {
    public IEnumerable<IGuild> Guilds => new List<IGuild> {
        new DummyGuild(),
    };

    public IUser User { get; } = new DummyUser();
}