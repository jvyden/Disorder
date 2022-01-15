namespace Disorder.TaikoRs {
    public class TaikoRsChatClient : IChatClient {
        public TaikoRsChatClient(string uri) {
            this.guilds = new List<TaikoRsGuild>(1);
            
            this.guilds.Add(new TaikoRsGuild(uri, this));
        }

        private List<TaikoRsGuild> guilds;
        public IEnumerable<IGuild> Guilds => guilds;
        public IUser User { get; }
        
        public event EventHandler? GuildsUpdated;
        public event EventHandler? OnLoggedIn;
    }
}