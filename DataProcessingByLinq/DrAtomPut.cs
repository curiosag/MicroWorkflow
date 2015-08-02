namespace DataProcessingByLinq
{
    public static class DrAtomPut
    {
        
        public static DrContext DrPut(this DrContext current, DrKindOfFile drKindOfFile)
        {
            var path = current.Path(drKindOfFile);
            var next = current.Clone();

			next.OperationId = DrAtom.OperationId("DrPut", drKindOfFile.ToString(), path);
            if (next.CanExecute())
            {
                        
            }
                        
            return next;
           
        }

    }
}
