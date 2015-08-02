using System.IO;

namespace DataProcessingByLinq
{
	public static class DrAtomGet
	{
		public static DrContext DrTryGet (this DrContext current, DrKindOfFile drKindOfFile, int secsTimeout)
		{
			var srcPath = current.Path (drKindOfFile);
			var destPath = current.Path (drKindOfFile);

			var next = current.Clone ();
			next.OperationId = DrAtom.OperationId ("DrGet", drKindOfFile.ToString(), srcPath);

			if (next.CanExecute ()) {
				// ftp get
			}

			return next;
		}

		public static DrContext DrGet (this DrContext current, DrKindOfFile drKindOfFile)
		{
			return DrTryGet (current, drKindOfFile, 0);
		}

	}


}
