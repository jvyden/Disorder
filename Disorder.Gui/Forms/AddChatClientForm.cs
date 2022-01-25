using Disorder.Gui.ListItems;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Forms; 
using Disorder;

public class AddChatClientForm : Form {
    private ListBox chatClientTypes;
    
    public AddChatClientForm() {
        DynamicLayout layout = new();
        this.Title = "Pick a client type";
        this.Size = new Size(300, 200);

        layout.Add(this.chatClientTypes = new ListBox());

        foreach(Type chatClientType in Disorder.Settings.ChatClientTypes) {
            this.chatClientTypes.Items.Add(new TypeListItem(chatClientType));
        }

        this.Content = layout;

        this.MouseDoubleClick += delegate {
            TypeListItem listItem = (TypeListItem)this.chatClientTypes.SelectedValue;
            IChatClient chatClient = (IChatClient)Activator.CreateInstance(listItem.Type)!;
            
            EditChatClientForm form = new (chatClient);
            
            form.Closed += delegate {
                Settings.Instance.ChatClients.Add(chatClient);
                this.Close();
            };
            
            form.Show();
        };
    }
}