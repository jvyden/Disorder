using System.Net;

namespace Disorder.IRC; 

public class IRCChatClient : IChatClient {
    private readonly List<IRCGuild> guilds;

    public IRCChatClient() {
        this.guilds = new List<IRCGuild> {
            new("localhost", this),
        };
    }
    
    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; } = new IRCUser();
}