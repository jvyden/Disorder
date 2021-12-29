namespace Disorder.Dummy; 

public class DummyUser : IUser {
    public string Username { get; set; } = "Dummy User";
    public long Id { get; set; } = new Random().Next();
}