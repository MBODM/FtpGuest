using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApi
{
    public sealed class BasicAuthResult : IHttpActionResult
    {
        private IBasicAuth basicAuth;

        public BasicAuthResult(IBasicAuth basicAuth)
        {
            if (basicAuth == null)
            {
                throw new ArgumentNullException(nameof(basicAuth));
            }

            this.basicAuth = basicAuth;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            response.Headers.Add(basicAuth.ResponseHeader.Name, basicAuth.ResponseHeader.Value);

            return Task.FromResult(response);
        }
    }
}
