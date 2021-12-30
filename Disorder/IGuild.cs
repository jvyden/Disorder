namespace Disorder;

public interface IGuild {
    public string Name { get; set; }
    public long Id { get; set; }

    public IEnumerable<IChannel> Channels { get; }

    public Task Process();

    public event EventHandler? OnLoggedIn;

    public event EventHandler<IChannel>? ChannelAdded;
}