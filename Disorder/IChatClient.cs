using Newtonsoft.Json;

namespace Disorder;

public interface IChatClient {
    [JsonIgnore]
    public IEnumerable<IGuild> Guilds { get; }

    [JsonIgnore]
    public IUser User { get; }

    public event EventHandler GuildsUpdated;
    public event EventHandler? OnLoggedIn;

    public void Initialize();

    public string ToString();
}
