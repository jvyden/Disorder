using Disorder.Gui.Forms;
using Eto.Forms;

namespace Disorder.Gui;

public static class Program {
    public static void Main(string[] args) {
        new Application {
            UIThreadCheckMode = UIThreadCheckMode.Warning,
        }.Run(new MainForm());
    }
}
