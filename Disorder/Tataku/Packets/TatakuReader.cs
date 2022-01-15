using System.Text;

namespace Disorder.Tataku.Packets;

public class TatakuReader : BinaryReader {
    public TatakuReader(Stream input) : base(input, Encoding.UTF8) {}
    public TatakuReader(Stream input, bool leaveOpen) : base(input, Encoding.UTF8, leaveOpen) {}

    public override string ReadString() {
        ulong length = this.ReadUInt64();

        byte[] data = this.ReadBytes((int)length);

        return Encoding.UTF8.GetString(data);
    }

    public TatakuPacketId ReadPacketId() {
        TatakuPacketId pid = (TatakuPacketId)this.ReadUInt16();
        return pid;
    }
}