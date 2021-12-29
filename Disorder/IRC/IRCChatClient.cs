using System.Net;

namespace Disorder.IRC; 

public class IRCChatClient : IChatClient {
    private readonly List<IRCGuild> guilds = new() {
        new IRCGuild("irc.volatile.bz"),
        new IRCGuild("irc.volatile.bz"),
        new IRCGuild("irc.volatile.bz"),
    };
    public IEnumerable<IGuild> Guilds => this.guilds;
}