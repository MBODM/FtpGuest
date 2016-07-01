using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApp
{
    public sealed class DownloadViewModel : BaseViewModel
    {
        public DownloadViewModel()
        {
            HasBackButton = true;
        }

        public IEnumerable<DownloadData> DownloadData
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        }
    }
}
