﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBODM.FtpGuest.Shared
{
    public sealed class Credentials
    {
        public Credentials()
        {
        }

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
