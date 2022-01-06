using Discord;

namespace Disorder.Discord;

public class DiscordChatClient : IChatClient {

    public DiscordClient Client;

    private readonly List<DiscordGuild> guilds = new();
    private string token;

    public DiscordChatClient(string token) {
        this.token = token;

        // Dirty hack to fix gui
        // TODO: find the actual problem
        Task.Factory.StartNew(() => {
            this.Client = new DiscordClient(token, new DiscordConfig {
                RetryOnRateLimit = false,
            });

            foreach(PartialGuild discordGuild in this.Client.GetGuilds()) {
                DiscordGuild guild = new(discordGuild) {
                    Id = (long)discordGuild.Id,
                    Name = discordGuild.Name,
                };

                this.guilds.Add(guild);
            }
        }).Wait();

        this.GuildsUpdated?.Invoke(this, null);
    }

    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; }
    public event EventHandler? GuildsUpdated;
}