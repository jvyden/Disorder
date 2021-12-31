using Disorder.Gui.Forms;
using Kettu;

namespace Disorder.Gui.Settings.Layouts;
using Disorder;

public class IRCSettingsLayout : SettingsLayout {
    public IRCSettingsLayout() : base("IRC") {
        this.Add(new FiringTextBox(ipChanged) { PlaceholderText = "IP Address", Text = Settings.Instance.IrcServerUrl});
    }

    private void ipChanged(object? _, string text) {
        Logger.Log("text changed to " + text, LoggerLevelGUIInfo.Instance);
        Settings.Instance.IrcServerUrl = text;
    }
}