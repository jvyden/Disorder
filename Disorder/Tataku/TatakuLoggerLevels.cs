using Kettu;

namespace Disorder.Tataku;

public class LoggerLevelTatakuInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTatakuInfo();
    private LoggerLevelTatakuInfo() {}

    public override string Name => "TatakuInfo";
}

public class LoggerLevelTatakuError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTatakuError();
    private LoggerLevelTatakuError() {}

    public override string Name => "TatakuError";
}

public class LoggerLevelTatakuWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelTatakuWarning();
    private LoggerLevelTatakuWarning() {}

    public override string Name => "TatakuWarning";
}