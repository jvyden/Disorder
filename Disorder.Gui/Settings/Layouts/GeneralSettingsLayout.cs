using Disorder.Gui.Forms;
using Disorder.Gui.ListItems;
using Eto.Drawing;
using Eto.Forms;
using Kettu;

namespace Disorder.Gui.Settings.Layouts;
using Disorder;

public class GeneralSettingsLayout : SettingsLayout {
    public ListBox ChatClientListBox;
    
    public IChatClient SelectedChatClient => ((ChatClientListItem)this.ChatClientListBox.SelectedValue).ChatClient;
    
    public GeneralSettingsLayout() : base("General") {
        this.Add(new FiringCheckBox(this.doThingsChecked) { Text = "Do things" });
        this.Add(new Label { Text = "Clients" });
        this.Add(ChatClientListBox = new ListBox());
        
        this.refresh();

        DynamicLayout buttonLayout = new() {
            Padding = new Padding(5),
            Spacing = new Size(5, 0),
        };
        
        buttonLayout.BeginHorizontal();
        buttonLayout.Add(new Button(this.addButtonClicked) { Text = "Add" });
        buttonLayout.Add(new Button(this.editButtonClicked) { Text = "Edit" });
        buttonLayout.Add(new Button(this.deleteButtonClicked) { Text = "Remove" });
        buttonLayout.Add(null);
        buttonLayout.EndHorizontal();

        this.Add(buttonLayout);
    }

    private void refresh() {
        this.ChatClientListBox.Items.Clear();

        foreach(IChatClient chatClient in Settings.Instance.ChatClients) {
            this.ChatClientListBox.Items.Add(new ChatClientListItem(chatClient));
        }
    }

    private void addButtonClicked(object? sender, EventArgs e) {
        Form form = new AddChatClientForm();
        form.Closed += delegate {
            this.refresh();
        };
        form.Show();
    }
    
    private void editButtonClicked(object? sender, EventArgs e) {
        Form form = new EditChatClientForm(this.SelectedChatClient);
        form.Closed += delegate {
            this.refresh();
        };
        form.Show();
    }

    private void deleteButtonClicked(object? sender, EventArgs e) {
        Settings.Instance.ChatClients.Remove(this.SelectedChatClient);
        
        this.refresh();
    }

    private void doThingsChecked(object? sender, bool isChecked) {
        Logger.Log("Do things: " + isChecked, LoggerLevelGUIInfo.Instance);
    }
}