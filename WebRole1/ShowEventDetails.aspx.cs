using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using System.Net;
using WebRole1;

namespace WebRole1
{
    public partial class ShowEventDetails : System.Web.UI.Page
    {

        private static bool storageInitialized = false;
        private static object gate = new Object();
        private static CloudBlobClient blobStorage;
        private static CloudQueueClient queueStorage;

        protected void Page_Load(object sender, EventArgs e)
        {
            string partitionKey = Server.HtmlEncode(Request.QueryString["RowKey"]);
            // queue a message to process the image
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            queueStorage = storageAccount.CreateCloudQueueClient();
            var queue = queueStorage.GetQueueReference("guestthumbs");
            var message = new CloudQueueMessage(partitionKey);
            queue.AddMessage(message);

            System.Diagnostics.Trace.WriteLine("Queued message to process visit counter");
        }

        protected void SignButton_Click(object sender, EventArgs e)
        {
            string eventIDGet = Server.HtmlEncode(Request.QueryString["RowKey"]);

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

                CommentDataModel entry = new CommentDataModel() { eventID = eventIDGet, Username = UsernameTextBox.Text, Message = MessageTextBox.Text, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };

                CommentDataSource ds = new CommentDataSource();
                //ds.AddGuestBookEntry(entry);
                ds.Insert(entry);
                System.Diagnostics.Trace.TraceInformation("Added entry PK: '{0}' RK '{1}' in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.Message);

                // queue a message to process the image
                var queue = queueStorage.GetQueueReference("commentthumbs");
                var message = new CloudQueueMessage(String.Format("{0},{1},{2}", blob.Uri.ToString(), entry.PartitionKey, entry.RowKey));
                queue.AddMessage(message);
                System.Diagnostics.Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
            }
            else
            {
                CommentDataModel entry = new CommentDataModel() { eventID = eventIDGet, Username = UsernameTextBox.Text, Message = MessageTextBox.Text, PhotoUrl = "empty.jpg", ThumbnailUrl = "empty.jpg" };

                CommentDataSource ds = new CommentDataSource();
                //ds.AddGuestBookEntry(entry);
                ds.Insert(entry);
                System.Diagnostics.Trace.TraceInformation("Added entry PK: '{0}' RK '{1}' in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.Message);
            }

            NameTextBox.Text = "";
            MessageTextBox.Text = "";

            //DataList1.DataBind();
            commentData1.DataBind();

            Page.Response.Redirect("ShowEventDetails.aspx?RowKey=" + eventIDGet, true);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            commentData1.DataBind();
        }

        private void InitializeStorage()
        {
            if (storageInitialized)
            {
                return;
            }

            lock (gate)
            {
                if (storageInitialized)
                {
                    return;
                }

                try
                {
                    // read account configuration settings
                    var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

                    // create blob container for images
                    blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference("guestbookpics");
                    container.CreateIfNotExist();

                    // configure container for public access
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);

                    // create queue to communicate with worker role
                    queueStorage = storageAccount.CreateCloudQueueClient();
                    CloudQueue queue = queueStorage.GetQueueReference("commentthumbs");
                    queue.CreateIfNotExist();

                    queue = queueStorage.GetQueueReference("guestthumbs");
                    queue.CreateIfNotExist();
                }
                catch (WebException)
                {
                    throw new WebException("Storage services initialization failure. "
                        + "Check your storage account configuration settings. If running locally, "
                        + "ensure that the Development Storage service is running.");
                }

                storageInitialized = true;
            }
        }

    }
}