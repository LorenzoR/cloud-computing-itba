using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using System.IO;
using System.Net;

namespace WebRole1
{
    public class EventDataSource
    {
        private EventDataServiceContext _ServiceContext = null;
        private static bool storageInitialized = false;
        private static CloudQueueClient queueStorage;
        private static object gate = new Object();

        public EventDataSource()
        {
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            _ServiceContext = new EventDataServiceContext(storageAccount.TableEndpoint.ToString(), storageAccount.Credentials);

            // Create the tables
            // In this case, just a single table.  
            storageAccount.CreateCloudTableClient().CreateTableIfNotExist(EventDataServiceContext.EventTableName);
        }

        public IEnumerable<EventDataModel> Select()
        {
            var results = from c in _ServiceContext.EventTable
                          select c;

            var query = results.AsTableServiceQuery<EventDataModel>();
            var queryResults = query.Execute();

            return queryResults;
        }

        public IEnumerable<EventDataModel> Select(string PartitionKey)
        {
            InitializeStorage();


            var results = from c in _ServiceContext.EventTable
                          where c.PartitionKey == PartitionKey
                          select c;

            var query = results.AsTableServiceQuery<EventDataModel>();
            var queryResults = query.Execute();

            EventDataModel queryEvent = (EventDataModel) queryResults.First();

            System.Diagnostics.Trace.WriteLine("***********Artista : " + queryEvent.Artist);

            // queue a message to process the image
            var queue = queueStorage.GetQueueReference("guestthumbs");
            var message = new CloudQueueMessage(String.Format("{0},{1},{2}", PartitionKey, "parte2", "parte3"));
            queue.AddMessage(message);
            System.Diagnostics.Trace.TraceInformation("***********Queued message to process");
            System.Diagnostics.Trace.WriteLine("***********Queued message to process");


            return queryResults;
        }

        public IEnumerable<EventDataModel> SelectMostVisited()
        {
            var results = from c in _ServiceContext.EventTable
                          select c;

            var query = results.AsTableServiceQuery<EventDataModel>();
            var queryResults = query.Execute();

            var mostVisited = from c in queryResults
                              orderby c.VisitCounter descending
                              select c;

            return mostVisited;
            //return queryResults;
        }

        public void Delete(EventDataModel itemToDelete)
        {
           /* System.Diagnostics.Debug.WriteLine("borro artist = " + itemToDelete.Artist);
            System.Diagnostics.Trace.WriteLine("borro artist = " + itemToDelete.Artist);
            */
            _ServiceContext.Detach(itemToDelete);
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToDelete, "*");
            _ServiceContext.DeleteObject(itemToDelete);
            _ServiceContext.SaveChanges();
        }

        public void Insert(EventDataModel newItem)
        {
            _ServiceContext.AddObject(EventDataServiceContext.EventTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        public void Update(EventDataModel itemToUpdate)
        {
            _ServiceContext.Detach(itemToUpdate);
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToUpdate, "*");
            _ServiceContext.UpdateObject(itemToUpdate);
            _ServiceContext.SaveChanges();
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

                    // create queue to communicate with worker role
                    queueStorage = storageAccount.CreateCloudQueueClient();
                    CloudQueue queue = queueStorage.GetQueueReference("guestthumbs");
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