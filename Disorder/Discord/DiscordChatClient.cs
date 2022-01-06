using Discord;

namespace Disorder.Discord;

public class DiscordChatClient : IChatClient {
    public DiscordClient Client;

    private readonly List<DiscordGuild> guilds = new();

    public DiscordChatClient(string token) {
        // Dirty hack to fix gui
        // TODO: find the actual problem
        Task.Factory.StartNew(() => {
            this.Client = new DiscordClient(token, new DiscordConfig {
                RetryOnRateLimit = true,
            });

            foreach(PartialGuild discordGuild in this.Client.GetGuilds()) {
                DiscordGuild guild = new(discordGuild) {
                    Id = (long)discordGuild.Id,
                    Name = discordGuild.Name,
                };

                this.guilds.Add(guild);
            }
            this.OnLoggedIn?.Invoke(this, null);
        }).Wait();

        this.GuildsUpdated?.Invoke(this, null);
    }

    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; }

    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
}
