using System;
using System.Diagnostics;

namespace DataProcessingByLinq
{
    public class OperationEnvironment<T>
    {

        public T Val { get; private set; }
        private OperationTrace _trace;
		private bool verbose = true;
		private bool canContinue = false;

        public OperationEnvironment()
        {
        }

        public OperationEnvironment(T val)
        {
            Val = val;
        }

		public OperationEnvironment<T> SetVerbose(bool b)
		{
			verbose = b;
			return this;
		}

		public OperationEnvironment(T val, OperationTrace trace)
		{
			Val = val;
			_trace = trace;
		}

		public OperationEnvironment<T> SetCanContinue(bool canContinue)
		{
			this.canContinue = canContinue;
			return this;
		}

        public OperationTrace Trace
        {
            get
            {
                return _trace;
            }
            set { _trace = value; }
        }

        public OperationEnvironment<T> NoteExecutedOperation(string operationId)
        {
            Trace.Note(operationId);
            return this;
        }

		private void Echo(string msg)
		{
			if (verbose)
				Console.WriteLine (msg);
		}

        public bool CanExecute(string operationId)
        {
            if (Trace.IsRedo(operationId))
            {
                Echo("can exec " + operationId + "? no - is redo");
                return false;
            }
			if (canContinue) {
				Echo("can exec " + operationId + "? no - sequence terminated due to negative where condition");
				return false;
			}
			Echo("can exec " + operationId + "? yes");
            return true;
        }

        public Exception Exception
        {
            get { return Trace.Exception; }
            set { Trace.Exception = value; }
        }
        public bool HasException
        {
            get
            {
                return Trace.Exception != null;
            }
        }

    }

    public static class OperationEnvironmentExt
    {

        public static OperationEnvironment<T> Merge<S, T>(this OperationEnvironment<T> to, OperationEnvironment<S> from)
        {
            to.Trace = from.Trace;
            return to;
        }

        public static OperationEnvironment<T> AsOperationEnvironment<T>(this T val)
        {
            return new OperationEnvironment<T>(val);
        }

        public static OperationEnvironment<T> InstanceWithTemplate<S, T>(this T val, OperationEnvironment<S> from)
        {
            return val.AsOperationEnvironment().Merge(from);
        }

		public static OperationEnvironment<R> Exec<T, R>(this OperationEnvironment<T> src,
			Func<T, R> f)
		{
			return SelectMany(src, x => f(src.Val).AsOperationEnvironment());

		}

        public static OperationEnvironment<R> Select<T, R>(this OperationEnvironment<T> src,
                                                          Func<T, R> f)
        {
            return SelectMany(src, x => f(src.Val).AsOperationEnvironment());

        }

        private static OperationEnvironment<R> InvokeSave<T, R>(OperationEnvironment<T> src,
                                                              Func<T, OperationEnvironment<R>> f)
        {
            OperationEnvironment<R> dest;
            if (!src.HasException)
            {
                try
                {
                    dest = f(src.Val).Merge(src);
                }
                catch (DataRequestException e)
                {
                    dest = new OperationEnvironment<R>().Merge(src);
                    dest.Exception = e;
                }
            }
            else
            {
                dest = new OperationEnvironment<R>().Merge(src);
            }
            return dest;
        }

        public static OperationEnvironment<R> SelectMany<T, R>(this OperationEnvironment<T> src,
                                                              Func<T, OperationEnvironment<R>> f)
        {
            OperationEnvironment<R> dest;

            IIndicatesOperation operationVal = src.Val as IIndicatesOperation;
            if (operationVal != null)
            {
                operationVal.CanExecuteCallback = (src.CanExecute);
            }

            dest = InvokeSave(src, f);

            IIndicatesOperation destVal = dest.Val as IIndicatesOperation;
            if (destVal != null)
            {
                dest.NoteExecutedOperation(destVal.OperationId);
            }
				
            return dest;
        }

        public static OperationEnvironment<R> SelectMany<T, V, R>(this OperationEnvironment<T> src,
                                                                    Func<T, OperationEnvironment<V>> f,
                                                                    Func<T, V, R> selectResult)
        {
            return src
                       .SelectMany(
                                   x => f(x)
                                                      .SelectMany(
                                                                  y => selectResult(x, y).InstanceWithTemplate(src)
                                                                 )
                                  );
        }

		public static OperationEnvironment<T> Where<T>(this OperationEnvironment<T> source, Func<T, bool> predicate)
		{
			return source.SetCanContinue (! predicate.Invoke(source.Val));
		}

		public static OperationEnvironment<T> Where<T>(this OperationEnvironment<T> source, Func<T, int, bool> predicate)
		{
			return Where(source, (T src) => predicate.Invoke(src, 0));
		}
    }

}