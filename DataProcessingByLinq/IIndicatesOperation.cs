namespace DataProcessingByLinq
{

    public delegate bool OperationCallback(string operationId);

    public interface IIndicatesOperation
    {
        string OperationId { get; }
        OperationCallback CanExecuteCallback { get; set; }
		OperationCallback OnOperationFinished { get; set; }
        bool CanExecute();

    }

}
