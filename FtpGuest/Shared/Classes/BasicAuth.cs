using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class BasicAuth : IBasicAuth
    {
        private readonly IAuthenticator authenticator;
        private readonly string realm;

        public BasicAuth(IAuthenticator authenticator, string realm)
        {
            if (authenticator == null)
            {
                throw new ArgumentNullException(nameof(authenticator));
            }

            if (realm == null)
            {
                throw new ArgumentNullException(nameof(realm));
            }

            this.authenticator = authenticator;
            this.realm = realm;
        }

        public string SchemeName
        {
            get { return "Basic"; }
        }

        public HttpHeader ResponseHeader
        {
            get { return new HttpHeader("WWW-Authenticate", $"Basic realm=\"{realm}\""); }
        }

        public IPrincipal Authenticate(string parameter)
        {
            try
            {
                StaticData.CachedPassword = null;

                var credentials = ConvertParameterToCredentials(parameter);

                if (StaticData.ValidUsers.Contains(credentials.Username))
                {
                    credentials.Username = StaticData.ValidUsers.Last();

                    if (authenticator.Authenticate(credentials))
                    {
                        StaticData.CachedPassword = credentials.Password;

                        var identity = new GenericIdentity(credentials.Username, SchemeName);
                        var principal = new GenericPrincipal(identity, new string[] { "Users" });

                        return principal;
                    }
                }
            }
            catch
            {
                // Nothing
            }

            return null;
        }

        public Credentials ConvertParameterToCredentials(string parameter)
        {
            var bytes = Convert.FromBase64String(parameter);

            var decoded = Encoding.GetEncoding("iso-8859-1").GetString(bytes);

            var credentials = decoded.Split(':');

            return new Credentials(credentials.First(), credentials.Last());
        }

        public string ConvertCredentialsToParameter(Credentials credentials)
        {
            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(credentials.Username + ":" + credentials.Password);

            var encoded = Convert.ToBase64String(bytes);

            return encoded;
        }
    }
}
