using Kettu;

namespace Disorder.Dummy; 

public class LoggerLevelDummyInfo : LoggerLevel {
	private LoggerLevelDummyInfo() {}
	
	public override string      Name => "DummyInfo";
	public static   LoggerLevel Instance = new LoggerLevelDummyInfo();
} 

public class LoggerLevelDummyError : LoggerLevel {
	private LoggerLevelDummyError() {}
	
	public override string      Name => "DummyError";
	public static   LoggerLevel Instance = new LoggerLevelDummyError();
} 

public class LoggerLevelDummyWarning : LoggerLevel {
	private LoggerLevelDummyWarning() {}
	
	public override string      Name => "DummyWarning";
	public static   LoggerLevel Instance = new LoggerLevelDummyWarning();
} 
