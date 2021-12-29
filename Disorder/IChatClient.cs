namespace Disorder; 

public interface IChatClient {
    public IEnumerable<IGuild> Guilds { get; }
    
    public IUser User { get; }
}