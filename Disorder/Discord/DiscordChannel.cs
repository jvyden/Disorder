using Discord;

namespace Disorder.Discord; 

public class DiscordChannel : IChannel {
    public TextChannel Channel;
    
    public DiscordChannel(TextChannel channel, DiscordGuild guild) {
        this.Channel = channel;
        this.Guild = guild;
    }

    public string Name { get; set; }
    public long Id { get; set; }
    public IGuild Guild { get; set; }
    public async Task<IMessage> SendMessage(string message) {
        await this.Channel.SendMessageAsync(message);
        return null;
    }
    
    public event EventHandler<IMessage>? MessageSent;
    public async Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<IUser>> FetchUsers() {
        throw new NotImplementedException();
    }
}