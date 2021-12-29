using System.Net.Sockets;
using System.Text;

namespace Disorder.IRC; 

public class IRCStream {
    private NetworkStream stream;
    
    public IRCStream(NetworkStream stream) {
        this.stream = stream;
    }

    public void RunIRCCommand(string text) {
        byte[] data = Encoding.ASCII.GetBytes(text + "\r\n");
        
        this.stream.Write(data);
        this.stream.Flush();
    }

    public string ReadData() {
        byte[] buffer = new byte[8196];
        stream.Read(buffer, 0, 8196);
        
        return Encoding.UTF8.GetString(buffer);
    } 
}