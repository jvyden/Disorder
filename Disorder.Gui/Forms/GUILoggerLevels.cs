using Kettu;

namespace Disorder.Gui.Forms; 

public class LoggerLevelGUIInfo : LoggerLevel {
	private LoggerLevelGUIInfo() {}
	
	public override string      Name => "GUIInfo";
	public static   LoggerLevel Instance = new LoggerLevelGUIInfo();
} 

public class LoggerLevelGUIError : LoggerLevel {
	private LoggerLevelGUIError() {}
	
	public override string      Name => "GUIError";
	public static   LoggerLevel Instance = new LoggerLevelGUIError();
} 

public class LoggerLevelGUIWarning : LoggerLevel {
	private LoggerLevelGUIWarning() {}
	
	public override string      Name => "GUIWarning";
	public static   LoggerLevel Instance = new LoggerLevelGUIWarning();
} 