using Disorder.Dummy;

namespace Disorder;

public static class Program {
    public static void Main(string[] args) {
        IChatClient chatClient = new DummyChatClient();

        chatClient.Guilds.First().Channels.First().SendMessage("test");
    }
}