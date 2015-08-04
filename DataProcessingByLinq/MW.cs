using System;
using System.Diagnostics;

namespace DataProcessingByLinq
{
	public class MW<T>
	{

		public T Val { get; private set; }

		private OperationTrace trace;
		private bool verbose = true;
		private bool breakCondition = false;

		public MW<T> SetVerbose (bool b)
		{
			verbose = b;
			return this;
		}

		public MW (T val, OperationTrace traceUntilNow) 
		{
			Val = val;
			SetCallbacks (val);
			// there must exist only one instance of trace per usage sequence of Mw
			// if each Mw in every step s of execution would have its own trace, 
			// that trace would only list the steps until s but not further
			// and users of Mw would have to take care, only to start further operations
			// from the instance of the last step.
			trace = traceUntilNow;
		}

		public MW (T val, string traceUntilNow) : this(val, new OperationTrace(traceUntilNow))
		{
		}

		public static MW<OpInitial> CreateMwf(DrContext context)
		{
			return CreateMwf(context, "");
		}

		public static MW<OpInitial> CreateMwf(DrContext context, string highWaterMark)
		{
			return (new OpInitial(context)).AsMw(highWaterMark);
		}

		private U SetCallbacks<U>(U val)
		{
			if (val is IIndicatesOperation) {
				(val as IIndicatesOperation).CanExecuteCallback = CanExecute;
				(val as IIndicatesOperation).OnOperationFinished = NoteExecutedOperation;
			}
			return val;
		}
			
		public MW<T> SetBreakCondition (bool breakCondition)
		{
			this.breakCondition = breakCondition;
			return this;
		}

		public OperationTrace Trace {
			get {
				return trace;
			}
		}

		public bool NoteExecutedOperation (string operationId)
		{
			Trace.Note (operationId);
			return true;
		}

		private void Echo (string msg)
		{
			if (verbose)
				Console.WriteLine (msg);
		}

		public bool CanExecute (string operationId)
		{
			if (Trace.IsRedo (operationId)) {
				Echo ("can exec " + operationId + "? no - is redo");
				return false;
			}
			if (breakCondition) {
				Echo ("can exec " + operationId + "? no - sequence terminated due to negative where condition");
				return false;
			}
			Echo ("can exec " + operationId + "? yes");
			return true;
		}

	}

	public static class MwfExtensions
	{
		public static MW<T> AsMw<T> (this T val, string highWaterMark)
		{
			return new MW<T> (val, highWaterMark);
		}

		public static MW<T> AsMw<T, U> (this T val, MW<U> previous)
		{
			return new MW<T> (val, previous.Trace);
		}

		public static MW<R> Select<T, R> (this MW<T> src, Func<T, R> f)
		{
			return SelectMany (src, x => f (src.Val).AsMw (src));
		}

		public static MW<R> SelectMany<T, R> (this MW<T> src,
		                                       Func<T, MW<R>> f)
		{
			return  f (src.Val);
		}

		public static MW<R> SelectMany<T, V, R> (this MW<T> src,
		                                          Func<T, MW<V>> f,
		                                          Func<T, V, R> selectResult)
		{

			return src
                       .SelectMany (
				x => f (x)
                                                      .SelectMany (
					y => selectResult (x, y).AsMw (src)
				)
			);

		}

		public static MW<T> Where<T> (this MW<T> source, Func<T, bool> predicate)
		{
			return source.SetBreakCondition (!predicate.Invoke (source.Val));
		}

		public static MW<T> Where<T> (this MW<T> source, Func<T, int, bool> predicate)
		{
			return Where (source, (T src) => predicate.Invoke (src, 0));
		}
	}

}