namespace Disorder.TaikoRs;

public class UserAction {
    public UserActionType Action = UserActionType.Idle;
    public string ActionText = "";
    public byte Mode;

    public UserAction(UserActionType action, string actionText, byte mode) {
        this.Action = action;
        this.ActionText = actionText;
        this.Mode = mode;
    }

    public UserAction() {}
}

public enum UserActionType : ushort {
    Unknown = 0,
    Idle,
    Ingame,
    Leaving,
    Editing,
}