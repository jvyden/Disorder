using Kettu;

namespace Disorder.Discord;

public class LoggerLevelDiscordInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDiscordInfo();
    private LoggerLevelDiscordInfo() {}

    public override string Name => "DiscordInfo";
}

public class LoggerLevelDiscordError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDiscordError();
    private LoggerLevelDiscordError() {}

    public override string Name => "DiscordError";
}

public class LoggerLevelDiscordWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDiscordWarning();
    private LoggerLevelDiscordWarning() {}

    public override string Name => "DiscordWarning";
}