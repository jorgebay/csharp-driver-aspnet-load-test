using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website
{
    public partial class settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["level"] == "verbose")
            {
                Cassandra.Diagnostics.CassandraTraceSwitch.Level = System.Diagnostics.TraceLevel.Verbose;
            }
            else if (Request["level"] == "info")
            {
                Cassandra.Diagnostics.CassandraTraceSwitch.Level = System.Diagnostics.TraceLevel.Info;
            }
            lbl1.Text = "Trace level: " + Cassandra.Diagnostics.CassandraTraceSwitch.Level.ToString();
        }
    }
}