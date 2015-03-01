using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace EventMonitor.Querier.WMI {
    abstract class WMIEventQuerier : EventQuerier{
        public WMIEventQuerier(EventQueue queue, EventType type) 
            : base(queue, type){
        }

        protected void RunQuery(String queryString) {
            RunQuery(null, queryString);
        }

        protected ManagementObjectCollection RunQuery(String scope, String queryString) {
            ManagementObjectSearcher searcher = CreateSearcher(scope, queryString);
            ManagementObjectCollection collection = searcher.Get();
            if (collection == null) return null;

            try {
                foreach (ManagementObject obj in collection) {
                    Queue.Enqueue(new WMIEvent(Type, obj, Attributes));
                }
            } catch (Exception e) {
                Enqueue(ExceptionHelper.HandleException(e, searcher.Query));
            }

            return collection;
        }

        private ManagementObjectSearcher CreateSearcher(String scope, String queryString) {
            return String.IsNullOrEmpty(scope) ?  new ManagementObjectSearcher(queryString)
                : new ManagementObjectSearcher(scope, queryString);
        }

        /// <summary>
        /// Attributes to display.
        /// </summary>
        protected virtual string[] Attributes { get { return null; } }
    }
}
