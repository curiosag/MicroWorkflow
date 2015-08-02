using System;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq.Ftp
{
	public class FtpFileProperties
	{

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public FtpFileType Type { get; set; }

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public string Name { get; set; }

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public long Size { get; set; }

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public DateTime LastModified { get; set; }

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public FtpFileProperties()
		{
		}

		[Mhs("2012-02-01 15:34", "PS", DC.N, T = 55823)]

		public FtpFileProperties(string name, FtpFileType type)
		{
			Name = name;
			Type = type;
		}
	}
}

