using System.Collections.ObjectModel;
using Disorder.Gui.Settings;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Forms; 

public class SettingsForm : Form {
    public readonly ListBox CategoryList;
    public readonly GridView Settings;
    
    public SettingsForm() {
        this.Title = "Disorder - Settings";
        this.ClientSize = new Size(800, 600);
        
        this.KeyDown += delegate(object? sender, KeyEventArgs args) {
            if(args.Key == Keys.Escape) this.Close();
        };

        DynamicLayout layout = new() {
            Spacing = new Size(5, 5),
            Padding = new Padding(10),
        };

        layout.BeginHorizontal();
        layout.Add(this.CategoryList = new ListBox { Size = new Size(200, -1) });
        layout.Add(this.Settings = new GridView {
            Columns = {
                new GridColumn {
                    DataCell = new TextBoxCell { Binding = Binding.Property<StringSetting, string>(s => s.Setting) },
                    HeaderText = "Setting",
                    Editable = false,
                },
                new GridColumn {
                    DataCell = new TextBoxCell { Binding = Binding.Property<StringSetting, string>(s => s.Value) },
                    HeaderText = "Value",
                    Editable = true,
                    Expand = true,
                },
            },
        });
        layout.EndHorizontal();
        
        List<SettingsCategory> categories = new() {
            new SettingsCategory {
                Settings = new ObservableCollection<ISetting> {
                    new StringSetting {
                        Setting = "test setting",
                        Value = "test value",
                    },
                },
                Text = "Test Category",
            },
        };
        
        foreach(SettingsCategory category in categories) {
            this.CategoryList.Items.Add(category);
        }
        
        this.CategoryList.SelectedValueChanged += categoryChanged;

        this.Content = layout;
    }
    
    private void categoryChanged(object sender, EventArgs e) {
        this.Settings.DataStore = ((SettingsCategory)this.CategoryList.SelectedValue).Settings;
    }
}