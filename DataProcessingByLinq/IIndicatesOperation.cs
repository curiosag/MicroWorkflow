namespace DataProcessingByLinq
{

    public delegate bool CanExecuteCallback(string operationId);

    public interface IIndicatesOperation
    {
        string OperationId { get; set; }
        CanExecuteCallback CanExecuteCallback { get; set; }
        bool CanExecute();
    }

}
