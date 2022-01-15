using System.Text;

namespace Disorder.Tataku.Packets;

public class TatakuWriter : BinaryWriter {
    public TatakuWriter(Stream input) : base(input, Encoding.UTF8) {}
    public TatakuWriter(Stream input, bool leaveOpen) : base(input, Encoding.UTF8, leaveOpen) {}

    public override void Write(string value) {
        byte[] bytes = Encoding.UTF8.GetBytes(value);

        ulong length = (ulong)bytes.LongLength;

        this.Write(length);
        this.Write(bytes);
    }
}