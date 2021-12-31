namespace Disorder.IRC;

public struct IRCMessage : IMessage {
    public IRCMessage(IUser author, string content) {
        this.Author = author;
        this.Content = content;

        this.Id = 0;
    }
    public IUser Author { get; }
    public long Id { get; set; }
    public string Content { get; set; }
}