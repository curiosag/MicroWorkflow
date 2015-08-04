namespace DataProcessingByLinq
{
	public class OpPut: Operation
    {
		private string path;

		public OpPut(string operationId, Operation previous, string path):base(operationId, previous)
		{
			this.path = path;
		}

    }

}
