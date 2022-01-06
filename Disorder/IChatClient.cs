namespace Disorder;

public interface IChatClient {
    public IEnumerable<IGuild> Guilds { get; }

    public IUser User { get; }

    public event EventHandler GuildsUpdated;
    public event EventHandler? OnLoggedIn;
}
