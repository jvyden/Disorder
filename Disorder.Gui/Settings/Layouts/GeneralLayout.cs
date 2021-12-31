using Eto.Forms;

namespace Disorder.Gui.Settings.Layouts; 

public class GeneralLayout : SettingsLayout {
    public GeneralLayout() : base("General") {
        this.Add(new CheckBox { Text = "Do things" });

        this.Add(null);
    }
}