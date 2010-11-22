﻿using System;
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

        public IEnumerable<EventDataModel> Select(string Artist)
        {
            var results = from c in _ServiceContext.EventTable
                          where c.Artist == Artist
                          select c;

            var query = results.AsTableServiceQuery<EventDataModel>();
            var queryResults = query.Execute();

            return queryResults;
        }

        public void Delete(EventDataModel itemToDelete)
        {
           /* System.Diagnostics.Debug.WriteLine("borro artist = " + itemToDelete.Artist);
            System.Diagnostics.Trace.WriteLine("borro artist = " + itemToDelete.Artist);
            */
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, itemToDelete, "*");
            _ServiceContext.DeleteObject(itemToDelete);
            _ServiceContext.SaveChanges();
        }

        public void Insert(EventDataModel newItem)
        {
            _ServiceContext.AddObject(EventDataServiceContext.EventTableName, newItem);
            _ServiceContext.SaveChanges();
        }

        public void Update(EventDataModel newItem)
        {
            _ServiceContext.AttachTo(EventDataServiceContext.EventTableName, newItem, "*");
            _ServiceContext.UpdateObject(newItem);
            _ServiceContext.SaveChanges();
        }

        
    }
    
}