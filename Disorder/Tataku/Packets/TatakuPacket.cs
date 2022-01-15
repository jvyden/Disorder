namespace Disorder.Tataku.Packets; 

public abstract class TatakuPacket {
    protected Dictionary<string, object> Data = new();
    public virtual TatakuPacketId PacketId => TatakuPacketId.Unknown;

    protected static readonly List<(string name, DataType type)> BlankDataDefinition = new();
    public virtual List<(string name, DataType type)> DataDefinition => BlankDataDefinition;
    
    public void ReadDataFromStream(TatakuReader reader) {
        foreach ((string name, DataType type) in this.DataDefinition)
            switch (type) {
                case DataType.Byte:
                    this.Data.Add(name, reader.ReadByte());
                    break;
                case DataType.SByte:
                    this.Data.Add(name, reader.ReadSByte());
                    break;
                case DataType.UShort:
                    this.Data.Add(name, reader.ReadUInt16());
                    break;
                case DataType.Short:
                    this.Data.Add(name, reader.ReadInt16());
                    break;
                case DataType.UInt:
                    this.Data.Add(name, reader.ReadUInt32());
                    break;
                case DataType.Int:
                    this.Data.Add(name, reader.ReadInt32());
                    break;
                case DataType.ULong:
                    this.Data.Add(name, reader.ReadUInt64());
                    break;
                case DataType.Long:
                    this.Data.Add(name, reader.ReadInt64());
                    break;
                case DataType.Single:
                    this.Data.Add(name, reader.ReadSingle());
                    break;
                case DataType.Double:
                    this.Data.Add(name, reader.ReadDouble());
                    break;
                case DataType.String:
                    this.Data.Add(name, reader.ReadString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }
    
    public void WriteDataToStream(TatakuWriter writer) {
        #region Header

        writer.Write((ushort)this.PacketId);

        #endregion

        #region Data

        foreach ((string name, DataType type) in this.DataDefinition)
            switch (type) {
                case DataType.Byte:
                    writer.Write((byte)this.Data[name]);
                    break;
                case DataType.SByte:
                    writer.Write((sbyte)this.Data[name]);
                    break;
                case DataType.UShort:
                    writer.Write((ushort)this.Data[name]);
                    break;
                case DataType.Short:
                    writer.Write((short)this.Data[name]);
                    break;
                case DataType.UInt:
                    writer.Write((uint)this.Data[name]);
                    break;
                case DataType.Int:
                    writer.Write((int)this.Data[name]);
                    break;
                case DataType.ULong:
                    writer.Write((ulong)this.Data[name]);
                    break;
                case DataType.Long:
                    writer.Write((long)this.Data[name]);
                    break;
                case DataType.Single:
                    writer.Write((float)this.Data[name]);
                    break;
                case DataType.Double:
                    writer.Write((double)this.Data[name]);
                    break;
                case DataType.String:
                    writer.Write((string)this.Data[name]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        #endregion
    }
}