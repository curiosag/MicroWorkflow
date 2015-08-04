using System;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq
{
	public class UserFeedback
	{

		public static void Echo(){
			Console.WriteLine ();
		}

		public static void Echo(string msg){
			Console.WriteLine (msg);
		}

		public static void Echo(string msg, object arg){
			Console.WriteLine (msg, arg);
		}

		public static void EchoState<T> (MW<T> wf)   where T: IIndicatesOperation
		{
			ArgumentCheck.Assigned (wf, "env");

/*			var e = wf.Exception;

			if (e != null) {
				Echo ();
				Echo ("--- exception type {0} encountered ---", e.GetType ().ToString ());
				Echo (e.Message);
				Echo ();
			}
*/
			Echo ("--- executionTrace ---");

			Echo (wf.Trace.Trace);
		}

		public static void EchoHeader(string msg){
			Echo ("-----------------------------------------------------------------------------------------");
			Echo (msg);
			Echo ();
		}

		public static MW<T> Verbose<T>(string msg, Func<MW<T>> f)   where T: IIndicatesOperation
		{
			EchoHeader (msg);

			var q = f.Invoke();

			EchoState (q);

			return q;
		}

	}
}

