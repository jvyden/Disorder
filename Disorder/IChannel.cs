using Newtonsoft.Json;

namespace Disorder;

public interface IChannel {
    public string Name { get; set; }
    public long Id { get; set; }

    [JsonIgnore]
    public IGuild Guild { get; set; }

    public Task<IMessage> SendMessage(string message);

    public event EventHandler<IMessage>? MessageSent;

    public Task<IEnumerable<IMessage>> FetchMessages(int limit = 50);

    public Task<IEnumerable<IUser>> FetchUsers();
    
    public bool IsNSFW { get; }
}