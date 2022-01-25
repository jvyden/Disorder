using Eto.Forms;

namespace Disorder.Gui.ListItems; 

public class ChatClientListItem : ListItem {
    public IChatClient ChatClient;
    
    public ChatClientListItem(IChatClient chatClient) {
        this.ChatClient = chatClient;

        this.Text = chatClient.ToString();
    }
}