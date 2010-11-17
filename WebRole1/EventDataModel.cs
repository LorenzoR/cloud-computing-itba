using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace WebRole1
{
    public class EventDataModel : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {

        public EventDataModel(string partitionKey, string rowKey): base(partitionKey, rowKey)
        {
        }

        public EventDataModel(): this(Guid.NewGuid().ToString(), String.Empty)
        {
        }

        public string Artist { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        //public int VisitCounter { get; set; }
        
    }
}