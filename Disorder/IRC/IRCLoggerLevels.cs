using Kettu;

namespace Disorder.IRC; 

public class LoggerLevelIRCInfo : LoggerLevel {
	private LoggerLevelIRCInfo() {}
	
	public override string      Name => "IRCInfo";
	public static   LoggerLevel Instance = new LoggerLevelIRCInfo();
} 

public class LoggerLevelIRCError : LoggerLevel {
	private LoggerLevelIRCError() {}
	
	public override string      Name => "IRCError";
	public static   LoggerLevel Instance = new LoggerLevelIRCError();
} 

public class LoggerLevelIRCWarning : LoggerLevel {
	private LoggerLevelIRCWarning() {}
	
	public override string      Name => "IRCWarning";
	public static   LoggerLevel Instance = new LoggerLevelIRCWarning();
} 
