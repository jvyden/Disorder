namespace Disorder.Discord; 

public class DiscordMessage : IMessage {
    public global::Discord.DiscordMessage Message;
    
    public DiscordMessage(global::Discord.DiscordMessage message, DiscordUser author) {
        this.Message = message;
        this.Author = author;
    }

    public IUser Author { get; }
    public long Id { get; set; }
    public string Content { get; set; }
}