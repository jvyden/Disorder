using Discord;
using Kettu;

namespace Disorder.Discord;

public class DiscordGuild : IGuild {

    public PartialGuild Guild;

    private bool loggedIn;
    public DiscordGuild(PartialGuild guild) {
        this.Guild = guild;
    }

    public string Name { get; set; }
    public long Id { get; set; }
    public IEnumerable<IChannel> Channels { get; }
    public async Task Process() {
        if(!this.loggedIn) {
            IReadOnlyList<GuildChannel>? channels = await this.Guild.GetChannelsAsync();
            if(channels != null)
                foreach(GuildChannel guildChannel in channels)
                    if(guildChannel is TextChannel textChannel) {
                        DiscordChannel channel = new(textChannel, this) {
                            Id = (long)textChannel.Id,
                            Name = "#" + textChannel.Name,
                        };
                        Logger.Log($"Constructed channel {channel.Name} (id: {channel.Id})", LoggerLevelDiscordInfo.Instance);
                        this.ChannelAdded?.Invoke(null, channel);
                    }
            this.loggedIn = true;
        }
    }
    public event EventHandler<IChannel>? ChannelAdded;

    public override string ToString() {
        return $"{nameof(this.Name)}: {this.Name}, {nameof(this.Id)}: {this.Id}";
    }
}
