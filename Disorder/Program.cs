using System.Collections.Concurrent;
using System.Text;
using Disorder.Dummy;
using Disorder.IRC;

namespace Disorder;

public static class Program {
    public static void Main(string[] args) {
        List<IChatClient> chatClients = new() {
            new IRCChatClient(args[0]),
        };

        ChatClientManager.Initialize(chatClients);

        Console.ReadLine();
    }
}