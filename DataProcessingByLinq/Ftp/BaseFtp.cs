using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq.Ftp
{

  public abstract class BaseFtp : Ftp
  {
    protected FtpSettings Settings { get; set; }
    protected FtpOperation Op { get; set; }
    private readonly Object mLock = new object();

    [Mhs("PS", DC.N, T = 55823)]

    protected BaseFtp()
    {
    }

    [Mhs("PS", DC.N, T = 55823)]

    protected BaseFtp(FtpSettings settings)
    {
      ArgumentCheck.Assigned(settings, "settings");
      Settings = settings;
    }
			
    [Mhs("PS", DC.N, T = 55823)]

    private bool InvokeFtpOperation(FtpOperation opType, Func<bool> operation)
    {
      lock (mLock)
      {
        try
        {
          Op = opType;
          try
          {
            return operation.Invoke();
          }
          catch (Exception e)
          {
            FtpException decoded;
            if (DecodeException(e, out decoded))
            {
              throw decoded;
            }
            throw;
          }
        }
        finally
        {
          Op = FtpOperation.None;
        }
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    private bool TryInvokeFtpOperation(Func<FtpException, Boolean> exceptionAccepted, FtpOperation opType, Func<bool> operation)
    {
      try
      {
        return InvokeFtpOperation(opType, operation);
      }
      catch (FtpException e)
      {
        if (exceptionAccepted(e))
        {
          return false;
        }
        throw;
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    private void CheckArgumentTimeout(int timeOut)
    {
      ArgumentCheck.Assigned(timeOut, "timeOut");
      Check.GreaterOrEqual(timeOut, 0, "timeOut < 0");
    }

    [Mhs("PS", DC.N, T = 55823)]

    private void CheckArgumentsGet(string remotePath, Stream result)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");
      ArgumentCheck.Assigned(result, "result");
      Check.True(result.CanWrite, "stream must be writable");
    }

    [Mhs("PS", DC.N, T = 55823)]

    private void CheckArgumentsGet(string remotePath, string localPath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");
      ArgumentCheck.Assigned(remotePath, "localPath");
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Get(string remotePath, Stream result)
    {
      CheckArgumentsGet(remotePath, result);
      InvokeFtpOperation(FtpOperation.Get, () => InternalGet(remotePath, result));
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override Boolean TryGet(string remotePath, Stream result)
    {
      CheckArgumentsGet(remotePath, result);

      return TryInvokeFtpOperation(x => x.ExceptionType == FtpExceptionType.FileNotFound,
                                             FtpOperation.Get,
                                             () => InternalGet(remotePath, result));


    }

    private Tuple<bool, Stream> TimeoutDelegateTryGet(string remotePath, Stream result)
    {
      return new Tuple<bool, Stream>(TryGet(remotePath, result), result);
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void TryGet(string remotePath, Stream result, int secsTimeout)
    {
      CheckArgumentsGet(remotePath, result);
      CheckArgumentTimeout(secsTimeout);

      result = new TimedOutExecution<Stream>().Execute(new TimeSpan(0, 0, secsTimeout), () => TimeoutDelegateTryGet(remotePath, result),
                               DebugFormat.Format("Timeout ({0:g} secs) hit while trying to get file {1}", secsTimeout, remotePath));    
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Get(string remotePath, string localPath)
    {
      CheckArgumentsGet(remotePath, localPath);
      using (var s = new FileStream(localPath, FileMode.CreateNew))
      {
        Get(remotePath, s);
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override bool TryGet(string remotePath, string localPath)
    {
      CheckArgumentsGet(remotePath, localPath);
      using (var s = new FileStream(localPath, FileMode.CreateNew))
      {
        return TryGet(remotePath, s);
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void TryGet(string remotePath, string localPath, int secsTimeout)
    {
      CheckArgumentsGet(remotePath, localPath);
      CheckArgumentTimeout(secsTimeout);
      using (var s = new FileStream(localPath, FileMode.CreateNew))
      {
        TryGet(remotePath, s, secsTimeout);
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void TryGet(string remotePath, Stream result, int secsTimeout, OnFtpFinishedCallback onFinished, Action<Exception> onError)
    {
      throw new NotImplementedException("TryGet(string remotePath, Stream result, int secsTimeout, OnFtpFinishedCallback onFinished, Action<Exception> onError)");
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override FtpFileProperties GetProperties(string remotePath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");

      var fileProperties = new FtpFileProperties("", FtpFileType.Unknown);

      try
      {
        InvokeFtpOperation(FtpOperation.List, () => InternalGetProperties(remotePath, fileProperties));
      }
      catch (FtpException e)
      {
        if (e.ExceptionType != FtpExceptionType.OperationFailure)
        {
          throw;
        }
      }
      return fileProperties;
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override bool Exists(string remotePath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");

      try
      {
        GetProperties(remotePath);
        return true;
      }
      catch (FtpException e)
      {
        if (e.ExceptionType == FtpExceptionType.FileNotFound)
        {
          return false;
        }
        throw;
      }
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Put(Stream source, string remotePath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");
      ArgumentCheck.Assigned(source, "source");
      Check.True(source.CanRead, "source stream must be readable");

      InvokeFtpOperation(FtpOperation.Put, () => InternalPut(source, remotePath));
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Delete(string remotePath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");

      InvokeFtpOperation(FtpOperation.Delete, () => InternalDelete(remotePath));
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override IEnumerable<FtpFileProperties> List(string remotePath)
    {
      ArgumentCheck.Assigned(remotePath, "remotePath");

      var result = new List<FtpFileProperties>();
      InvokeFtpOperation(FtpOperation.List, () => InternalList(remotePath, result));
      return result;
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Connect()
    {
      InvokeFtpOperation(FtpOperation.Connect, InternalConnect);
    }

    [Mhs("PS", DC.N, T = 55823)]

    public override void Dispose()
    {
      InvokeFtpOperation(FtpOperation.Finalize, InternalDispose);
    }

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool DecodeException(Exception e, out FtpException result);

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalGetProperties(string remotePath, FtpFileProperties properties);

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalGet(string remotePath, Stream result);

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalPut(Stream source, string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalConnect();

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalDispose();

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalDelete(string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    protected abstract bool InternalList(string remotePath, ICollection<FtpFileProperties> result);

    [Mhs("PS", DC.N, T = 55823)]

    private bool Timeout(int secsTimeout)
    {
			return false;
	}

  }

}
