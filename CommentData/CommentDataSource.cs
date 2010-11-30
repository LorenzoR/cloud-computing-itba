using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using System.IO;
using System.Net;

namespace WebRole1
{
    public class CommentDataSource
    {
        private CommentDataServiceContext _ServiceContext = null;
        private static bool storageInitialized = false;
        private static CloudQueueClient queueStorage;
        private static object gate = new Object();

        public CommentDataSource()
        {
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            _ServiceContext = new CommentDataServiceContext(storageAccount.TableEndpoint.ToString(), storageAccount.Credentials);

            // Create the tables
            // In this case, just a single table.  
            storageAccount.CreateCloudTableClient().CreateTableIfNotExist(CommentDataServiceContext.CommentTableName);
        }

        public IEnumerable<CommentDataModel> Select()
        {
            var results = from c in _ServiceContext.CommentTable
                          select c;

            var query = results.AsTableServiceQuery<CommentDataModel>();
            var queryResults = query.Execute();

            return queryResults;
        }

        public IEnumerable<CommentDataModel> Select(string RowKey)
        {
            InitializeStorage();


            var results = from c in _ServiceContext.CommentTable
                          where c.eventID == RowKey
                          select c;

            var query = results.AsTableServiceQuery<CommentDataModel>();
            var queryResults = query.Execute();

            return queryResults;
        }

        public void Delete(CommentDataModel itemToDelete)
        {
            System.Diagnostics.Trace.WriteLine("Borrando rowkey" + itemToDelete.RowKey);
            System.Diagnostics.Trace.WriteLine("Borrando partitionkey" + itemToDelete.PartitionKey);
            _ServiceContext.Detach(itemToDelete);
            _ServiceContext.AttachTo(CommentDataServiceContext.CommentTableName, itemToDelete, "*");
            _ServiceContext.DeleteObject(itemToDelete);
            _ServiceContext.SaveChanges();
            /*var item = (from i in _ServiceContext.CommentTable
                        where i.PartitionKey == itemToDelete.PartitionKey &&
                        i.RowKey == itemToDelete.RowKey
                        select i).Single();
            _ServiceContext.DeleteObject(item);
            _ServiceContext.SaveChanges();*/
        }

        public void Insert(CommentDataModel newItem)
        {
           // System.Diagnostics.Debug.WriteLine("PK: " + newItem.PartitionKey);
           // System.Diagnostics.Trace.WriteLine("aaaaatexto: " + newItem.PartitionKey);
            System.Diagnostics.Trace.TraceInformation("PK: " + newItem.PartitionKey);
            System.Diagnostics.Trace.TraceInformation("RK: " + newItem.RowKey);
            newItem.PartitionKey = newItem.eventID;
            _ServiceContext.AddObject(CommentDataServiceContext.CommentTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        /*public void Insert(string message, string partitionKey)
        {
            System.Diagnostics.Debug.WriteLine("textooooo: " + partitionKey);
            System.Diagnostics.Trace.WriteLine("textoppoppp: " + partitionKey);

        }

        public void Insert(CommentDataModel newItem)
        {
            System.Diagnostics.Debug.WriteLine("aaaaatexto: " + newItem.PartitionKey);
            System.Diagnostics.Trace.WriteLine("aaaaatexto: " + newItem.PartitionKey);
            _ServiceContext.AddObject(CommentDataServiceContext.CommentTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        public void Insert(CommentDataModel newItem, string PartitionKey)
        {
            System.Diagnostics.Debug.WriteLine("texto: " + PartitionKey);
            System.Diagnostics.Trace.WriteLine("texto: " + PartitionKey);
            _ServiceContext.AddObject(CommentDataServiceContext.CommentTableName, newItem);
            _ServiceContext.SaveChanges();
        }
        */
        public void Update(CommentDataModel itemToUpdate)
        {
            _ServiceContext.Detach(itemToUpdate);
            _ServiceContext.AttachTo(CommentDataServiceContext.CommentTableName, itemToUpdate, "*");
            _ServiceContext.UpdateObject(itemToUpdate);
            _ServiceContext.SaveChanges();
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
            var results = from g in _ServiceContext.CommentTable
                          where g.PartitionKey == partitionKey && g.RowKey == rowKey
                          select g;

            var entry = results.FirstOrDefault<CommentDataModel>();
            entry.ThumbnailUrl = thumbUrl;
            _ServiceContext.UpdateObject(entry);
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
                    //queueStorage = storageAccount.CreateCloudQueueClient();
                    //CloudQueue queue = queueStorage.GetQueueReference("commmentsQueue");
                    //queue.CreateIfNotExist();
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
