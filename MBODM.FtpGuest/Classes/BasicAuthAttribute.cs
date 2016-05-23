using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MBODM.FtpGuest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class BasicAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var request = filterContext.RequestContext.HttpContext.Request;
            var response = filterContext.RequestContext.HttpContext.Response;

            if (request.Headers["Authorization"] != null)
            {
                if (request.Headers["Authorization"].Contains("Basic "))
                {
                    var encodedText = request.Headers["Authorization"].Replace("Basic ", string.Empty);
                    var decodedText = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(encodedText));

                    var username = decodedText.Split(':').First();
                    var password = decodedText.Split(':').Last();

                    if (AppInfos.ValidUsernames.Contains(username))
                    {
                        if (FtpAuth(AppInfos.ValidUsernames.First(), password))
                        {
                            AppInfos.Password = password;

                            return;
                        }
                    }
                }
            }

            response.AppendHeader("WWW-Authenticate", "Basic realm=\"MBODM\"");
            filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
        }

        private bool FtpAuth(string username, string password)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(AppInfos.FtpServer);

                request.Credentials = new NetworkCredential(AppInfos.ValidUsernames.First(), password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
