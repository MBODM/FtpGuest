using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class DownloadData
    {
        public DownloadData()
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
