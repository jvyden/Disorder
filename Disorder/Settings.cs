using Disorder.Helpers.Serialization;
using Kettu;
using Newtonsoft.Json;

namespace Disorder;

public class Settings {
    private const string configFileName = "disorder.config.json";

    public const int CurrentConfigVersion = 5; // MUST BE INCREMENTED FOR EVERY CONFIG CHANGE!

    public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Disorder");

    public static readonly string ConfigFile = Path.Combine(ConfigPath, configFileName);

    private static readonly List<Type> chatClientTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(p => typeof(IChatClient).IsAssignableFrom(p))
        .Where(p => p != typeof(IChatClient))
        .ToList();

    private static readonly KnownTypesBinder knownTypesBinder = new() {
        KnownTypes = chatClientTypes,
    };
    
    private static JsonSerializerSettings serializerSettings => new() {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        SerializationBinder = knownTypesBinder,
    };

    static Settings() {
        Directory.CreateDirectory(ConfigPath);

        Logger.Log($"Found chat client types: {string.Join(", ", chatClientTypes.Select(c => c.Name))}", LoggerLevelDisorderInfo.Instance);

        if(File.Exists(ConfigFile)) {
            string configFile = File.ReadAllText(ConfigFile);

            Instance = JsonConvert.DeserializeObject<Settings>(configFile, serializerSettings) ?? throw new ArgumentNullException(nameof(ConfigFile));

            if(Instance.ConfigVersion >= CurrentConfigVersion) return;

            Logger.Log($"Upgrading config file from version {Instance.ConfigVersion} to version {CurrentConfigVersion}", LoggerLevelDisorderInfo.Instance);
            Instance.ConfigVersion = CurrentConfigVersion;
            
            configFile = JsonConvert.SerializeObject(Instance, typeof(Settings), serializerSettings);
            File.WriteAllText(ConfigFile, configFile);
        }
        else {
            Instance = new Settings();
            Instance.Save();

            Logger.Log
            (
                "The configuration file was not found. " +
                "A blank configuration file has been created at " +
                ConfigFile + ".",
                LoggerLevelDisorderInfo.Instance
            );
        }
    }
    public static Settings Instance { get; }

    [JsonProperty("ConfigVersionDoNotModifyOrYouWillBeSlapped")]
    public int ConfigVersion { get; set; } = CurrentConfigVersion;

    public string IrcServerUrl { get; set; } = "localhost";
    public string IrcUsername { get; set; } = Environment.UserName;
    public string IrcAutoJoinList { get; set; } = "#general";

    public string? DiscordToken { get; set; }

    public List<IChatClient> ChatClients { get; set; } = new();

    public void Save() {
        string configFile = JsonConvert.SerializeObject(this, typeof(Settings), serializerSettings);
        File.WriteAllText(ConfigFile, configFile);
    }
}