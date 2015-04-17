using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventMonitor {
    class WatchDog {
        public const long DEFAULT_TTL = 5 * 1000;
        [ThreadStatic]
        private volatile long ttl;
        [ThreadStatic]
        private volatile long startMillseconds;
        [ThreadStatic]
        private volatile Boolean monitoring;

        public WatchDog() : this(DEFAULT_TTL) { }

        public WatchDog(long timeToLive){
            this.ttl = timeToLive;
        }

        public void Start() {
            this.Start(Environment.TickCount);
        }

        public void Start(long startMillseconds) {
            Interlocked.Exchange(ref startMillseconds, startMillseconds);
        }

        public void End() {
            Interlocked.Exchange<Boolean>(ref monitoring, true);
        }
    }
}
