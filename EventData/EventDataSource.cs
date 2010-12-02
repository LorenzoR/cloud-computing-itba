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

        private static CloudBlobClient blobStorage;

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

        public IEnumerable<EventDataModel> Select(string RowKey)
        {
            InitializeStorage();


            var results = from c in _ServiceContext.EventTable
                          where c.RowKey == RowKey
                          select c;

            var query = results.AsTableServiceQuery<EventDataModel>();
            var queryResults = query.Execute();

            /*EventDataModel queryEvent = (EventDataModel) queryResults.First();
            
            // queue a message to process the image
            var queue = queueStorage.GetQueueReference("guestthumbs");
            var message = new CloudQueueMessage(PartitionKey);
            queue.AddMessage(message);

            System.Diagnostics.Trace.WriteLine("Queued message to process");
            */

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
        }

        public void Delete(EventDataModel itemToDelete)
        {
            System.Diagnostics.Trace.WriteLine("Borrando " + itemToDelete.PartitionKey);
            System.Diagnostics.Trace.WriteLine("Voy a borrar " + itemToDelete.Artist);
            _ServiceContext.Detach(itemToDelete);
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToDelete, "*");
            _ServiceContext.DeleteObject(itemToDelete);
            _ServiceContext.SaveChanges();/*
            var item = (from i in _ServiceContext.EventTable
                        where i.PartitionKey == itemToDelete.PartitionKey
                        select i).Single();
            _ServiceContext.DeleteObject(item);
            _ServiceContext.SaveChanges();*/
        }

        public void Insert(EventDataModel newItem)
        {
            newItem.PartitionKey = newItem.EventDate.Replace("-", "");
            _ServiceContext.AddObject(EventDataServiceContext.EventTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        public void Update(EventDataModel itemToUpdate)
        {
            System.Diagnostics.Trace.WriteLine("Actualizando " + itemToUpdate.PartitionKey);
            System.Diagnostics.Trace.WriteLine("Actualizando " + itemToUpdate.Artist);
            System.Diagnostics.Trace.WriteLine("Actualizando " + itemToUpdate.Place);
            _ServiceContext.Detach(itemToUpdate);
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToUpdate, "*");
            _ServiceContext.UpdateObject(itemToUpdate);
            _ServiceContext.SaveChangesWithRetries();
            /*var item = (from i in _ServiceContext.EventTable
                        where i.PartitionKey == itemToUpdate.PartitionKey
                        select i).Single();
            item.Artist = itemToUpdate.Artist;
            item.Place = itemToUpdate.Place;
            item.Description = itemToUpdate.Description;
            item.PartitionKey = itemToUpdate.PartitionKey;
            item.RowKey = itemToUpdate.RowKey;
            item.EventDate = itemToUpdate.EventDate;
            item.Timestamp = itemToUpdate.Timestamp;
            item.VisitCounter = itemToUpdate.VisitCounter;
            _ServiceContext.UpdateObject(item);
            _ServiceContext.SaveChanges();*/
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
                    CloudQueue queue2 = queueStorage.GetQueueReference("commentthumbs");
                    queue2.CreateIfNotExist();

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