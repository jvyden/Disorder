namespace Disorder.Dummy;

public class DummyGuild : IGuild {

    private bool loggedIn;
    private bool sentChannels;
    
    public string Name { get; set; } = "Dummy Guild";
    public long Id { get; set; } = new Random().Next();

    public IEnumerable<IChannel> Channels { get; }

    public DummyGuild() {
        this.Channels = new List<IChannel> {
            new DummyChannel(this),
            new DummyChannel(this),
        };
    }

    
    public async Task Process() {
        if(!this.loggedIn && this.OnLoggedIn?.GetInvocationList().Length != 0) {
            this.OnLoggedIn?.Invoke(this, null);
            this.loggedIn = true;
        }

        if(!this.sentChannels && this.ChannelAdded?.GetInvocationList().Length != 0) {
            foreach(IChannel channel in this.Channels) {
                this.ChannelAdded?.Invoke(this, channel);
            }
            this.sentChannels = true;
        }
    }
    public event EventHandler? OnLoggedIn;
    public event EventHandler<IChannel>? ChannelAdded;
}