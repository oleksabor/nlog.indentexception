using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourNamespace.NLog.Extention
{
	/// <summary>
	/// renders exception starting from new line
	/// with short type exception name followed by message
	/// and stacktrace (optionally)
	/// if exception is logged more than once (catched, logged and re-thrown as inner), stack trace is not written
	/// </summary>
	[LayoutRenderer("IndentException")]
	public class IndentExceptionLayoutRenderer : LayoutRenderer
	{
		/// <summary>
		/// indent before exception type (default is tab)
		/// </summary>
		public string Indent { get; set; }
		/// <summary>
		/// indent between each stack trace line (default is two tab characters)
		/// </summary>
		public string StackTraceIndent { get; set; }
		/// <summary>
		/// is written before exception type name (default [)
		/// </summary>
		public string BeforeType { get; set; }
		/// <summary>
		/// is written after exception type name (default ])
		/// </summary>
		public string AfterType { get; set; }
		/// <summary>
		/// separator between exception type and message
		/// </summary>
		public string Separator { get; set; }
		/// <summary>
		/// log stack trace or not (for console logger e.g.)
		/// </summary>
		public bool LogStack { get; set; }

		/// <summary>
		/// holds logged already exceptions just to skip stack logging
		/// </summary>
		static ConcurrentQueue<Exception> _loggedErrors = new ConcurrentQueue<Exception>();

		public IndentExceptionLayoutRenderer()
		{
			Indent = "\t";
			StackTraceIndent = "\t\t";
			BeforeType = "[";
			AfterType = "]";
			LogStack = true;
			Separator = " ";
		}

		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			var e = logEvent.Exception;
			while (e != null)
			{
				builder.AppendFormat("{1}{2}{0}{3}{4}", e.GetType().Name, Indent, BeforeType, AfterType, Separator);
				builder.Append(e.Message);

				if (LogStack)
				{
					var stackTraceWasLogged = _loggedErrors.Contains(e);
					if (!stackTraceWasLogged)
					{
						builder.AppendLine();
						_loggedErrors.Enqueue(e);
						builder.AppendFormat("{0}", e.StackTrace.Replace("   ", StackTraceIndent));
					}

					if (_loggedErrors.Count > 33)
					{
						_loggedErrors.TryDequeue(out Exception ex1);
						_loggedErrors.TryDequeue(out Exception ex2);
					}
				}

				e = e.InnerException;
				if (e != null)
					builder.AppendLine();
			}
		}
	}

}
