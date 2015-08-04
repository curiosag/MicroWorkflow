
namespace DataProcessingByLinq
{

	public class OpCheckResult: Operation
	{
		private string path;

		public OpCheckResult(string operationId, Operation previous, string path):base(operationId, previous)
		{
			this.path = path;
		}

	}


}
