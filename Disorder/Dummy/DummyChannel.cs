namespace Disorder.Dummy; 

public class DummyChannel : IChannel {
    public string Name { get; set; } = "Dummy Channel";
    public long Id { get; set; } = new Random().Next();
    public async Task<IMessage> SendMessage(string message) {
        Console.WriteLine($"Sending dummy message '{message}' to {this}");

        return new DummyMessage(new DummyUser(), message);
    }
    public async Task<IEnumerable<IMessage>> FetchMessages(int limit = 50) {
        return new List<IMessage> {
            new DummyMessage(new DummyUser(), new Random().Next().ToString()),
            new DummyMessage(new DummyUser(), new Random().Next().ToString()),
            new DummyMessage(new DummyUser(), new Random().Next().ToString()),
            new DummyMessage(new DummyUser(), new Random().Next().ToString()),
            new DummyMessage(new DummyUser(), new Random().Next().ToString()),
        };
    }
    public async Task<IEnumerable<IUser>> FetchUsers() {
        return new List<IUser> {
            new DummyUser(),
            new DummyUser(),
            new DummyUser(),
            new DummyUser(),
            new DummyUser(),
        };
    }

    public override string ToString() {
        return $"DummyChannel (Name: {this.Name}, Id: {this.Id})";
    }
}