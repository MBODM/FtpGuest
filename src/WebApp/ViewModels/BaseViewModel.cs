using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBODM.FtpGuest.WebApp
{
    public class BaseViewModel
    {
        public string AppName
        {
            get { return "FTP-Guest"; }
        }

        public string AppVersion
        {
            get { return typeof(MvcApplication).Assembly.GetName().Version.ToString(); }
        }

        public bool HasBackButton
        {
            get;
            protected set;
        }
    }
}
