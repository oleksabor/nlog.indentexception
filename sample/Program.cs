using NLog;

namespace nlog.identtest;
class Program
{
		static ILogger Log = LogManager.GetCurrentClassLogger();
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
			Log.Debug("starting");
			try
			{
				throw new ApplicationException("test");
			}
			catch (Exception e)
			{
				Log.Error(e, "failed to start NLogTest");
				Console.ReadKey();
			}
			Log.Debug("the end");
    }
}
