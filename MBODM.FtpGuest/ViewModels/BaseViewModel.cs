using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBODM.FtpGuest
{
    public class BaseViewModel
    {
        public string AppName
        {
            get { return AppInfos.AppName; }
        }

        public string AppVersion
        {
            get { return AppInfos.AppVersion; }
        }

        // Add stuff here, used in shared layout and/or by all view models.
    }
}
