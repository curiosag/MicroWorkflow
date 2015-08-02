using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProcessingByLinq.Ftp;

namespace DataProcessingByLinq
{
	class Program
	{
		private static OperationEnvironment<DrContext> getNewEnvironment (Dr def, string executionState)
		{
			return new OperationEnvironment<DrContext> (new DrContext (def, new FileBasedFtpEmulator ()), new OperationTrace (executionState));
		}

		public static void Demo1 (Dr def, string executionState)
		{

			var env = getNewEnvironment (def, executionState);

			// nested select version. only the final step is available in q
			var q = from t in (from s in (from r in env
			                              select r.DrPut (DrKindOfFile.ProviderRequest))
			                   select s.DrGet (DrKindOfFile.ProviderResultData))
			        select t.DrCheckTkResult (t);

			env = getNewEnvironment (def, executionState);

			// same unsugared
			q = env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
                     .Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
				.Select (x => x.DrCheckTkResult (x));
				
			// in each step a new environment is created, conserving the state of the step
			var q6 = from d in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest)).InstanceWithTemplate (env)
			         from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus)).InstanceWithTemplate (env)
			         from t in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData)).InstanceWithTemplate (env)
			         select new { upload = d, status = s, data = t };

			var q6a = from u in env.Exec (x => x.DrPut (DrKindOfFile.ProviderRequest))
			          from s in env.Exec (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
			          from d in env.Exec (x => x.DrGet (DrKindOfFile.ProviderResultData))
			          let c = env.Exec (x => d.DrCheckTkResult (s))
			          select new { name = "checked result data", val = c.Val.DataPath };

			// same for let
			var q7 = from r in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
			         let s = env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus)).Val
			         let d = env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData)).Val
			         select new { status = s, data = d };

		}

		private static void NotifyOuterWorld (Exception e, string executionState)
		{

			if (e != null) {
				Console.WriteLine ();
				Console.WriteLine ("--- exception type {0} encountered ---", e.GetType ().ToString ());
				Console.WriteLine (e.Message);
				Console.WriteLine ();
			}
			Console.WriteLine ("--- executionTrace ---");
			Console.WriteLine (executionState);
		}

		public static void Demo2 (Dr def, string executionState)
		{

			var ftp = new FileBasedFtpEmulator ();
			var env = new OperationEnvironment<DrContext> (new DrContext (def, ftp),
				          new OperationTrace (executionState));
            
			var q1 = from u in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
			         from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
			         from d in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
			         select d.DrCheckTkResult (s);
            
			var supload = env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest));
			var downloadStatus = env.Select (x => x.DrPut (DrKindOfFile.ProviderRequestStatus));
			var downloadData = downloadStatus.Select (x => x.DrGet (DrKindOfFile.ProviderResultData));
			var checkedData = downloadData.Select (x => x.DrCheckTkResult (downloadStatus.Val));

			NotifyOuterWorld (env.Exception, env.Trace.HighWaterMark);

			var q2 = from u in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
			         from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
			         from d in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
			         let c = env.Select (x => d.DrCheckTkResult (d))
			         select new { name = "checked result data", val = d.DataPath };
				
		}

		static void Main (string[] args)
		{
			var executionState = "";

			var trace = "DrPut_ProviderRequest()" + Const.Eol +
			            "DrGet_ProviderResultData()" + Const.Eol;

			Demo1 (new BaseDr (Provider.Bloomberg,
				"BB request",
				"ISIN",
				new List<string> (),
				new List<string> ()),
				executionState);

		}

	}
}
