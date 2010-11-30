using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace WebRole1
{
    public partial class ShowEventDetailsAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Administrator"))
            {
                Page.Response.Redirect("Default.aspx", true);
            }
        }

        protected void SignButton_Click(object sender, EventArgs e)
        {
            /*string eventIDGet = Server.HtmlEncode(Request.QueryString["PartitionKey"]);

            if (FileUpload1.HasFile)
            {
                InitializeStorage();

                // upload the image to blob storage
                string uniqueBlobName = string.Format("guestbookpics/image_{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(FileUpload1.FileName));
                CloudBlockBlob blob = blobStorage.GetBlockBlobReference(uniqueBlobName);
                blob.Properties.ContentType = FileUpload1.PostedFile.ContentType;
                blob.UploadFromStream(FileUpload1.FileContent);
                System.Diagnostics.Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", FileUpload1.FileName, uniqueBlobName);

                // create a new entry in table storage
                //CommentDataModel entry = new CommentDataModel() { eventID = NameTextBox.Text, Message = MessageTextBox.Text, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };

                CommentDataModel entry = new CommentDataModel() { eventID = eventIDGet, Message = MessageTextBox.Text, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };

                CommentDataSource ds = new CommentDataSource();
                //ds.AddGuestBookEntry(entry);
                ds.Insert(entry);
                System.Diagnostics.Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, "");

                // queue a message to process the image
                var queue = queueStorage.GetQueueReference("commentthumbs");
                var message = new CloudQueueMessage(String.Format("{0},{1},{2}", blob.Uri.ToString(), entry.PartitionKey, entry.RowKey));
                queue.AddMessage(message);
                System.Diagnostics.Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
            }
            else
            {
                CommentDataModel entry = new CommentDataModel() { eventID = eventIDGet, Message = MessageTextBox.Text, PhotoUrl = "empty.jpg", ThumbnailUrl = "empty.jpg" };

                CommentDataSource ds = new CommentDataSource();
                //ds.AddGuestBookEntry(entry);
                ds.Insert(entry);
                System.Diagnostics.Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.PhotoUrl);
            }

            NameTextBox.Text = "";
            MessageTextBox.Text = "";

            //DataList1.DataBind();
            commentData1.DataBind();

            Page.Response.Redirect("ShowEventDetails.aspx?PartitionKey=" + eventIDGet, true);*/
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //commentData1.DataBind();
        }
    }
}