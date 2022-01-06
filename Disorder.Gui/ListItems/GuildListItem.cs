using Eto.Forms;

namespace Disorder.Gui.ListItems;

public class GuildListItem : ImageListItem {
    public IGuild Guild;

    public GuildListItem(IGuild guild) {
        this.Guild = guild;

        this.Text = $"{guild.GetType().Name.Replace("Guild", "")} - {guild.Name}";
    }
}