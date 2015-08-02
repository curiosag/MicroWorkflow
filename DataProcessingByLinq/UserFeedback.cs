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

		public static void EchoState (OperationEnvironment<DrContext> env)
		{
			ArgumentCheck.Assigned (env, "env");

			var e = env.Exception;

			if (e != null) {
				Echo ();
				Echo ("--- exception type {0} encountered ---", e.GetType ().ToString ());
				Echo (e.Message);
				Echo ();
			}

			Echo ("--- executionTrace ---");

			Echo (env.Trace.Trace);
		}

		public static OperationEnvironment<DrContext> Verbose(string msg, Func<OperationEnvironment<DrContext>> f)
		{
			Echo ("-----------------------------------------------------------------------------------------");
			Echo (msg);
			Echo ();

			var q = f.Invoke();

			EchoState (q);

			return q;
		}


	}
}

