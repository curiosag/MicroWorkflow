using System;

namespace DataProcessingByLinq.Base
{


	[System.AttributeUsage(System.AttributeTargets.All,
		AllowMultiple = true)  // Multiuse attribute.
	]

	public class Mhs : System.Attribute
	{

		public int T;

		public Mhs(string name, DC changeType)
		{
		}

		public Mhs(string timestamp, string name, DC changeType)
		{
		}

	}

}

