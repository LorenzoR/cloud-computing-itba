using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace WebRole1
{
    public class EventDataServiceContext : TableServiceContext
    {
        public EventDataServiceContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }

        public const string EventTableName = "EventTable";

        public IQueryable<EventDataModel> EventTable
        {
            get
            {
                return this.CreateQuery<EventDataModel>(EventTableName);
            }
        }

    }
}