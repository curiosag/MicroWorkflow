using System;
using System.Diagnostics;

namespace DataProcessingByLinq
{
    public class OperationTrace
    {
        public string Trace { get; private set; }
        public string HighWaterMark { get; set; }

        public OperationTrace()
        {
        }

        public OperationTrace(string highWaterMark)
        {
            HighWaterMark = highWaterMark;
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

        public Exception Exception { get; set; }
        public bool HasException
        {
            get
            {
                return Exception != null;
            }
        }

    }
}
