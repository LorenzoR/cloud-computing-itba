using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using WebRole1;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace WorkerRole2
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudQueue queue;
        private CloudBlobContainer container;

        public override void Run()
        {
            Trace.TraceInformation("Listening for queue messages...");

            while (true)
            {
                try
                {
                    // retrieve a new message from the queue
                    CloudQueueMessage msg = queue.GetMessage();
                    if (msg != null)
                    {
                        // parse message retrieved from queue
                        var messageParts = msg.AsString.Split(new char[] { ',' });
                        var imageBlobUri = messageParts[0];
                        var partitionKey = messageParts[1];
                        var rowkey = messageParts[2];
                        Trace.TraceInformation("Processing image in blob '{0}'.", imageBlobUri);

                        string thumbnailBlobUri = System.Text.RegularExpressions.Regex.Replace(imageBlobUri, "([^\\.]+)(\\.[^\\.]+)?$", "$1-thumb$2");

                        CloudBlob inputBlob = container.GetBlobReference(imageBlobUri);
                        CloudBlob outputBlob = container.GetBlobReference(thumbnailBlobUri);

                        using (BlobStream input = inputBlob.OpenRead())
                        using (BlobStream output = outputBlob.OpenWrite())
                        {
                            ProcessImage(input, output);

                            // commit the blob and set its properties
                            output.Commit();
                            outputBlob.Properties.ContentType = "image/jpeg";
                            outputBlob.SetProperties();

                            // update the entry in table storage to point to the thumbnail
                            CommentDataSource ds = new CommentDataSource();
                            ds.UpdateImageThumbnail(partitionKey, rowkey, thumbnailBlobUri);

                            // remove message from queue
                            queue.DeleteMessage(msg);

                            Trace.TraceInformation("Generated thumbnail in blob '{0}'.", thumbnailBlobUri);
                        }

                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (StorageClientException e)
                {
                    Trace.TraceError("Exception when processing queue item. Message: '{0}'", e.Message);
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            #region Setup CloudStorageAccount Configuration Setting Publisher

            // This code sets up a handler to update CloudStorageAccount instances when their corresponding
            // configuration settings change in the service configuration file.
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));

                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        // The corresponding configuration setting has changed, propagate the value
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            // In this case, the change to the storage account credentials in the
                            // service configuration is significant enough that the role needs to be
                            // recycled in order to use the latest settings. (for example, the 
                            // endpoint has changed)
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });
            #endregion

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;



            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            // initialize blob storage
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            container = blobStorage.GetContainerReference("guestbookpics");


            // initialize queue storage 
            CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
            queue = queueStorage.GetQueueReference("commentthumbs");

            Trace.TraceInformation("Creating container and queue for comments photos...");

            bool storageInitialized = false;
            while (!storageInitialized)
            {
                try
                {
                    // create the blob container and allow public access
                    container.CreateIfNotExist();
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);

                    // create the message queue(s)
                    queue.CreateIfNotExist();

                    storageInitialized = true;
                }
                catch (StorageClientException e)
                {
                    if (e.ErrorCode == StorageErrorCode.TransportError)
                    {
                        Trace.TraceError("Storage services initialization failure. "
                          + "Check your storage account configuration settings. If running locally, "
                          + "ensure that the Development Storage service is running. Message: '{0}'", e.Message);
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        public void ProcessImage(Stream input, Stream output)
        {
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = 128;
                height = 128 * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = 128;
                width = 128 * originalImage.Width / originalImage.Height;
            }

            var thumbnailImage = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(thumbnailImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(originalImage, 0, 0, width, height);
            }

            thumbnailImage.Save(output, ImageFormat.Jpeg);
        }
    }
}
