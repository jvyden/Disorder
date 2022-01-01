using Eto.Forms;

namespace Disorder.Gui.ListItems;

public class ChannelListItem : ListItem {
    public IChannel Channel;

    public ChannelListItem(IChannel channel) {
        this.Channel = channel;
        this.Text = " -> " + channel.Name;
    }
}