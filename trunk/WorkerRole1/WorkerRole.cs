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

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudQueue queue;
        private Dictionary<string, int> visitCounter = new Dictionary<string, int>();
        EventDataSource eventDS = null;
        public const int VISITS_NEEDED = 10;

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
                        var partitionKey = messageParts[0];
                        var imageBlobUri2 = messageParts[1];
                        var rowkey = messageParts[2];
                        //Trace.TraceInformation("Processing image in blob '{0}', partitionKey '{1}'.", visitCounter, imageBlobUri);


                        if (visitCounter.ContainsKey(partitionKey))
                        {
                            visitCounter[partitionKey]++;
                        }
                        else {
                            visitCounter.Add(partitionKey, 1);
                        }

                        if (visitCounter[partitionKey] >= VISITS_NEEDED)
                        {
                            EventDataModel evento = eventDS.Select(partitionKey).First();

                            evento.VisitCounter += visitCounter[partitionKey];

                            eventDS.Update(evento);

                            visitCounter[partitionKey] = 0;

                            Trace.TraceInformation("Storage Updated.");

                        }

                        queue.DeleteMessage(msg);

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

            // initialize queue storage 
            CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
            queue = queueStorage.GetQueueReference("guestthumbs");

            Trace.TraceInformation("Creating container and queue...");

            eventDS = new EventDataSource();

            bool storageInitialized = false;
            while (!storageInitialized)
            {
                try
                {
                    

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
    }
}
