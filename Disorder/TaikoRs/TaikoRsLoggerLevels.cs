using Kettu;

namespace Disorder.TaikoRs;

public class LoggerLevelTaikoRsInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTaikoRsInfo();
    private LoggerLevelTaikoRsInfo() {}

    public override string Name => "TaikoRsInfo";
}

public class LoggerLevelTaikoRsError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTaikoRsError();
    private LoggerLevelTaikoRsError() {}

    public override string Name => "TaikoRsError";
}

public class LoggerLevelTaikoRsWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTaikoRsWarning();
    private LoggerLevelTaikoRsWarning() {}

    public override string Name => "TaikoRsWarning";
}