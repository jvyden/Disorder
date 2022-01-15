namespace Disorder.Tataku; 

public class TatakuUser : IUser {
    public string Username { get; set; }

    public string Nickname {
        get => Username;
        set => Username = value;
    }
    
    public long Id { get; set; }
    public override string ToString() {
        return $"TatakuUser ({nameof(this.Username)}: {this.Username}, {nameof(this.Nickname)}: {this.Nickname})";
    }
}