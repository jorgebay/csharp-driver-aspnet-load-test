using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Website
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Cassandra.Diagnostics.CassandraTraceSwitch.Level = TraceLevel.Info;
            Trace.Listeners.Add(new TextWriterTraceListener(HttpRuntime.AppDomainAppPath + "\\logs\\" + DateTime.Now.Ticks + ".txt"));

            Trace.TraceInformation("App started");
            Repository.Setup();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Repository.TearDown();
        }
    }
}