using Disorder.Dummy;

namespace Disorder;

public static class Program {
    public static async Task Main(string[] args) {
        List<IChatClient> chatClients = new() {
            new DummyChatClient(),
        };

        await chatClients.First().Guilds.First().Channels.First().SendMessage("balls");
        IEnumerable<IMessage> messages = await chatClients.First().Guilds.First().Channels.First().FetchMessages();

        foreach(IChatClient chatClient in chatClients) {
            Console.WriteLine(chatClient);
            foreach(IGuild guild in chatClient.Guilds) {
                Console.WriteLine("├─ " + guild);
                foreach(IChannel channel in guild.Channels) {
                    Console.WriteLine("   ├─ " + channel);
                }
            }
        }
    }
}