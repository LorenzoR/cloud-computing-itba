using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using SocialMediaLibrary;

namespace WebRole1
{
    public partial class ShowEventsAdmin : System.Web.UI.Page
    {
        TwitterInfo ti = new TwitterInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Administrator"))
            {
                Page.Response.Redirect("Default.aspx", true);
            }
        }

        protected void btnTweet_Click(object sender, EventArgs e)
        {
            
            TextBox textBox = (TextBox)frmAddComment.FindControl("artistBox");
            String artist = textBox.Text;

            TextBox placeBox = (TextBox)frmAddComment.FindControl("placeBox");
            String place = placeBox.Text;

            TextBox dateBox = (TextBox)frmAddComment.FindControl("dateBox");
            String date = dateBox.Text;

            TwitterInfo.TwitterInfoStatus status = ti.PostTweet("Nuevo evento! " + artist + " en " + place + " el día " + date, false);


            //Warn if we're past the max length
            if (status == TwitterInfo.TwitterInfoStatus.ExceededMaxLength)
            {
                System.Diagnostics.Trace.WriteLine("La longitud maxima del lenguaje es 140");
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("Tweet OK!");
            }
        }

 
    }
}