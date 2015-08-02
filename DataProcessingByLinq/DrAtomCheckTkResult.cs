
namespace DataProcessingByLinq
{
    public static class DrCheckTkResultExt
    {

		public static DrContext DrCheckTkResult(this DrContext tkDataDownloaded, DrContext tkStatus)
        {
			var operationId = DrAtom.OperationId("DrCheckTkResult", DrKindOfFile.LocalRequestStatus.ToString(), tkDataDownloaded.DataPath);
            var next = tkDataDownloaded.Clone();

            //-------- demo --------------
            next.OperationId = operationId;
            if (next.CanExecute())
            {

            }
            return next;
            //----------------------------
            
                       
        }

    }
}
