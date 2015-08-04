using System;
using System.Diagnostics;
using System.IO;
using DataProcessingByLinq.Ftp;

namespace DataProcessingByLinq
{

	public class DrContext
	{
		private readonly Ftp.Ftp _ftp;

		public Ftp.Ftp Ftp {
			get {
				return _ftp;
			}
		}

		public DrContext (Ftp.Ftp targetConnection)
		{
			_ftp = targetConnection;
		}
	
	}

}
