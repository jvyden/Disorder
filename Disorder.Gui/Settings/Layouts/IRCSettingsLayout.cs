namespace Disorder.Gui.Settings.Layouts;
using Disorder;

public class IRCSettingsLayout : SettingsLayout {
    public IRCSettingsLayout() : base("IRC") {
        this.Add(new FiringTextBox(ipChanged) { PlaceholderText = "IP Address", Text = Settings.Instance.IrcServerUrl});
    }

    private void ipChanged(object? _, string text) {
        Console.WriteLine("text changed to " + text);
        Settings.Instance.IrcServerUrl = text;
    }
}