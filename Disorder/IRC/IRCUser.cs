namespace Disorder.IRC; 

public class IRCUser : IUser {
    // nickname!username@hostname
    public string Username { get; set; } = "jvyden2";
    public string Nickname { get; set; } = "jvyden2";
    public string Hostname { get; set; } = "localhost";
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
        return $"IRCUser (nick:{Nickname} user:{Username} host:{Hostname})";
    }
}