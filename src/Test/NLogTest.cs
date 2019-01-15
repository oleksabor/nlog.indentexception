using NLog;
using System;
using System.Collections.Generic;

namespace YourNamespace.NLog.Extention.Test
{
	using YourNamespace.NLog.Extention.Test.Classes;

	class Program
	{
		static ILogger Log = LogManager.GetCurrentClassLogger();

		static void TestLogIndent(string[] args)
		{
			Log.Debug("starting");
			try
			{
				new UnitOfWork().tryException();
			}
			catch (Exception e)
			{
				Log.Error(e, "failed to start NLogTest");
				Console.ReadKey();
			}
			Log.Debug("the end");
		}
	}
}

namespace YourNamespace.NLog.Extention.Test.Classes
{
	class UnitOfWork
	{
		static ILogger Log = LogManager.GetCurrentClassLogger();

		public void innerException()
		{
			throw new KeyNotFoundException("innerException");
		}

		public void outerException()
		{
			try
			{
				innerException();
			}
			catch (Exception e)
			{
				throw new ArgumentException("outer exception", e);
			}
		}

		public void tryException()
		{
			try
			{
				outerException();
			}
			catch (Exception e)
			{
				Log.Error(e, "tryException failure");
				throw new ArgumentException("bad try", e);
			}
		}

	}
}