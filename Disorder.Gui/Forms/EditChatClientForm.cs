using System.Reflection;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Forms; 

public class EditChatClientForm : Form {
    private PropertyInfo[] configurableAttributes;
    private IChatClient chatClient;

    private Dictionary<PropertyInfo, TextControl?> textBoxFromProperty = new();

    public EditChatClientForm(IChatClient chatClient) {
        this.chatClient = chatClient;
        
        this.Title = $"Editing {chatClient}";
        this.Padding = new Padding(10);

        DynamicLayout layout = new();
        layout.Spacing = new Size(5, 5);

        layout.Add(new Label {
            Text = this.Title,
            Font = new Font(SystemFont.Bold, 14f),
        });

        configurableAttributes = chatClient
            .GetType()
            .GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(ConfigurablePropertyAttribute)))
            .ToArray();
        
        foreach(PropertyInfo propertyInfo in configurableAttributes) {
            ConfigurablePropertyAttribute attribute = propertyInfo.GetCustomAttributes<ConfigurablePropertyAttribute>().First();

            object value = propertyInfo.GetValue(chatClient) ?? "";

            layout.Add(new Label { Text = attribute.Name });

            TextControl? control;

            if(attribute.IsPassword) control = new PasswordBox { Text = value.ToString() };
            else control = new TextBox { Text = value.ToString() };
            
            layout.Add(control);
            this.textBoxFromProperty.Add(propertyInfo, control);
        }

        layout.Add(new Button(this.saveButtonClicked) {
            Text = "Save",
        });

        layout.Add(new Button(this.cancelButtonClicked) {
            Text = "Cancel",
        });

        this.Content = layout;
    }

    private void saveButtonClicked(object? sender, EventArgs args) {
        foreach(PropertyInfo propertyInfo in this.configurableAttributes) {
            if(this.textBoxFromProperty.TryGetValue(propertyInfo, out TextControl? control)) {
                propertyInfo.SetValue(this.chatClient, control!.Text);
            }
        }

        this.Close();
    }

    private void cancelButtonClicked(object? sender, EventArgs args) {
        this.Close();
    }
}