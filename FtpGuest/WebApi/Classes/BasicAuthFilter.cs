using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using MBODM.FtpGuest.Shared;

namespace MBODM.FtpGuest.WebApi
{
    public sealed class BasicAuthFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var basicAuth = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IBasicAuth)) as IBasicAuth;

            if (basicAuth == null)
            {
                throw new NullReferenceException("DependencyResolver returned null.");
            }

            var scheme = context.Request.Headers.Authorization?.Scheme;
            var parameter = context.Request.Headers.Authorization?.Parameter;

            if ((scheme != null) && (parameter != null) && (scheme == basicAuth.SchemeName))
            {
                var principal = basicAuth.Authenticate(parameter);

                if (principal != null)
                {
                    context.Principal = principal;

                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;

                    return Task.FromResult(0);
                }
            }

            context.ErrorResult = new BasicAuthResult(basicAuth);

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
