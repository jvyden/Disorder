namespace Disorder.Dummy;

public class DummyGuild : IGuild {
    private bool sentChannels;

    public DummyGuild() {
        this.Channels = new List<IChannel> {
            new DummyChannel(this),
            new DummyChannel(this),
        };
    }

    public string Name { get; set; } = "Dummy Guild";
    public long Id { get; set; } = new Random().Next();

    public IEnumerable<IChannel> Channels { get; }

    public async Task Process() {
        if(!this.sentChannels && this.ChannelAdded?.GetInvocationList().Length != 0) {
            foreach(IChannel channel in this.Channels) this.ChannelAdded?.Invoke(this, channel);
            this.sentChannels = true;
        }
    }
    public event EventHandler<IChannel>? ChannelAdded;
}
