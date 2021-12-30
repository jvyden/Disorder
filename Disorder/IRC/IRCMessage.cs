namespace Disorder.IRC; 

public class IRCMessage : IMessage {
    public IRCMessage(IUser author, string content) {
        this.Author = author;
        this.Content = content;
    }
    public IUser Author { get; }
    public long Id { get; set; }
    public string Content { get; set; }
}