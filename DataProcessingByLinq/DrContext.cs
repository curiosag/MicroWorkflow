using System;
using System.Diagnostics;
using System.IO;
using DataProcessingByLinq.Ftp;

namespace DataProcessingByLinq
{

    public class DrContext : IIndicatesOperation
    {
        public Dr Request { get; private set; }

        private readonly Ftp.Ftp _ftp;
		public Ftp.Ftp Ftp
        {
            get
            {
                return _ftp;
            }
        }

		public DrContext(Dr request, Ftp.Ftp targetConnection)
        {
            Request = request;
            _ftp = targetConnection;
        }

        public string OperationId { get; set; }

        public CanExecuteCallback CanExecuteCallback { get; set; }

        public bool CanExecute()
        {
            if (CanExecuteCallback != null)
            {
                return CanExecuteCallback(OperationId);
            }
            return true;
        }

        public string DataPath { get; set; }

        public DrContext Clone()
        {

            return new DrContext(Request, Ftp)
                            {
                                OperationId = OperationId,
                                CanExecuteCallback = CanExecuteCallback,
                                DataPath = DataPath
                            }; 
          
        }

        public virtual string Path(DrKindOfFile kind)
        {
			return "";
        }

        public virtual Stream ProviderRequestFileFormat()
        {
			return null;
        }

    }

}
