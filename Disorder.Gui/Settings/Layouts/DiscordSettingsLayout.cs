namespace Disorder.Gui.Settings.Layouts; 

public class DiscordSettingsLayout : SettingsLayout {
    public DiscordSettingsLayout() : base("Discord") {
        this.Add(new FiringCheckBox(this.constructChannels) { Text = "Construct all channels in all guilds" });
    }
    
    private void constructChannels(object? sender, bool isChecked) {
        Disorder.Settings.Instance.ConstructDiscordChannels = isChecked;
    }
}