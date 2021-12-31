using System.Text.Json;
using System.Text.Json.Serialization;

namespace Disorder; 

public class Settings {
    public static Settings Instance { get; private set; }
    private const string configFileName = "disorder.config.json";

    public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Disorder");

    public static readonly string ConfigFile = Path.Combine(ConfigPath, configFileName);

    public const int CurrentConfigVersion = 2; // MUST BE INCREMENTED FOR EVERY CONFIG CHANGE!

    [JsonPropertyName("ConfigVersionDoNotModifyOrYouWillBeSlapped")]
    public int ConfigVersion { get; set; } = CurrentConfigVersion;

    static Settings() {
        Directory.CreateDirectory(ConfigPath);
        
        if(File.Exists(ConfigFile)) {
            string configFile = File.ReadAllText(ConfigFile);

            Instance = JsonSerializer.Deserialize<Settings>(configFile) ?? throw new ArgumentNullException(nameof(ConfigFile));

            if(Instance.ConfigVersion >= CurrentConfigVersion) return;

            Console.WriteLine($"Upgrading config file from version {Instance.ConfigVersion} to version {CurrentConfigVersion}");
            Instance.ConfigVersion = CurrentConfigVersion;
            configFile = JsonSerializer.Serialize
            (
                Instance,
                typeof(Settings),
                new JsonSerializerOptions {
                    WriteIndented = true,
                }
            );

            File.WriteAllText(ConfigFile, configFile);
        }
        else {
            Instance = new Settings();
            Instance.Save();
            
            Console.WriteLine
            (
                "The configuration file was not found. " +
                "A blank configuration file has been created at " +
                ConfigFile + "."
            );
        }
    }

    public void Save() {
        string configFile = JsonSerializer.Serialize
        (
            this,
            typeof(Settings),
            new JsonSerializerOptions {
                WriteIndented = true,
            }
        );

        File.WriteAllText(ConfigFile, configFile);
    }

    public string IrcServerUrl { get; set; } = "localhost";
    public string IrcUsername { get; set; } = Environment.UserName;
    public string IrcAutoJoinList { get; set; } = "#general";
    
    public string DiscordToken { get; set; }
}