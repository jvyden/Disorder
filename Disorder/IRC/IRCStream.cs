using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Disorder.IRC; 

public class IRCStream {
    private readonly NetworkStream stream;

    public IRCStream(NetworkStream stream) {
        this.stream = stream;
    }

    public void RunIRCCommand(string text) {
        this.stream.Write(Encoding.UTF8.GetBytes(text + "\r\n"));
        this.stream.Flush();
    }

    public string? ReadLine() {
        List<byte> buffer = new();
        if(!this.stream.CanRead || !this.stream.DataAvailable) {
            return null;
        }

        byte lastByte = 0x00;
        while(this.stream.DataAvailable) { // While data is available, read
            int thisByte = this.stream.ReadByte();
            if(thisByte == -1) throw new ArgumentNullException();

            if(lastByte == '\r' && (byte)thisByte == '\n') {
                buffer.Add((byte)thisByte);
                break;
            }
            
            buffer.Add(lastByte = (byte)thisByte);
        }

        return Encoding.UTF8.GetString(buffer.ToArray()).TrimEnd();
    }
}