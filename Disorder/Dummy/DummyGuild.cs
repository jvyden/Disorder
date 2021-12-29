namespace Disorder.Dummy; 

public class DummyGuild : IGuild {
    public string Name { get; set; } = "Dummy Guild";
    public long Id { get; set; } = new Random().Next();

    public IEnumerable<IChannel> Channels => new List<IChannel> {
        new DummyChannel(),
        new DummyChannel(),
    };
}