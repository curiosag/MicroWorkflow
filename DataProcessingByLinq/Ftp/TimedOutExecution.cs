using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq.Ftp
{
	public class TimedOutExecution<T>
	{
		private readonly Stopwatch mStopwatch = new Stopwatch ();

		[Mhs ("PS", DC.N, T = 55823)]
   
		private Stopwatch StopWatch {
			get {
				return mStopwatch;
			}
		}

		[Mhs ("PS", DC.N, T = 55823)]
   
		public T Execute (TimeSpan timeout, Func<Tuple<bool, T>> operation, string timeoutErrorMsg)
		{      
			ArgumentCheck.Assigned (operation, "operation");
			ArgumentCheck.Assigned (timeoutErrorMsg, "timeoutErrorMsg");
			ArgumentCheck.Assigned (timeout, "secsTimeout");
			Check.GreaterOrEqual (timeout.Milliseconds, 0, "timeOut < 0");

			StopWatch.Restart ();

			bool success = false;
      
			Tuple<bool, T> result = null;

			while (!success) {

				if (StopWatch.ElapsedMilliseconds () > timeout.TotalMilliseconds) {
					throw new FtpException (timeoutErrorMsg);
				}

				result = operation.Invoke ();
				Check.Assigned (result);
				Check.Assigned (result.Item1);
        
				success = result.Item1;
				if (!success) {
					Thread.Sleep (timeout.Milliseconds);
				}        
			}
			return result.Item2;
		}
   
	}
}
