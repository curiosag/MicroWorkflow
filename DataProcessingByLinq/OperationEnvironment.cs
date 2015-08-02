using System;
using System.Diagnostics;

namespace DataProcessingByLinq
{
    public class OperationEnvironment<T>
    {

        public T Val { get; private set; }
        private OperationTrace _trace;

        public OperationEnvironment()
        {
        }

        public OperationEnvironment(T val)
        {
            Val = val;
        }

        public OperationEnvironment(T val, OperationTrace trace)
        {
            Val = val;
            _trace = trace;
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

        public bool CanExecute(string operationId)
        {
            if (Trace.IsRedo(operationId))
            {
                Console.WriteLine("can exec " + operationId + "? no - is redo");
                return false;

            }
            Console.WriteLine("can exec " + operationId + "? yes");
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
			Func<T, R> selectValue)
		{
			return SelectMany(src, x => selectValue(src.Val).AsOperationEnvironment());

		}

        public static OperationEnvironment<R> Select<T, R>(this OperationEnvironment<T> src,
                                                          Func<T, R> selectValue)
        {
            return SelectMany(src, x => selectValue(src.Val).AsOperationEnvironment());

        }

        private static OperationEnvironment<R> InvokeSave<T, R>(OperationEnvironment<T> src,
                                                              Func<T, OperationEnvironment<R>> selectValue)
        {
            OperationEnvironment<R> dest;
            if (!src.HasException)
            {
                try
                {
                    dest = selectValue(src.Val).Merge(src);
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
                                                              Func<T, OperationEnvironment<R>> selectValue)
        {
            OperationEnvironment<R> dest;

            IIndicatesOperation operationVal = src.Val as IIndicatesOperation;
            if (operationVal != null)
            {
                operationVal.CanExecuteCallback = (src.CanExecute);
            }

            dest = InvokeSave(src, selectValue);

            IIndicatesOperation destVal = dest.Val as IIndicatesOperation;
            if (destVal != null)
            {
                dest.NoteExecutedOperation(destVal.OperationId);
            }


            return dest;
        }


        public static OperationEnvironment<R> SelectMany<T, V, R>(this OperationEnvironment<T> src,
                                                                    Func<T, OperationEnvironment<V>> selectValue,
                                                                    Func<T, V, R> selectResult)
        {
            return src
                       .SelectMany(
                                   x => selectValue(x)
                                                      .SelectMany(
                                                                  y => selectResult(x, y).InstanceWithTemplate(src)
                                                                 )
                                  );
        }
    }

}
