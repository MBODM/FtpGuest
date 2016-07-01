using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public interface IBasicAuth
    {
        string SchemeName
        {
            get;
        }

        HttpHeader ResponseHeader
        {
            get;
        }

        IPrincipal Authenticate(string parameter);
        Credentials ConvertParameterToCredentials(string parameter);
        string ConvertCredentialsToParameter(Credentials credentials);
    }
}
