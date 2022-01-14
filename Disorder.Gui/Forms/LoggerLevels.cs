using Kettu;

namespace Disorder.Gui.Forms;

public class LoggerLevelGUIInfo : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelGUIInfo();
    private LoggerLevelGUIInfo() {}

    public override string Name => "GUIInfo";
}

public class LoggerLevelGUIError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelGUIError();
    private LoggerLevelGUIError() {}

    public override string Name => "GUIError";
}

public class LoggerLevelGUIWarning : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelGUIWarning();
    private LoggerLevelGUIWarning() {}

    public override string Name => "GUIWarning";
}

public class LoggerLevelClientCreationError : LoggerLevel {
    public static LoggerLevel Instance = new LoggerLevelClientCreationError();
    private LoggerLevelClientCreationError() {}

    public override string Name => "ClientCreationError";
}