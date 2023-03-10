using Discord;
using Newtonsoft.Json;

namespace Disorder.Discord;

public class DiscordChatClient : IChatClient {
    [JsonIgnore]
    public DiscordClient Client;

    private readonly List<DiscordGuild> guilds = new();

    [ConfigurableProperty("Token", true)]
    public string Token { get; set; }

    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; }

    public event EventHandler? GuildsUpdated;
    public event EventHandler? OnLoggedIn;
    public void Initialize() {
        Exception? exceptionIfFail = null;

        // Dirty hack to fix gui
        // TODO: find the actual problem
        Task.Factory.StartNew(() => {
            try {
                this.Client = new DiscordClient(Token, new DiscordConfig {
                    RetryOnRateLimit = true,
                });
            }
            catch(Exception ex) {
                exceptionIfFail = ex;
                return;
            }

            foreach(PartialGuild discordGuild in this.Client.GetGuilds()) {
                DiscordGuild guild = new(discordGuild) {
                    Id = (long)discordGuild.Id,
                    Name = discordGuild.Name,
                };

                this.guilds.Add(guild);
            }
            this.OnLoggedIn?.Invoke(this, null);
        }).Wait();

        if(exceptionIfFail != null)
            throw exceptionIfFail;

        this.GuildsUpdated?.Invoke(this, null);
    }

    public override string ToString() {
        return $"{nameof(DiscordChatClient)} (u: {User?.Username})";
    }
}
