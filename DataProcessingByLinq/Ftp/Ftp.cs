using System;
using System.Collections.Generic;
using System.IO;
using DataProcessingByLinq.Base;

namespace DataProcessingByLinq.Ftp
{
  public abstract class Ftp : IDisposable
  {
    [Mhs("PS", DC.N, T = 55823)]

    public abstract FtpFileProperties GetProperties(string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract bool Exists(string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Get(string remotePath, Stream result);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract bool TryGet(string remotePath, Stream result);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void TryGet(string remotePath, Stream result, int secsTimeout);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Get(string remotePath, string localPath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract bool TryGet(string remotePath, string localPath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void TryGet(string remotePath, string localPath, int secsTimeout);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void TryGet(string remotePath, Stream result, int secsTimeout, OnFtpFinishedCallback onFinished, Action<Exception> onError);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Put(Stream source, string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Delete(string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract IEnumerable<FtpFileProperties> List(string remotePath);

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Connect();

    [Mhs("PS", DC.N, T = 55823)]

    public abstract void Dispose();

  }

}
