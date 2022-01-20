using Disorder.Gui.ListItems;
using Disorder.Gui.Settings;
using Disorder.Gui.Settings.Layouts;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Forms;

public class SettingsForm : Form {
    public readonly ListBox CategoryList;
    public DynamicLayout? SettingsLayout;

    public SettingsForm() {
        this.Title = "Disorder - Settings";
        this.ClientSize = new Size(1000, 600);

        this.KeyDown += delegate(object? _, KeyEventArgs args) {
            if(args.Key == Keys.Escape) this.Close();
        };

        List<SettingsLayout> settingsLayouts = new() {
            new GeneralSettingsLayout(),
        };

        this.CategoryList = new ListBox { Size = new Size(200, -1) };
        this.redoLayout();

        foreach(SettingsLayout settingsLayout in settingsLayouts) this.CategoryList.Items.Add(new SettingsLayoutListItem(settingsLayout));

        this.CategoryList.SelectedValueChanged += this.categoryChanged;
    }

    private void categoryChanged(object? sender, EventArgs e) {
        this.SettingsLayout = ((SettingsLayoutListItem)this.CategoryList.SelectedValue).SettingsLayout;
        this.SettingsLayout.Add(null); // Null element makes it so the last element doesn't stretch on the y axis

        this.redoLayout();
    }

    private void redoLayout() {
        DynamicLayout layout = new() {
            Spacing = new Size(5, 5),
            Padding = new Padding(10),
        };

        layout.BeginHorizontal();
        layout.Add(this.CategoryList);
        layout.Add(this.SettingsLayout);
        layout.EndHorizontal();

        this.Content = layout;
    }
}