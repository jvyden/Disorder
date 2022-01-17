using System.Collections.Concurrent;
using System.Diagnostics;
using Disorder.Discord;
using Disorder.Dummy;
using GLib;
using Kettu;

namespace Disorder;

public static class ChatClientManager {
    private static readonly ConcurrentQueue<IChatClient> chatClientQueue = new();

    public static void Initialize(List<IChatClient> chatClients) {
        AppDomain.CurrentDomain.ProcessExit += onExit;
        AppDomain.CurrentDomain.UnhandledException += onExit;
        ExceptionManager.UnhandledException += onExit;

        foreach(IChatClient chatClient in chatClients) {
            chatClientQueue.Enqueue(chatClient);
            chatClient.OnLoggedIn += delegate {
                Logger.Log($"{chatClient} logged in as {chatClient.User}", LoggerLevelDisorderInfo.Instance);
            };
        }

        int threads = Math.Min(Environment.ProcessorCount, chatClients.Count);
        Logger.Log($"Spinning up {threads} worker threads", LoggerLevelDummyInfo.Instance);

        for(int i = 0; i < threads; i++)
            Task.Factory.StartNew(async () => {
                while(true) await processQueue();
            });
    }
    private static void onExit(UnhandledExceptionArgs args) {
        onExit(null, args);
    }

    private static void onExit(object? sender, EventArgs e) {
        Logger.Log("Exiting safely...", LoggerLevelDummyInfo.Instance);
        Settings.Instance.Save();
    }

    private static async Task processQueue() {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if(chatClientQueue.TryDequeue(out IChatClient? chatClient) && chatClient != null) // Process every guild in every chat client
            try {
                foreach(IGuild guild in chatClient.Guilds) await guild.Process();
            }
            catch(Exception ex) {
                Logger.Log($"{ex.GetType()} occured during processing {chatClient.GetType()}!", LoggerLevelDisorderError.Instance);
                foreach(string line in ex.ToString().Split("\n")) {
                    Logger.Log($"{line}", LoggerLevelDiscordError.Instance);
                }
            }
            finally {
                chatClientQueue.Enqueue(chatClient);
            }
        await Task.Delay(10);
    }
}
