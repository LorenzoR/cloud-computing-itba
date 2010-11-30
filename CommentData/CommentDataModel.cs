using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Web;

namespace WebRole1
{
    public class CommentDataModel
        : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
       /* public CommentDataModel()
        {
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyyhhmm");

            // Row key allows sorting, so we make sure the rows come back in time order.
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }*/
        public CommentDataModel(string partitionKey, string rowKey): base(partitionKey, rowKey)
        {
        }

        /*public CommentDataModel(): this(Guid.NewGuid().ToString(), string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid()))
        {
        }*/

        public CommentDataModel(): this(DateTime.UtcNow.ToString("MMddyyyyhhmmss"), string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid()))
        {
        }

        public string eventID { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
