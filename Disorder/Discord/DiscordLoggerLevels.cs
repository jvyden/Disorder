using Kettu;

namespace Disorder.Discord; 

public class LoggerLevelDiscordInfo : LoggerLevel {
	private LoggerLevelDiscordInfo() {}
	
	public override string      Name => "DiscordInfo";
	public static   LoggerLevel Instance = new LoggerLevelDiscordInfo();
} 

public class LoggerLevelDiscordError : LoggerLevel {
	private LoggerLevelDiscordError() {}
	
	public override string      Name => "DiscordError";
	public static   LoggerLevel Instance = new LoggerLevelDiscordError();
} 

public class LoggerLevelDiscordWarning : LoggerLevel {
	private LoggerLevelDiscordWarning() {}
	
	public override string      Name => "DiscordWarning";
	public static   LoggerLevel Instance = new LoggerLevelDiscordWarning();
} 
