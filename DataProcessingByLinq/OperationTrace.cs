using System;
using System.Diagnostics;

namespace DataProcessingByLinq
{
    public class OperationTrace
    {

        public string Trace { get; private set; }
		public string HighWaterMark { get; private set; }

        public OperationTrace(string highWaterMark)
        {
			Trace = "";
            HighWaterMark = highWaterMark;
        }

		public OperationTrace(OperationTrace traceTillNow): this(traceTillNow.HighWaterMark)
		{
			Trace = traceTillNow.Trace;
		}

        private string TraceExtendedBy(string operationId)
        {
            if (! string.IsNullOrEmpty(operationId))
            {
                return Trace + operationId + Const.Eol;
            }
            return Trace;
        }
			
        public string Note(string operationId)
        {

            if (!IsRedo(operationId))
            {
                HighWaterMark = TraceExtendedBy(operationId);
            }
            Trace = TraceExtendedBy(operationId);
            return Trace;
        }

        public bool IsRedo(string operationId)
        {
            return HighWaterMark.StartsWith(TraceExtendedBy(operationId));
        }
			
    }
}
