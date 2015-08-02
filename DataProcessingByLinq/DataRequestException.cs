using System;

namespace DataProcessingByLinq
{
    public class DataRequestException: Exception
    {
        public DataRequestException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
