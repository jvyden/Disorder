using Kettu;

namespace Disorder.Dummy;

public class LoggerLevelDummyInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDummyInfo();
    private LoggerLevelDummyInfo() {}

    public override string Name => "DummyInfo";
}

public class LoggerLevelDummyError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDummyError();
    private LoggerLevelDummyError() {}

    public override string Name => "DummyError";
}

public class LoggerLevelDummyWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelDummyWarning();
    private LoggerLevelDummyWarning() {}

    public override string Name => "DummyWarning";
}