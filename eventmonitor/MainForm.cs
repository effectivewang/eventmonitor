using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventMonitor.Querier.Registry;

namespace EventMonitor {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            EventQueue queue = new EventQueue();
            InstalledSoftwareQuerier querier = new InstalledSoftwareQuerier(queue);
            querier.Query();
            
            StringBuilder builder = new StringBuilder();
            int total = queue.Count;
            Console.WriteLine(String.Format("Total Event Number: {0}", total));
            for (int i = 0; i < total; i++) {
                Event ev = queue.Dequeue();

                listBox1.Items.Add(ev.ToString());
            }
            
        }
    }
}
