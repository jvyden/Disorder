using System.Collections.ObjectModel;
using Disorder.Gui.ListItems;
using Disorder.Gui.Settings;
using Disorder.Gui.Settings.Layouts;
using Eto.Drawing;
using Eto.Forms;
using GLib;

namespace Disorder.Gui.Forms; 

public class SettingsForm : Form {
    public readonly ListBox CategoryList;
    public DynamicLayout? SettingsLayout;
    
    public SettingsForm() {
        this.Title = "Disorder - Settings";
        this.ClientSize = new Size(1000, 600);
        
        this.KeyDown += delegate(object? sender, KeyEventArgs args) {
            if(args.Key == Keys.Escape) this.Close();
        };

        List<SettingsLayout> settingsLayouts = new() {
            new GeneralLayout(),
            new SettingsLayout("IRC"),
        };

        this.CategoryList = new ListBox { Size = new Size(200, -1) };
        this.redoLayout();

        foreach(SettingsLayout settingsLayout in settingsLayouts) {
            this.CategoryList.Items.Add(new SettingsLayoutListItem(settingsLayout));
        }
        
        this.CategoryList.SelectedValueChanged += categoryChanged;
    }
    
    private void categoryChanged(object sender, EventArgs e) {
        this.SettingsLayout = ((SettingsLayoutListItem)this.CategoryList.SelectedValue).SettingsLayout;
        
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