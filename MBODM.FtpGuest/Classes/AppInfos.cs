using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBODM.FtpGuest
{
    public sealed class AppInfos
    {
        public static string AppName
        {
            get { return "FTP-Guest"; }
        }

        public static string AppVersion
        {
            get { return typeof(MvcApplication).Assembly.GetName().Version.ToString(); }
        }

        public static string FtpServer
        {
            get { return "ftp://mbodm.com"; }
        }

        public static string TrashFolder
        {
            get { return "trash"; }
        }

        public static IEnumerable<string> ValidUsernames
        {
            get { return new List<string>() { "guest@mbodm.com", "guest" }; }
        }

        public static string Password
        {
            get;
            set;
        }
    }
}
