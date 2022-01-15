namespace Disorder.Tataku {
    public class TatakuChatClient : IChatClient {
        public TatakuChatClient(string uri) {
            this.guilds = new List<TatakuGuild>(1);
            
            this.guilds.Add(new TatakuGuild(uri, this));
        }

        private List<TatakuGuild> guilds;
        public IEnumerable<IGuild> Guilds => guilds;
        public IUser User { get; set; }
        
        public event EventHandler? GuildsUpdated;
        public event EventHandler? OnLoggedIn;
    }
}