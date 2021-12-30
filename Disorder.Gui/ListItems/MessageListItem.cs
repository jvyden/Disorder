using Eto.Forms;

namespace Disorder.Gui.ListItems;

public class MessageListItem : ListItem {
    public IMessage Message;

    public MessageListItem(IMessage message) {
        this.Message = message;

        this.Text = $"{message.Author.Nickname}: {message.Content}";
    }
}