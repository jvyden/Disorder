using System.Net;

namespace Disorder.IRC; 

public class IRCChatClient : IChatClient {
    private readonly List<IRCGuild> guilds;

    public IRCChatClient(string uri) {
        this.guilds = new List<IRCGuild>(1) {
            new(uri, this),
        };
    }
    
    public IEnumerable<IGuild> Guilds => this.guilds;
    public IUser User { get; } = new IRCUser();
}