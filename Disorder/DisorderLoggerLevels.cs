using Kettu;

namespace Disorder; 

public class LoggerLevelDisorderInfo : LoggerLevel {
	private LoggerLevelDisorderInfo() {}
	
	public override string      Name => "DisorderInfo";
	public static   LoggerLevel Instance = new LoggerLevelDisorderInfo();
} 

public class LoggerLevelDisorderError : LoggerLevel {
	private LoggerLevelDisorderError() {}
	
	public override string      Name => "DisorderError";
	public static   LoggerLevel Instance = new LoggerLevelDisorderError();
} 

public class LoggerLevelDisorderWarning : LoggerLevel {
	private LoggerLevelDisorderWarning() {}
	
	public override string      Name => "DisorderWarning";
	public static   LoggerLevel Instance = new LoggerLevelDisorderWarning();
} 
