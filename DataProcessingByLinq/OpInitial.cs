using System;

namespace DataProcessingByLinq
{
	public class OpInitial: Operation
	{
		public OpInitial(DrContext context): base("initial", context, null, null)
		{
		}

	}
}

