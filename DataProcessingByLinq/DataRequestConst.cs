using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeFtpByLinq
{
    public class DataRequestConst
    {
        public const int FtpPollingSleepMsec = 10000;

        public const int BbMaxSecsRetry = 300;

        public static readonly string Eol = Environment.NewLine;
        public const int MsecPerSec = 1000;
        public const bool FailOnGetStatus = false;
        public const string DigraphFillColor = "orange";
        public const string DigraphFontSize = "12";

        public const string BbOutFileExtension = ".out";
        public const string BbReqFileExtension = ".req";
        public const string BbErrFileExtension = ".err";
        public const int BbProviderReplyTimeout = 10;

        public const string BbEncryptedFileSuffix = ".enc";

    }
}
