﻿using System;
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
        if (Thread.CurrentPrincipal.IsInRole("Administrator"))
        {
            Page.Response.Redirect("ShowEventsAdmin.aspx", true);
            //this.SecretContent.Visible = true;
        }

    }
}
