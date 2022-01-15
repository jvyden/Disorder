using Disorder.Discord;
using Disorder.IRC;
using Kettu;

namespace Disorder;

public static class Program {
    public static void Main(string[] args) {
        Logger.AddLogger(new ConsoleLogger());
        Logger.StartLogging();

        List<IChatClient> chatClients = new() {
            new IRCChatClient(args[0]),
//            new DiscordChatClient(Settings.Instance.DiscordToken),
        };

        ChatClientManager.Initialize(chatClients);

        Console.ReadLine();

        Logger.StopLogging().Wait();
    }
}
