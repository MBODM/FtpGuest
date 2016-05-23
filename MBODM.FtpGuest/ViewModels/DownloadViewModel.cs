using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBODM.FtpGuest
{
    public sealed class DownloadViewModel : BaseViewModel
    {
        public IEnumerable<DownloadData> DownloadData
        {
            get;
            set;
        }
    }
}
