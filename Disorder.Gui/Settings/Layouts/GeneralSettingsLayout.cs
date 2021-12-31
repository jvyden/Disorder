using Eto.Forms;

namespace Disorder.Gui.Settings.Layouts; 

public class GeneralSettingsLayout : SettingsLayout {
    public GeneralSettingsLayout() : base("General") {
        this.Add(new FiringCheckBox(this.doThingsChecked) { Text = "Do things" });
    }

    private void doThingsChecked(object? sender, bool isChecked) {
        Console.WriteLine("Do things: " + isChecked);
    }
}