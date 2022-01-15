namespace Disorder.Tataku; 

public class TatakuMessage : IMessage {
    public TatakuMessage(IUser author, string content) {
        this.Author = author;
        this.Content = content;
    }
    public IUser Author { get; }
    
    public string Content { get; set; }
    
    public long Id { get; set; }
}