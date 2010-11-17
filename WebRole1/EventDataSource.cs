using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace WebRole1
{
    public class EventDataSource
    {
        private EventDataServiceContext _ServiceContext = null;

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

        public void Delete(EventDataModel itemToDelete)
        {
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToDelete, "*");
            _ServiceContext.DeleteObject(itemToDelete);
            _ServiceContext.SaveChanges();
        }

        public void Insert(EventDataModel newItem)
        {
            _ServiceContext.AddObject(EventDataServiceContext.EventTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        
    }
    
}