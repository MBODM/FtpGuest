using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBODM.FtpGuest
{
    public sealed class DownloadData
    {
        public DownloadData()
            : this(string.Empty, 0, string.Empty)
        {
        }

        public DownloadData(string fileName, long fileSize, string downloadLink)
        {
            FileName = fileName;
            FileSize = fileSize;
            DownloadLink = downloadLink;
        }

        public string FileName
        {
            get;
            set;
        }

        public long FileSize
        {
            get;
            set;
        }

        public string DownloadLink
        {
            get;
            set;
        }
    }
}
