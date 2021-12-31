using Disorder.Discord;
using Disorder.IRC;

namespace Disorder;

public static class Program {
    public static void Main(string[] args) {
        List<IChatClient> chatClients = new() {
//            new IRCChatClient(args[0]),
            new DiscordChatClient(Settings.Instance.DiscordToken),
        };

        ChatClientManager.Initialize(chatClients);

        Console.ReadLine();
    }
}