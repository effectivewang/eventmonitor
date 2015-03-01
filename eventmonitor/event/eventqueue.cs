using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace EventMonitor
{
    /// <summary>
    /// Queue of <see cref="Event"/>
    /// </summary>
    class EventQueue : Queue<Event>
    {
        private ReaderWriterLockSlim slim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        public new void Enqueue(Event e) {
            slim.EnterWriteLock();

            try {
                base.Enqueue(e);
            } finally {
                slim.ExitWriteLock();
            }
        }

        public new Event Dequeue() {
            slim.EnterReadLock();

            try {
                return base.Dequeue();
            } finally {
                slim.ExitReadLock();
            }
        }
    }
}
