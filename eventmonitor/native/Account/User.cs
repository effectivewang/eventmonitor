using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace EventMonitor.Native.Account {
    class User {
        private String sid;
        private NTAccount account;

        public static User Current = new User(Environment.UserDomainName, Environment.UserName);

        public User(String domainName, String userName) {
            account = new NTAccount(Environment.UserDomainName, Environment.UserName);
        }

        public String SID {
            get {
                if (String.IsNullOrEmpty(sid)) {
                    SecurityIdentifier si = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
                    sid = si.Value;
                }
                return sid;
            }
        }

    }
}
