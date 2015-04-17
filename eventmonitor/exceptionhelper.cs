using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace EventMonitor {
    class ExceptionHelper {
        public static String HandleException(Exception e, ObjectQuery query) {
            if (AppConfig.CompabilityPolicy == Compability.CompabilityPolicy.Ignore) {
                return String.Format("Query '{0}' not supported at '{1}' {2}Message: {3}", 
                        query.QueryString, Environment.OSVersion, Environment.NewLine, e.GetBaseException().Message);
            } else {
                throw e;
            }
        }
    }
}
