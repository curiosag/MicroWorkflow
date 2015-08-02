namespace DataProcessingByLinq
{
    public enum Provider
    {
        Telekurs,
        Bloomberg
    }

    public enum RequestStatus
    {
        InProcess,
        Success,
        InvalidSettings,
        ConnectionError,
        OperationError
    }

    public enum DrKindOfFile
    {
        ProviderRequest,
        ProviderResultData,
        ProviderRequestStatus,
        LocalResultData,
        LocalRequestStatus
    }
}
