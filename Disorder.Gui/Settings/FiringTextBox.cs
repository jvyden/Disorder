using Eto.Forms;

namespace Disorder.Gui.Settings;

public class FiringTextBox : TextBox {
    public FiringTextBox(EventHandler<string> eventHandler) {
        this.KeyDown += delegate(object? sender, KeyEventArgs args) {
            if(args.Key == Keys.Enter) eventHandler.Invoke(this, this.Text);
        };

        this.LostFocus += delegate {
            eventHandler.Invoke(this, this.Text);
        };
    }
}