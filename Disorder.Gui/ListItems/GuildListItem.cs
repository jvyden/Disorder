using Eto.Forms;

namespace Disorder.Gui.ListItems;

public class GuildListItem : ListItem {
    public IGuild Guild;

    public GuildListItem(IGuild guild) {
        this.Guild = guild;

        this.Text = $"{guild.GetType().Name} - {guild.Name}";
    }
}