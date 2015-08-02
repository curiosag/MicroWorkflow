using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq.Ftp
{

	public class FileBasedFtpEmulator : BaseFtp
	{

		[Mhs ("PS", DC.N, T = 55823)]

		protected override bool DecodeException (Exception e, out FtpException result)
		{
			result = null;
			if (e is FileNotFoundException) {
				result = new FtpException (FtpExceptionType.FileNotFound, e.Message, e);
			} else if (e is DirectoryNotFoundException) {
				result = new FtpException (FtpExceptionType.DirectoryUnknown, e.Message, e);
			} else if (e is IOException) {
				result = new FtpException (FtpExceptionType.OperationFailure, e.Message, e);
			}

			return result != null;
		}


		[Mhs ("PS", DC.N, T = 55823)]

		private bool TryInternalGet (string remotePath, Stream result, bool suppressFileLockedException)
		{
			try {
				using (var source = new FileStream (remotePath, FileMode.Open)) {
					source.CopyTo (result);
				}
				return true;
			} catch (IOException e) {
				if (suppressFileLockedException && e.Message.StartsWith ("The process cannot access the file") && e.Message.EndsWith ("because it is being used by another process.")) {
					return false;
				} else {
					throw;
				}
			}
		}

		[Mhs ("PS", DC.N, T = 55823)]

		protected override bool InternalGet (string remotePath, Stream result)
		{
			ArgumentCheck.Assigned (remotePath, "remotePath");
			ArgumentCheck.Assigned (result, "result");

			if (File.Exists (remotePath)) {
				const bool suppressFileLockedException = true;

				if (!TryInternalGet (remotePath, result, suppressFileLockedException)) {
					Thread.Sleep (Constants.WaitTimeForGetOfLockedFile);
					TryInternalGet (remotePath, result, !suppressFileLockedException);
				}
				return true;
			}
			return false;
		}

		[Mhs ("PS", DC.N, T = 55823)]

		protected override bool InternalDelete (string remotePath)
		{
			ArgumentCheck.Assigned (remotePath, "remotePath");
			if (File.Exists (remotePath)) {
				File.Delete (remotePath);
			}
			return true;
		}

		[Mhs ("PS", DC.N, T = 55823)]

		protected override bool InternalGetProperties (string remotePath, FtpFileProperties properties)
		{
			return false;
		}

		[Mhs ("PS", DC.N, T = 55823)]

		protected override bool InternalPut(Stream source, string remotePath)
		{
			return false;
		}

		[Mhs("PS", DC.N, T = 55823)]

		protected override bool InternalConnect()
		{
			return false;
		}

		[Mhs("PS", DC.N, T = 55823)]

		protected override bool InternalDispose()
		{
			return false;
		}


		[Mhs("PS", DC.N, T = 55823)]

		protected override bool InternalList(string remotePath, ICollection<FtpFileProperties> result)
		{
			return false;
		}


	}
}
