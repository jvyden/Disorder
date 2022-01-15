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

public enum LoginStatus : byte {
    UnknownError = 0,
    Ok = 1,
    BadPassword = 2,
    NoUser = 3,
}