namespace Disorder.Dummy; 

public class DummyChannel : IChannel {
    public string Name { get; set; } = "Dummy Channel";
    public long Id { get; set; } = new Random().Next();
    public Task SendMessage(string message) {
        Console.WriteLine($"Sending dummy message '{message}' to {this}");

        return Task.CompletedTask;
    }

    public override string ToString() {
        return $"DummyChannel (Name: {this.Name}, Id: {this.Id})";
    }
}