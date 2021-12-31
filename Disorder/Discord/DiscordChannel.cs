using Discord;
using Kettu;

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
        Logger.Log("Fetching messages", LoggerLevelDiscordInfo.Instance);
        IReadOnlyList<global::Discord.DiscordMessage>? messages = await this.Channel.GetMessagesAsync(new MessageFilters {
            Limit = (uint?)limit,
        });

        List<DiscordMessage> outMessages = new();
        if(messages != null) {
            if(messages.Count == 0) Logger.Log("No messages in response", LoggerLevelDiscordInfo.Instance);
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
            Logger.Log("Messages null", LoggerLevelDiscordWarning.Instance);
        }
        return outMessages;
    }
    public async Task<IEnumerable<IUser>> FetchUsers() {
        throw new NotImplementedException();
    }
}