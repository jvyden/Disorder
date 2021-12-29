using System.Text;
using Disorder.Dummy;
using Disorder.IRC;

namespace Disorder;

public static class Program {
    public static async Task Main(string[] args) {
        List<IChatClient> chatClients = new() {
            new IRCChatClient(),
        };

        while(true) {
            foreach(IGuild guild in chatClients.First().Guilds) {
                await guild.Process();
            }
            await Task.Delay(25);
        }
    }
}