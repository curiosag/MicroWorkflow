namespace DataProcessingByLinq.Ftp
{
  public enum FtpExceptionType
  {        
    ConnectFailure,
    ConnectionClosed,
    SocketError,
    NameResolutionFailure,
    ProtocolError,
    ProxyNameResolutionFailure,
    UnclassifiableError,
    ServerProtocolViolation,
    Timeout,
    AsyncError,
    OperationFailure,
    FileNotFound,
    DirectoryUnknown,
    Unknown
  }
}