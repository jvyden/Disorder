using System.Text;

namespace Disorder.TaikoRs.Packets;

public class TaikoRsReader : BinaryReader {
    public TaikoRsReader(Stream input) : base(input, Encoding.UTF8) {}
    public TaikoRsReader(Stream input, bool leaveOpen) : base(input, Encoding.UTF8, leaveOpen) {}

    public override string ReadString() {
        ulong length = this.ReadUInt64();

        byte[] data = this.ReadBytes((int)length);

        return Encoding.UTF8.GetString(data);
    }

    public TaikoRsPacketId ReadPacketId() {
        TaikoRsPacketId pid = (TaikoRsPacketId)this.ReadUInt16();
        return pid;
    }
}