using Eto.Forms;

namespace Disorder.Gui.ListItems; 

public class TypeListItem : ListItem {
    public Type Type;

    public TypeListItem(Type type) {
        this.Type = type;

        this.Text = type.Name;
    }
}