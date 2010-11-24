using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Diagnostics.Trace.WriteLine("***********LOOOGGGIIINNN");
        if (Thread.CurrentPrincipal.IsInRole("Administrator"))
        {
          //  System.Diagnostics.Trace.WriteLine("***********ENTRO!!");
            this.SecretContent.Visible = true;
        }

        //System.Diagnostics.Trace.WriteLine("***********NOOO ENTRO!!");
    }
}
