using System;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq
{
	public class Operation: IIndicatesOperation
	{
		private string opId;
		private DrContext context;
		private DrResultStatus opStatus = DrResultStatus.ok;

		public Operation (string operationId, DrContext context, OperationCallback canExecute, OperationCallback onOperationFinished)
		{
			ArgumentCheck.Assigned (context, "context");
		
			opId = operationId;

			this.context = context;
			CanExecuteCallback = canExecute;
			OnOperationFinished = onOperationFinished;
		}

		public Operation (string operationId, Operation previous) : this (operationId, previous.Context, previous.CanExecuteCallback, previous.OnOperationFinished)
		{
			ArgumentCheck.Assigned (previous, "previous");
		}

		public Exception Exception { get; set; }

		public string OperationId {
			get { return opId; }
		}

		public DrResultStatus OpStatus {
			get { return opStatus; }
			set { opStatus = value; }
		}

		public DrContext Context {
			get { return context; }
		}

		public OperationCallback OnOperationFinished { get; set; }

		public void NotifyOperationFinished ()
		{
			Check.Assigned (OnOperationFinished);
			OnOperationFinished (opId);
		}

		public OperationCallback CanExecuteCallback { get; set; }

		public bool CanExecute ()
		{
			Check.Assigned (CanExecuteCallback);
			return CanExecuteCallback (OperationId);
		}

		public bool HasException {
			get {
				return Exception != null;
			}
		}

		public OpPut Put (string path)
		{
			var previous = this;
			var current = new OpPut ("Put", previous, path);

			if (current.CanExecute ()) {

			}
			current.NotifyOperationFinished ();

			return current;

		}
			
		public OpGet Get (string path)
		{
			var previous = this;
			var current = new OpGet ("Get", previous, path);

			if (current.CanExecute ()) {

			}
			current.NotifyOperationFinished ();

			return current;

		}

		public OpCheckResult CheckResult(string path)
		{
			var previous = this;
			var current = new OpCheckResult("Get", previous, path);

			if (current.CanExecute ()) {

			}
			current.OpStatus = Const.checkTkResultStatus;
			current.NotifyOperationFinished ();

			return current;

		}


	}
}
