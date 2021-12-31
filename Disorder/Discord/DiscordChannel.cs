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
        Console.WriteLine("Fetching messages");
        IReadOnlyList<global::Discord.DiscordMessage>? messages = await this.Channel.GetMessagesAsync(new MessageFilters {
            Limit = (uint?)limit,
        });

        List<DiscordMessage> outMessages = new();
        if(messages != null) {
            if(messages.Count == 0) Console.WriteLine("No messages in response");
            foreach(global::Discord.DiscordMessage discordMessage in messages.OrderBy(m => m.SentAt)) {
                DiscordUser author = new(discordMessage.Author.User) {
                    Nickname = discordMessage.Author.User.Username,
                    Username = discordMessage.Author.User.ToString(),
                };

                DiscordMessage message = new(discordMessage, author) {
                    Id = (long)discordMessage.Id,
                    Content = discordMessage.Content,
                };
                
                outMessages.Add(message);
            }
        }
        else {
            Console.WriteLine("Messages null");
        }
        return outMessages;
    }
    public async Task<IEnumerable<IUser>> FetchUsers() {
        throw new NotImplementedException();
    }
}