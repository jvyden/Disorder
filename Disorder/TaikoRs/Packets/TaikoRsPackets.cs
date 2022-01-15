namespace Disorder.TaikoRs.Packets;

public class ClientUserLoginPacket : TaikoRsPacket {
    public ClientUserLoginPacket(string username, string password) {
        this.Username = username;
        this.Password = password;
        this.ProtocolVersion = 1;
        this.Game = "Disorder\nN/A";
    }
    public override TaikoRsPacketId PacketId => TaikoRsPacketId.ClientUserLogin;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("protocol_version", DataType.UShort),
        ("username", DataType.String),
        ("password", DataType.String),
        ("game", DataType.String),
    };

    public ushort ProtocolVersion {
        get => (ushort)this.Data["protocol_version"];
        set => this.Data["protocol_version"] = value;
    }

    public string Username {
        get => (string)this.Data["username"];
        set => this.Data["username"] = value;
    }

    public string Password {
        get => (string)this.Data["password"];
        set => this.Data["password"] = value;
    }

    public string Game {
        get => (string)this.Data["game"];
        set => this.Data["game"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ServerLoginResponsePacket : TaikoRsPacket {
    public override TaikoRsPacketId PacketId => TaikoRsPacketId.ServerLoginResponse;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("status", DataType.Byte),
        ("user_id", DataType.UInt),
    };

    public TaikoRsLoginStatus LoginStatus {
        get => (TaikoRsLoginStatus)this.Data["status"];
        set => this.Data["status"] = value;
    }

    public uint UserId {
        get => (uint)this.Data["user_id"];
        set => this.Data["user_id"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ClientStatusUpdatePacket : TaikoRsPacket {
    public ClientStatusUpdatePacket(UserAction action) => this.Action = action;
    public override TaikoRsPacketId PacketId => TaikoRsPacketId.ClientStatusUpdate;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("action", DataType.UShort),
        ("action_text", DataType.String),
        ("mode", DataType.Byte),
    };

    public UserAction Action {
        get => new((UserActionType)this.Data["action"], this.Data["action_text"].ToString()!, (byte)this.Data["mode"]);
        set {
            this.Data["action"] = value.Action;
            this.Data["action_text"] = value.ActionText;
            this.Data["mode"] = value.Mode;
        }
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public enum TaikoRsLoginStatus : byte {
    UnknownError = 0,
    Ok = 1,
    BadPassword = 2,
    NoUser = 3,
}