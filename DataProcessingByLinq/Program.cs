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
		private static MW<OpInitial> CreateMwf (string executionState)
		{
			return MW<OpInitial>.CreateMwf (new DrContext (new FileBasedFtpEmulator ()), executionState);
		}

		private static MW<OpCheckResult> RunNestedSelect (MW<OpInitial> init)
		{

			return from t in (from s in (from r in init
			                             select r.Put ("ProviderRequest"))
			                  select s.Get ("ProviderResultData"))
			       select t.CheckResult ("");
				
		}

		private static MW<OpGet> RunSelectWhere (MW<OpInitial> env)
		{

			return from v in (from u in (from t in (from s in (from r in env
			                                                   select r.Put ("a"))
			                                        select s.Get ("b"))
			                             select t.CheckResult ("c"))
			                  where u.OpStatus == DrResultStatus.ok
			                  select u)
			       select v.Get ("d");
		}

		private static MW<OpGet> RunSelectWhereDesugared (MW<OpInitial> e)
		{
			return e.Select (x => x.Put ("a"))
				.Select (x => x.Get ("b"))
				.Select (x => x.CheckResult ("x"))
				.Where (x => x.OpStatus == DrResultStatus.ok)
				.Select (x => x.Get ("c"));
		}

		private static MW<OpCheckResult> RunNestedSelectUnsugared<T> (MW<T> env) where T: Operation
		{
			return env.Select (x => x.Put ("a"))
				.Select (x => x.Get ("b"))
				.Select (x => x.CheckResult ("x"));
		}

		public static void RunDemoProcessing (BaseDr dr)
		{
			var q10 = UserFeedback.Verbose ("nested select", () => RunNestedSelect (CreateMwf (Const.EmptyTrace)));

			var q11 = UserFeedback.Verbose ("same select, unsugared and in same wf", () => RunNestedSelectUnsugared (q10));

			q11 = UserFeedback.Verbose ("re-run nested select in new wf, with previous execution trace as high water mark", 
				() => RunNestedSelect (CreateMwf (q11.Trace.HighWaterMark)));

			string partialTrace = "Put" + Const.Eol +	"Get" + Const.Eol;

			var q12 = UserFeedback.Verbose ("re-run nested select in new Wf, with this trace as high water mark:" + Const.Eol + partialTrace, 
				          () => RunNestedSelect (CreateMwf (partialTrace)));
				
			Const.checkTkResultStatus = DrResultStatus.ok;
			var q21 = UserFeedback.Verbose ("select with where clause, positive result, new wf", () => RunSelectWhere (CreateMwf (Const.EmptyTrace)));

			Const.checkTkResultStatus = DrResultStatus.emptyResult;
			var q22 = UserFeedback.Verbose ("select with where clause, negative result, new wf", () => RunSelectWhereDesugared (CreateMwf (Const.EmptyTrace)));

			moreSyntaxVariations ();
		}

		private static void moreSyntaxVariations ()
		{

			var env = CreateMwf (Const.EmptyTrace).SetVerbose (true);

			UserFeedback.EchoHeader ("let sequence in new wf");
			var q7 =  
				     from r in env.Select (x => x.Put ("ProviderRequest"))
			      let s = env.Select (x => x.Get ("ProviderRequestStatus")).Val
			      let d = env.Select (x => x.Get ("ProviderResultData")).Val
			      select new { status = s, data = d };

			UserFeedback.EchoState (env);

			UserFeedback.EchoHeader ("more selects on env, same wf");

			var q8 = 
				from u in env.Select (x => x.Put ("ProviderRequest"))
			         from s in env.Select (x => x.Get ("ProviderRequestStatus"))
			         from d in env.Select (x => x.Get ("ProviderResultData"))
			         let c = env.Select (x => d.CheckResult ("d"))
				select new { name = "checked result data", val = d.OpStatus };

			UserFeedback.EchoState (env);


			UserFeedback.EchoHeader ("and yet more");

			var q9 = 
				from u in env.Select (x => x.Put ("ProviderRequest"))
			         from s in env.Select (x => x.Get ("ProviderRequestStatus"))
			         from d in env.Select (x => x.Get ("ProviderResultData"))
				select d.CheckResult ("s");

			UserFeedback.EchoState (env);

			UserFeedback.EchoHeader ("some solitary operations");

			var supload = env.Select (x => x.Put ("ProviderRequest"));
			var downloadStatus = env.Select (x => x.Put ("ProviderRequestStatus"));
			var downloadData = downloadStatus.Select (x => x.Get ("ProviderResultData"));
			var checkedData = downloadData.Select (x => x.CheckResult ("x"));

			UserFeedback.EchoState (env);

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
