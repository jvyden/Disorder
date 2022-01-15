using System.Text.Json;
using System.Text.Json.Serialization;
using Kettu;

namespace Disorder;

public class Settings {
    private const string configFileName = "disorder.config.json";

    public const int CurrentConfigVersion = 3; // MUST BE INCREMENTED FOR EVERY CONFIG CHANGE!

    public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Disorder");

    public static readonly string ConfigFile = Path.Combine(ConfigPath, configFileName);

    static Settings() {
        Directory.CreateDirectory(ConfigPath);

        if(File.Exists(ConfigFile)) {
            string configFile = File.ReadAllText(ConfigFile);

            Instance = JsonSerializer.Deserialize<Settings>(configFile) ?? throw new ArgumentNullException(nameof(ConfigFile));

            if(Instance.ConfigVersion >= CurrentConfigVersion) return;

            Logger.Log($"Upgrading config file from version {Instance.ConfigVersion} to version {CurrentConfigVersion}", LoggerLevelDisorderInfo.Instance);
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

            Logger.Log
            (
                "The configuration file was not found. " +
                "A blank configuration file has been created at " +
                ConfigFile + ".",
                LoggerLevelDisorderInfo.Instance
            );
        }
    }
    public static Settings Instance { get; private set; }

    [JsonPropertyName("ConfigVersionDoNotModifyOrYouWillBeSlapped")]
    public int ConfigVersion { get; set; } = CurrentConfigVersion;

    public string IrcServerUrl { get; set; } = "localhost";
    public string IrcUsername { get; set; } = Environment.UserName;
    public string IrcAutoJoinList { get; set; } = "#general";

    public string TatakuUsername { get; set; } = Environment.UserName;

    public string TatakuPassword { get; set; } = "";

    public string DiscordToken { get; set; }

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
}