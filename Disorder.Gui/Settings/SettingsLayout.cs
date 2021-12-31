using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Settings; 

public class SettingsLayout : DynamicLayout {
    public string CategoryName { get; set; }
    
    public SettingsLayout(string categoryName) {
        this.CategoryName = categoryName;
        
        this.Padding = new Padding(5);
        this.Spacing = new Size(5, 5);

        Label categoryNameLabel = new() { Text = CategoryName };
        categoryNameLabel.Font = new Font(SystemFont.Bold, 14f);
        
        this.Add(categoryNameLabel);
    }
}