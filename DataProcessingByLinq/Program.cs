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

		private static OperationEnvironment<DrContext> RunNestedSelect (OperationEnvironment<DrContext> env)
		{

			return from t in (from s in (from r in env
			                             select r.DrPut (DrKindOfFile.ProviderRequest))
			                  select s.DrGet (DrKindOfFile.ProviderResultData))
			       select t.DrCheckTkResult (t);
				
		}

		private static OperationEnvironment<DrContext> RunNestedSelectUnsugared (OperationEnvironment<DrContext> env)
		{
			return env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
				.Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
				.Select (x => x.DrCheckTkResult (x));
		}

		public static void RunDemoProcessing (Dr def)
		{
			var q10 = UserFeedback.Verbose ("nested select", () => RunNestedSelect (getNewEnvironment (def, "")));

			var q11 = UserFeedback.Verbose ("same select, unsugared and in sequence", () => RunNestedSelectUnsugared (q10));

			q11 = UserFeedback.Verbose ("re-run nested select in new environment, with previous execution trace as high water mark", 
				() => RunNestedSelect (getNewEnvironment (def, q11.Trace.HighWaterMark)));

			string partialTrace = "DrPut_ProviderRequest()" + Const.Eol +	"DrGet_ProviderResultData()" + Const.Eol;

			var q12 = UserFeedback.Verbose ("re-run nested select in new environment, with this trace as high water mark:" + Const.Eol + partialTrace, 
				          () => RunNestedSelect (getNewEnvironment (def, partialTrace)));
				



		}
			
		private void moreSyntaxVariations(){

			var env = getNewEnvironment (null, "").SetVerbose (false);

			// in each step a new environment is created (InstanceWithTemplate), conserving the state of the step
			var q6 = from d in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest)).InstanceWithTemplate (env)
				from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus)).InstanceWithTemplate (env)
				from t in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData)).InstanceWithTemplate (env)
				select new { upload = d, status = s, data = t };

			var q6a = from u in env.Exec (x => x.DrPut (DrKindOfFile.ProviderRequest))
				from s in env.Exec (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
				from d in env.Exec (x => x.DrGet (DrKindOfFile.ProviderResultData))
				let c = env.Exec (x => d.DrCheckTkResult (s))
				select new { name = "checked result data", val = c.Val.DataPath };

			var q7 = from r in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
				let s = env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus)).Val
				let d = env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData)).Val
				select new { status = s, data = d };

			var q8 = from u in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
				from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
				from d in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
				let c = env.Select (x => d.DrCheckTkResult (d))
				select new { name = "checked result data", val = d.DataPath };

			var q9 = from u in env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest))
				from s in env.Select (x => x.DrGet (DrKindOfFile.ProviderRequestStatus))
				from d in env.Select (x => x.DrGet (DrKindOfFile.ProviderResultData))
				select d.DrCheckTkResult (s);

			var supload = env.Select (x => x.DrPut (DrKindOfFile.ProviderRequest));
			var downloadStatus = env.Select (x => x.DrPut (DrKindOfFile.ProviderRequestStatus));
			var downloadData = downloadStatus.Select (x => x.DrGet (DrKindOfFile.ProviderResultData));
			var checkedData = downloadData.Select (x => x.DrCheckTkResult (downloadStatus.Val));

		}

		static void Main (string[] args)
		{
			// to show, how a request may look like, isn't actually used now
			var request = new BaseDr (Provider.Bloomberg, 
				"BB request",
				"ISIN",
				new List<string> (),
				new List<string> ());

			RunDemoProcessing (request);
		}

	}
}
