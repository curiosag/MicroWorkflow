using System;

namespace DataProcessingByLinq.Ftp
{

	public class FtpException: Exception
	{

		public FtpException (String message)
			: base (message, null)
		{
		}

		public FtpException (String message, Exception innerException)
			: base (message, innerException)
		{
		}

		public FtpException (FtpExceptionType exceptionType, String message, Exception innerException)
			: base (message, innerException)
		{
		}

		public FtpExceptionType ExceptionType {

			get{ return FtpExceptionType.Unknown; }
			set{ }

		}
	}
}

