using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public interface IAuthenticator
    {
        bool Authenticate(Credentials credentials);
    }
}
