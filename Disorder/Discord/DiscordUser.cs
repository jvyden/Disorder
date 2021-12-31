namespace Disorder.Discord; 

public class DiscordUser : IUser {
    public global::Discord.DiscordUser User;
    
    public DiscordUser(global::Discord.DiscordUser user) {
        this.User = user;
    }

    public string Username { get; set; }
    public string Nickname { get; set; }
    public long Id { get; set; }
}