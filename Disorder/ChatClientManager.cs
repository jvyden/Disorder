using System.Collections.Concurrent;
using GLib;

namespace Disorder;


public static class ChatClientManager {
    private static readonly ConcurrentQueue<IChatClient> chatClientQueue = new();

    public static void Initialize(List<IChatClient> chatClients) {
        AppDomain.CurrentDomain.ProcessExit += onExit;
        AppDomain.CurrentDomain.UnhandledException += onExit;
        GLib.ExceptionManager.UnhandledException += onExit;
        
        foreach(IChatClient chatClient in chatClients) {
            chatClientQueue.Enqueue(chatClient);
            foreach(IGuild guild in chatClient.Guilds)
                guild.OnLoggedIn += delegate {
                    Console.WriteLine($"{guild} logged in as {chatClient.User}");
                };
        }

        int threads = Math.Min(Environment.ProcessorCount, chatClients.Count);
        Console.WriteLine($"Spinning up {threads} worker threads");

        for(int i = 0; i < threads; i++)
            Task.Factory.StartNew(async () => {
                while(true) await processQueue();
            });
    }
    private static void onExit(UnhandledExceptionArgs args) {
        onExit(null, args);
    }

    private static void onExit(object? sender, EventArgs e) {
        Console.WriteLine("Exiting safely...");
        Settings.Instance.Save();
    }

    private static async Task processQueue() {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if(chatClientQueue.TryDequeue(out IChatClient? chatClient) && chatClient != null) // Process every guild in every chat client
            try {
                foreach(IGuild guild in chatClient.Guilds) await guild.Process();
            }
            finally {
                chatClientQueue.Enqueue(chatClient);
            }
        await Task.Delay(10);
    }
}