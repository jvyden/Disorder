using Disorder.Discord;
using Disorder.Dummy;
using Disorder.Gui.ListItems;
using Disorder.IRC;
using Disorder.Tataku;
using Eto.Drawing;
using Eto.Forms;
using Kettu;

namespace Disorder.Gui.Forms;

using Disorder;

public class MainForm : Form {
    private readonly List<IChatClient> chatClients = new();

    public readonly ListBox GuildList;
    public readonly TextBox MessageField;
    public readonly ListBox MessageList;
    public readonly ListBox UserList;

    private void CreateChatClients() {
//        #region IRC
//        try {
//            IRCChatClient irc = new(Settings.Instance.IrcServerUrl);
//            this.chatClients.Add(irc);
//        }
//        catch (Exception ex){
//            LogFailedChatClient(typeof(IRCChatClient), ex);
//        }
//        #endregion
        
//        #region Discord
//        try {
//            DiscordChatClient discord = new(Settings.Instance.DiscordToken);
//            this.chatClients.Add(discord);
//        }
//        catch (Exception ex){
//            LogFailedChatClient(typeof(DiscordChatClient), ex);
//        }
//        #endregion
        
//        #region Dummy
//        try {
//            DummyChatClient dummy = new();
//            this.chatClients.Add(dummy);
//        }
//        catch (Exception ex){
//            LogFailedChatClient(typeof(DummyChatClient), ex);
//        }
//        #endregion

        #region Taiko.rs

        try {
            TatakuChatClient tataku = new("wss://taikors.ayyeve.xyz");
            this.chatClients.Add(tataku);
        }
        catch(Exception ex) {
            LogFailedChatClient(typeof(TatakuChatClient), ex);
        }

        #endregion
    }

    private static void LogFailedChatClient(Type chatClientType, Exception ex) {
        Logger.Log(
            $"{ex.GetType()} creating {chatClientType.Name}! Details: {ex.Message}", 
            LoggerLevelClientCreationError.Instance
        );
    }
    
    public MainForm() {
        Logger.AddLogger(new ConsoleLogger());
        Logger.StartLogging();

        Logger.Log("Constructing main form", LoggerLevelGUIInfo.Instance);

        this.Title = "Disorder";
        this.ClientSize = new Size(1280, 960);

        DynamicLayout layout = new() {
            Spacing = new Size(5, 5),
            Padding = new Padding(10),
        };

        this.Menu = new MenuBar {
            Items = {
                new ButtonMenuItem {
                    Text = "File", Items = {
                        new Command((_, _) => new SettingsForm().Show()) {
                            MenuText = "Settings",
                            Shortcut = Application.Instance.CommonModifier | Keys.Comma,
                        },
                        new Command((_, _) => Application.Instance.Quit()) {
                            MenuText = "Exit",
                            Shortcut = Application.Instance.CommonModifier | Keys.Q,
                        },
                    },
                },
                new ButtonMenuItem { Text = "Help" },
            },
        };

        layout.BeginHorizontal();
        layout.Add(this.GuildList = new ListBox { Size = new Size(300, -1) });
        layout.BeginVertical();
        layout.BeginHorizontal();
        layout.Add(this.MessageList = new ListBox(), true, true);
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

        this.CreateChatClients();

        ChatClientManager.Initialize(this.chatClients);

        foreach(IChatClient chatClient in this.chatClients) chatClient.GuildsUpdated += this.guildsUpdated;

        this.GuildList.SelectedValueChanged += this.channelChanged;

        this.guildsUpdated(this, null);
    }

    protected override void Dispose(bool disposing) {
        Logger.StopLogging().Wait();

        base.Dispose(disposing);
    }
    
    private void channelChanged(object? sender, EventArgs e) {
        if(this.GuildList.SelectedValue is not ChannelListItem channelItem) {
            this.UserList.Items.Clear();
            this.MessageList.Items.Clear();
            return;
        }

        Task.Factory.StartNew(async () => {
            await this.RefreshMessages(channelItem.Channel);
        });

        Logger.Log("channel item changed to " + channelItem.Channel.Name, LoggerLevelGUIInfo.Instance);
    }

    private void guildsUpdated(object? sender, EventArgs e) {
        this.GuildList.Items.Clear();

        using HttpClient client = new();

        foreach(IGuild guild in this.chatClients.SelectMany(chatClient => chatClient.Guilds)) {
            GuildListItem guildListItem = new(guild);
            
            if(guild is DiscordGuild discordGuild && discordGuild.Guild.Icon != null) {
                Directory.CreateDirectory(Path.Combine(Settings.ConfigPath, "ServerIconCache"));
                
                string avatarUrl = discordGuild.Guild.Icon.Url;
                string avatarPath = Path.Combine(Settings.ConfigPath, "ServerIconCache", avatarUrl.Substring(avatarUrl.LastIndexOf('/') + 1));

                Stream? stream = null;
                
                if(File.Exists(avatarPath)) {
                    stream = File.Open(avatarPath, FileMode.Open);
                }
                else {
                    // TODO: make async
                    Logger.Log($"Fetching image for {discordGuild} from {avatarUrl}", LoggerLevelDiscordInfo.Instance);
                    HttpResponseMessage response = client.GetAsync(avatarUrl).Result;
                    Logger.Log($"Successfully fetched image for {discordGuild}", LoggerLevelDiscordInfo.Instance);

                    if(response.IsSuccessStatusCode) {
                        stream = response.Content.ReadAsStream();

                        using MemoryStream ms = new();
                        stream.CopyTo(ms);
                        
                        File.WriteAllBytes(avatarPath, ms.ToArray());
                    }
                }

                if(stream != null) {
                    try {
                        Bitmap bitmap = new(stream);
                        guildListItem.Image = bitmap.WithSize(32, 32);
                    }
                    catch (Exception ex){
                        Logger.Log($"Failed to get image for {discordGuild}: {ex.Message}", LoggerLevelDiscordError.Instance);
                    }
                }
                else {
                    Logger.Log($"Failed to get image for {discordGuild}", LoggerLevelDiscordError.Instance);
                }
            }
            
            this.GuildList.Items.Add(guildListItem);
            guild.ChannelAdded += this.channelAddedToGuild;
        }
    }

    private void messageSentToCurrentChannel(IMessage message) {
        this.MessageList.Items.Add(new MessageListItem(message));
    }

    private void channelAddedToGuild(object? _, IChannel channel) {
        GuildListItem guildListItem = (GuildListItem)this.GuildList.Items.First(i => {
            if(i is GuildListItem guildListItem) return guildListItem.Guild == channel.Guild;

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

    public async Task RefreshMessages(IChannel channel, int limit = 50) {
        this.MessageList.Items.Clear();
        IEnumerable<IMessage> messages = await channel.FetchMessages(limit);

        foreach(IMessage message in messages) this.MessageList.Items.Add(new MessageListItem(message));

    }
}
