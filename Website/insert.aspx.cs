using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website
{
    public partial class insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            var repository = new Repository();
            stopWatch.Start();
            repository.Insert();
            lbl1.Text = stopWatch.Elapsed.TotalMilliseconds.ToString();
        }
    }
}