namespace Disorder.Dummy; 

public class DummyMessage : IMessage {
    public DummyMessage(IUser author, string content) {
        this.Author = author;
        this.Content = content;
    }
    
    public IUser Author { get; }
    public long Id { get; set; } = new Random().Next();
    public string Content { get; set; }
}