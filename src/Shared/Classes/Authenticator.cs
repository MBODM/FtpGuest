using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class Authenticator : IAuthenticator
    {
        private string ftpServer;

        public Authenticator(string ftpServer)
        {
            this.ftpServer = ftpServer;
        }

        public bool Authenticate(Credentials credentials)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServer);

                request.Credentials = new NetworkCredential(credentials.Username, credentials.Password);
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.KeepAlive = false;
                request.UseBinary = true;
                request.Timeout = 5000;
                request.Proxy = null;

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
