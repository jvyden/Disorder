namespace Disorder.Dummy; 

public class DummyUser : IUser {
    public string Username { get; set; } = "Dummy User";
    public string Nickname { get; set; } = "Dummy Nickname";
    public long Id { get; set; } = new Random().Next();
}