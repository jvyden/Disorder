using Disorder.Gui.Forms;
using Kettu;

namespace Disorder.Gui.Settings.Layouts;

public class IRCSettingsLayout : SettingsLayout {
    public IRCSettingsLayout() : base("IRC") {
        this.Add(new FiringTextBox(this.ipChanged) { PlaceholderText = "IP Address", Text = Disorder.Settings.Instance.IrcServerUrl });
    }

    private void ipChanged(object? _, string text) {
        Logger.Log("text changed to " + text, LoggerLevelGUIInfo.Instance);
        Disorder.Settings.Instance.IrcServerUrl = text;
    }
}