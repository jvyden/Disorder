namespace Disorder.Tataku.Packets;

public class ClientUserLoginPacket : TatakuPacket {
    public ClientUserLoginPacket(string username, string password) {
        this.Username = username;
        this.Password = password;
        this.ProtocolVersion = 1;
        this.Game = "Disorder\nN/A";
    }
    public override TatakuPacketId PacketId => TatakuPacketId.ClientUserLogin;

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

public class ServerLoginResponsePacket : TatakuPacket {
    public override TatakuPacketId PacketId => TatakuPacketId.ServerLoginResponse;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("status", DataType.Byte),
        ("user_id", DataType.UInt),
    };

    public TatakuLoginStatus LoginStatus {
        get => (TatakuLoginStatus)this.Data["status"];
        set => this.Data["status"] = value;
    }

    public uint UserId {
        get => (uint)this.Data["user_id"];
        set => this.Data["user_id"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ClientStatusUpdatePacket : TatakuPacket {
    public ClientStatusUpdatePacket(UserAction action) => this.Action = action;
    public override TatakuPacketId PacketId => TatakuPacketId.ClientStatusUpdate;

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

public class ClientSendMessagePacket : TatakuPacket {
    public ClientSendMessagePacket(string channel, string message) {
        this.Channel = channel;
        this.Message = message;
    }
    public override TatakuPacketId PacketId => TatakuPacketId.ClientSendMessage;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("channel", DataType.String),
        ("message", DataType.String),
    };

    public string Channel {
        get => (string)this.Data["channel"];
        set => this.Data["channel"] = value;
    }

    public string Message {
        get => (string)this.Data["message"];
        set => this.Data["message"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ServerSendMessagePacket : TatakuPacket {
    public override TatakuPacketId PacketId => TatakuPacketId.ServerSendMessage;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("sender_id", DataType.UInt),
        ("channel", DataType.String),
        ("message", DataType.String),
    };

    public uint SenderId {
        get => (uint)this.Data["sender_id"];
        set => this.Data["sender_id"] = value;
    }

    public string Channel {
        get => (string)this.Data["channel"];
        set => this.Data["channel"] = value;
    }

    public string Message {
        get => (string)this.Data["message"];
        set => this.Data["message"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ServerUserJoinedPacket : TatakuPacket {
    public override TatakuPacketId PacketId => TatakuPacketId.ServerPermissions;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("user_id", DataType.UInt),
        ("username", DataType.String),
    };

    public string Username {
        get => (string)this.Data["username"];
        set => this.Data["username"] = value;
    }

    public uint UserId {
        get => (uint)this.Data["user_id"];
        set => this.Data["user_id"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public class ServerUserLeftPacket : TatakuPacket {
    public override TatakuPacketId PacketId => TatakuPacketId.ServerUserLeft;

    private static readonly List<(string name, DataType type)> DATA_DEFINITION = new() {
        ("user_id", DataType.UInt),
    };

    public uint UserId {
        get => (uint)this.Data["user_id"];
        set => this.Data["user_id"] = value;
    }

    public override List<(string name, DataType type)> DataDefinition => DATA_DEFINITION;
}

public enum TatakuLoginStatus : byte {
    UnknownError = 0,
    Ok = 1,
    BadPassword = 2,
    NoUser = 3,
}