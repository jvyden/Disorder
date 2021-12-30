using System.Collections.Concurrent;
using System.Text;
using Disorder.Dummy;
using Disorder.IRC;

namespace Disorder;

public static class Program {
    private static readonly ConcurrentQueue<IChatClient> chatClientQueue = new();

    public static async Task Main(string[] args) {
        List<IChatClient> chatClients = new() {
            new IRCChatClient(args[0]),
        };

        foreach(IChatClient chatClient in chatClients) {
            chatClientQueue.Enqueue(chatClient);
            foreach(IGuild guild in chatClient.Guilds) {
                guild.OnLoggedIn += delegate {
                    Console.WriteLine($"{guild} logged in as {chatClient.User}");
                };
            }
        }

        int threads = Math.Min(Environment.ProcessorCount, chatClients.Count);
        Console.WriteLine($"Spinning up {threads} worker threads");

        for(int i = 0; i < threads; i++) {
            await Task.Factory.StartNew(async () => {
                while(true) {
                    await ProcessQueue();
                }
            });
        }
        
        Console.ReadLine();
    }

    public static async Task ProcessQueue() {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if(chatClientQueue.TryDequeue(out IChatClient? chatClient) && chatClient != null) { // Process every guild in every chat client
            try {
                foreach(IGuild guild in chatClient.Guilds) {
                    await guild.Process();
                }
            }
            finally {
                chatClientQueue.Enqueue(chatClient);
            }
        }
        await Task.Delay(10);
    }
}