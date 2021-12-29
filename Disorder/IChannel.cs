namespace Disorder;

public interface IChannel {
    public string Name { get; set; }
    public long Id { get; set; }

    public Task SendMessage(string message);
}