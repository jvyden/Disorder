using Disorder.Dummy;
using Disorder.IRC;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui; 

public class MainForm : Form {
    public ListBox GuildList;
    public ListBox TextList;
    public ListBox UserList;
    public TextBox MessageField;
    
    private readonly List<IChatClient> chatClients = new() {
        new IRCChatClient("localhost"),
        new DummyChatClient(),
    };
    
    public MainForm() {
        this.Title = "Disorder";
        this.ClientSize = new Size(1280, 960);

        DynamicLayout layout = new() {
            Spacing = new Size(5,5),
            Padding = new Padding(10),
        };

        layout.BeginHorizontal();
        layout.Add(this.GuildList = new ListBox() { Size = new Size(200, -1)});
        layout.BeginVertical();
        layout.BeginHorizontal();
        layout.Add(this.TextList = new ListBox(), true, true);
        layout.Add(this.UserList = new ListBox());
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(this.MessageField = new TextBox(), true);
        layout.Add(new Button() {Text = "Send", MinimumSize = new Size(200, -1)});
        layout.EndHorizontal();
        layout.EndVertical();
        layout.EndHorizontal();

        this.Content = layout;

        ChatClientManager.Initialize(chatClients);

        foreach(IGuild guild in chatClients.SelectMany(chatClient => chatClient.Guilds)) {
            this.GuildList.Items.Add(new GuildListItem(guild));

            guild.ChannelAdded += this.channelAddedToGuild;
        }

        this.GuildList.SelectedValueChanged += guildChanged;
    }
    private void guildChanged(object? sender, EventArgs e) {
        GuildListItem? listItem = (GuildListItem?)this.GuildList.SelectedValue;
        if(listItem == null) return;

        Task.Factory.StartNew(async () => {
            await this.RefreshMessages(listItem.Guild.Channels.First());
        });

        Console.WriteLine("guild item changed to " + listItem.Text);
    }

    private void messageSentToCurrentChannel(object? sender, IMessage message) {
        this.TextList.Items.Add(new MessageListItem(message));
    }

    private void channelAddedToGuild(object? sender, IChannel channel) {
        channel.MessageSent += delegate(object? sender, IMessage message) {
            GuildListItem? listItem = (GuildListItem?)this.GuildList.SelectedValue;
            if(listItem == null) return;

            if(channel.Guild == listItem.Guild) messageSentToCurrentChannel(sender, message);
        };
    } 

    public async Task RefreshMessages(IChannel channel) {
        this.TextList.Items.Clear();
        IEnumerable<IMessage> messages = await channel.FetchMessages();

        foreach(IMessage message in messages) this.TextList.Items.Add(new MessageListItem(message));
        
    }

}