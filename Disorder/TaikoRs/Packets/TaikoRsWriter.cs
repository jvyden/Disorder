using System.Text;

namespace Disorder.TaikoRs.Packets;

public class TaikoRsWriter : BinaryWriter {
    public TaikoRsWriter(Stream input) : base(input, Encoding.UTF8) {}
    public TaikoRsWriter(Stream input, bool leaveOpen) : base(input, Encoding.UTF8, leaveOpen) {}

    public override void Write(string value) {
        byte[] bytes = Encoding.UTF8.GetBytes(value);

        ulong length = (ulong)bytes.LongLength;

        this.Write(length);
        this.Write(bytes);
    }
}