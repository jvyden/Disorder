using Disorder.Discord;
using Disorder.Dummy;
using Disorder.Gui.ListItems;
using Disorder.IRC;
using Eto.Drawing;
using Eto.Forms;

namespace Disorder.Gui.Forms;

using Disorder;

public class MainForm : Form {

    private readonly List<IChatClient> chatClients = new() {
        new IRCChatClient(Settings.Instance.IrcServerUrl),
        new DiscordChatClient(Settings.Instance.DiscordToken),
        new DummyChatClient(),
    };

    public readonly ListBox GuildList;
    public readonly TextBox MessageField;
    public readonly ListBox TextList;
    public readonly ListBox UserList;

    public MainForm() {
        Console.WriteLine("Constructing main form");
        
        this.Title = "Disorder";
        this.ClientSize = new Size(1280, 960);

        DynamicLayout layout = new() {
            Spacing = new Size(5, 5),
            Padding = new Padding(10),
        };

        this.Menu = new MenuBar {
            Items = {
                new ButtonMenuItem { Text = "File", Items = {
                    new Command((_, _) => new SettingsForm().Show()) {
                        MenuText = "Settings",
                        Shortcut = Application.Instance.CommonModifier | Keys.Comma,
                    },
                    new Command((_,_) => Application.Instance.Quit()) {
                        MenuText = "Exit",
                        Shortcut = Application.Instance.CommonModifier | Keys.Q,
                    },
                }},
                new ButtonMenuItem { Text = "Help" },
            },
        };

        layout.BeginHorizontal();
        layout.Add(this.GuildList = new ListBox { Size = new Size(250, -1) });
        layout.BeginVertical();
        layout.BeginHorizontal();
        layout.Add(this.TextList = new ListBox(), true, true);
        layout.Add(this.UserList = new ListBox());
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(this.MessageField = new TextBox(), true);
        layout.Add(new Button(this.sendButtonClicked) { Text = "Send", MinimumSize = new Size(200, -1) });
        layout.EndHorizontal();
        layout.EndVertical();
        layout.EndHorizontal();

        this.MessageField.KeyDown += delegate(object? _, KeyEventArgs args) {
            if(args.Key == Keys.Enter) this.sendButtonClicked(this, args);
        };

        this.Content = layout;

        ChatClientManager.Initialize(this.chatClients);
        
        foreach(IChatClient chatClient in this.chatClients) {
            chatClient.GuildsUpdated += this.guildsUpdated;
        }

        this.GuildList.SelectedValueChanged += this.channelChanged;
        
        this.guildsUpdated(this, null);
    }
    private void channelChanged(object? sender, EventArgs e) {
        if(this.GuildList.SelectedValue is not ChannelListItem channelItem) {
            this.UserList.Items.Clear();
            this.TextList.Items.Clear();
            return;
        }

        Task.Factory.StartNew(async () => {
            await this.RefreshMessages(channelItem.Channel);
        });

        Console.WriteLine("channel item changed to " + channelItem.Channel.Name);
    }

    private void guildsUpdated(object? sender, EventArgs e) {
        this.GuildList.Items.Clear();
        
        foreach(IGuild guild in this.chatClients.SelectMany(chatClient => chatClient.Guilds)) {
            this.GuildList.Items.Add(new GuildListItem(guild));
            guild.ChannelAdded += this.channelAddedToGuild;
        }
    }

    private void messageSentToCurrentChannel(IMessage message) {
        this.TextList.Items.Add(new MessageListItem(message));
    }

    private void channelAddedToGuild(object? _, IChannel channel) {
        GuildListItem guildListItem = (GuildListItem)this.GuildList.Items.First(i => {
            if(i is GuildListItem guildListItem) {
                return guildListItem.Guild == channel.Guild;
            }
            return false;
        });
        
        this.GuildList.Items.Insert(this.GuildList.Items.IndexOf(guildListItem) + 1, new ChannelListItem(channel));
        
        channel.MessageSent += delegate(object? _, IMessage message) {
            if(this.GuildList.SelectedValue is not ChannelListItem channelItem) return;

            if(channelItem.Channel == channel) this.messageSentToCurrentChannel(message);
        };
    }

    private void sendButtonClicked(object? sender, EventArgs e) {
        if(this.GuildList.SelectedValue is not ChannelListItem channelItem) return;

        channelItem.Channel.SendMessage(this.MessageField.Text);
        this.MessageField.Text = string.Empty;
    }

    public async Task RefreshMessages(IChannel channel) {
        this.TextList.Items.Clear();
        IEnumerable<IMessage> messages = await channel.FetchMessages();

        foreach(IMessage message in messages) this.TextList.Items.Add(new MessageListItem(message));

    }
}