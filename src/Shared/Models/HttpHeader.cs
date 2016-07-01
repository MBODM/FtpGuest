using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class HttpHeader
    {
        public HttpHeader()
        {
        }

        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
