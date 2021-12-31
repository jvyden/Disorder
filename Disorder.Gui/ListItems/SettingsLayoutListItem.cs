using Disorder.Gui.Settings;
using Eto.Forms;

namespace Disorder.Gui.ListItems; 

public class SettingsLayoutListItem : ListItem {
    public SettingsLayout SettingsLayout;
    
    public SettingsLayoutListItem(SettingsLayout settingsLayout) {
        this.SettingsLayout = settingsLayout;
        this.Text = settingsLayout.CategoryName;
    }
}