namespace Disorder.Dummy; 

public class DummyGuild : IGuild {
    public string Name { get; set; } = "Dummy Guild";
    public long Id { get; set; } = new Random().Next();

    public IEnumerable<IChannel> Channels => new List<IChannel> {
        new DummyChannel(this),
        new DummyChannel(this),
    };

    private bool loggedIn = false;
    public async Task Process() {
        if(!this.loggedIn && this.OnLoggedIn != null) {
            this.OnLoggedIn?.Invoke(this, null);
            this.loggedIn = true;
        }
    }
    public event EventHandler? OnLoggedIn;
    public event EventHandler<IChannel>? ChannelAdded;
}