using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApp
{
    public sealed class BasicAuthFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var basicAuth = DependencyResolver.Current.GetService<IBasicAuth>();

            if (basicAuth == null)
            {
                throw new NullReferenceException("DependencyResolver returned null.");
            }

            var header = filterContext?.HttpContext?.Request?.Headers?.Get("Authorization");

            if (header != null)
            {
                if (header.StartsWith(basicAuth.SchemeName + " "))
                {
                    var parameter = header.Replace(basicAuth.SchemeName + " ", string.Empty);

                    var principal = basicAuth.Authenticate(parameter);

                    if (principal != null)
                    {
                        filterContext.Principal = principal;

                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;

                        return;
                    }
                }
            }

            filterContext.HttpContext.Response.AppendHeader(basicAuth.ResponseHeader.Name, basicAuth.ResponseHeader.Value);
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            return;
        }
    }
}
