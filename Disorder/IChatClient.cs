namespace Disorder; 

public interface IChatClient {
    public IEnumerable<IGuild> Guilds { get; }
}