using Disorder.Gui.Forms;
using Eto.Forms;
using Kettu;

namespace Disorder.Gui.Settings.Layouts;

public class GeneralSettingsLayout : SettingsLayout {
    public ListBox ChatClientListBox;
    
    public GeneralSettingsLayout() : base("General") {
        this.Add(new FiringCheckBox(this.doThingsChecked) { Text = "Do things" });
        this.Add(ChatClientListBox = new ListBox());
    }

    private void doThingsChecked(object? sender, bool isChecked) {
        Logger.Log("Do things: " + isChecked, LoggerLevelGUIInfo.Instance);
    }
}