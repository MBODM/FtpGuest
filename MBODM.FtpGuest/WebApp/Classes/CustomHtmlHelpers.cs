using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest.WebApp
{
    public static class CustomHtmlHelpers
    {
        public static string ConvertFileSize(this HtmlHelper htmlHelper, long fileSize)
        {
            var kb = 1024;
            var mb = 1024 * 1024;
            var gb = 1024 * 1024 * 1024;

            if (fileSize < kb)
            {
                return $"({fileSize} Bytes)";
            }
            else if (fileSize < mb)
            {
                return $"({fileSize / kb} KB)";
            }
            else if (fileSize < gb)
            {
                return $"({fileSize / mb} MB)";
            }
            else
            {
                return $"({fileSize / gb} GB)";
            }
        }
    }
}
