using Eto.Forms;

namespace Disorder.Gui.Settings;

public class FiringCheckBox : CheckBox {
    public FiringCheckBox(EventHandler<bool> eventHandler) {
        this.CheckedChanged += delegate {
            eventHandler.Invoke(this, this.Checked ?? false);
        };
    }
}