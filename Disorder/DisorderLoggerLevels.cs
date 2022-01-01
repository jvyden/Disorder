using Kettu;

namespace Disorder;

public class LoggerLevelDisorderInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDisorderInfo();
    private LoggerLevelDisorderInfo() {}

    public override string Name => "DisorderInfo";
}

public class LoggerLevelDisorderError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDisorderError();
    private LoggerLevelDisorderError() {}

    public override string Name => "DisorderError";
}

public class LoggerLevelDisorderWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDisorderWarning();
    private LoggerLevelDisorderWarning() {}

    public override string Name => "DisorderWarning";
}