using Disorder.Dummy;
using Disorder.IRC;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui; 

public class MainForm : Form {
    public ListBox GuildList;
    public ListBox TextList;
    
    private readonly List<IChatClient> chatClients = new() {
        new IRCChatClient("localhost"),
        new DummyChatClient(),
    };
    
    public MainForm() {
        this.Title = "Disorder";
        this.ClientSize = new Size(800, 900);

        this.Content = new TableLayout {
            Spacing = new Size(5,5),
            Padding = new Padding(10),
            Rows = {
                new TableRow(
                    new TableCell(this.GuildList = new ListBox())
                ),
                new TableRow(
                    new TableCell(this.TextList = new ListBox())
                ),
            },
        };

        ChatClientManager.Initialize(chatClients);

        foreach(IGuild guild in chatClients.SelectMany(chatClient => chatClient.Guilds)) {
            this.GuildList.Items.Add(new GuildListItem(guild));
        }

        this.GuildList.SelectedValueChanged += guildChanged;
    }
    private void guildChanged(object? sender, EventArgs e) {
        GuildListItem? listItem = (GuildListItem?)this.GuildList.SelectedValue;
        if(listItem == null) return;

        Task.Factory.StartNew(async () => {
            this.TextList.Items.Clear();
            IEnumerable<IMessage> messages = await listItem.Guild.Channels.First().FetchMessages();
            
            foreach(IMessage message in messages) this.TextList.Items.Add(new MessageListItem(message));

            Console.WriteLine("guild item changed to " + listItem.Text);
        });
    }

}