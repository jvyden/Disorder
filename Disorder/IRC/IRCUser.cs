namespace Disorder.IRC;

public class IRCUser : IUser {
    public string Hostname { get; set; } = "localhost";

    // nickname!username@hostname
    public string Username { get; set; } = Settings.Instance.IrcUsername;
    public string Nickname { get; set; } = Settings.Instance.IrcUsername;
    public long Id { get; set; }

    public static IRCUser FromCloak(string cloak) {
        int indexOfNicknameSplit = cloak.IndexOf('!');
        int indexOfHostnameSplit = cloak.IndexOf('@');

        string nickname = cloak.Substring(0, indexOfNicknameSplit);
        string username = cloak.Substring(indexOfNicknameSplit + 1, indexOfHostnameSplit - indexOfNicknameSplit - 1);
        string hostname = cloak.Substring(indexOfHostnameSplit + 1);

        return new IRCUser {
            Username = username,
            Nickname = nickname,
            Hostname = hostname,
        };
    }

    public override string ToString() {
        return $"IRCUser (nick:{this.Nickname} user:{this.Username} host:{this.Hostname})";
    }

    protected bool Equals(IRCUser other) {
        return this.Hostname == other.Hostname && this.Username == other.Username && this.Nickname == other.Nickname;
    }
    
    public override bool Equals(object? obj) {
        if(ReferenceEquals(null, obj)) return false;
        if(ReferenceEquals(this, obj)) return true;
        if(obj.GetType() != this.GetType()) return false;

        return Equals((IRCUser)obj);
    }
    
    public override int GetHashCode() {
        return HashCode.Combine(this.Hostname, this.Username, this.Nickname);
    }
}