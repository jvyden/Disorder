namespace Disorder;

public interface IUser {
    public string Username { get; set; }
    public string Nickname { get; set; }
    public long Id { get; set; }
}