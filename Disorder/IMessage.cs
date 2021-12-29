namespace Disorder;

public interface IMessage {
    public IUser Author { get; }
    public long Id { get; set; }
    public string Content { get; set; }
}