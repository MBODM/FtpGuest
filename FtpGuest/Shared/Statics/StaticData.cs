using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public static class StaticData
    {
        public static string FtpServer
        {
            get { return "mbodm.com"; }
        }

        public static IEnumerable<string> ValidUsers
        {
            get { return new List<string>() { "guest", "guest@mbodm.com" }; }
        }

        public static string CachedPassword
        {
            get;
            set;
        }
    }
}
