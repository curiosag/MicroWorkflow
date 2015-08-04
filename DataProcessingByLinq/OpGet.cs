using System.IO;

namespace DataProcessingByLinq
{
	public class OpGet: Operation
	{
		private string path;

		public OpGet(string operationId, Operation previous, string path):base(operationId, previous)
		{
			this.path = path;
		}


	}
		
}
