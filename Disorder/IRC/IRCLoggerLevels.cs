using Kettu;

namespace Disorder.IRC;

public class LoggerLevelIRCInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelIRCInfo();
    private LoggerLevelIRCInfo() {}

    public override string Name => "IRCInfo";
}

public class LoggerLevelIRCError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelIRCError();
    private LoggerLevelIRCError() {}

    public override string Name => "IRCError";
}

public class LoggerLevelIRCWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelIRCWarning();
    private LoggerLevelIRCWarning() {}

    public override string Name => "IRCWarning";
}