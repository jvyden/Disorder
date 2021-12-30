using System.Collections.ObjectModel;
using Eto.Forms;

namespace Disorder.Gui.Settings; 

public class SettingsCategory : ListItem {
    public ObservableCollection<ISetting> Settings { get; set; } = new();
}