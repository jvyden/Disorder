namespace Disorder.Tataku;

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

public enum UserActionType : byte {
    Unknown = 0,
    Idle,
    Ingame,
    Leaving,
    Editing,
}