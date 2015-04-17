using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier {
    class CompositeQuerier : EventQuerier {
        private List<EventQuerier> Children { get; set; }
        public CompositeQuerier(List<EventQuerier> children) {
            Children = children;
        }

        public override void Query() {
            throw new NotImplementedException();
        }
    }
}
