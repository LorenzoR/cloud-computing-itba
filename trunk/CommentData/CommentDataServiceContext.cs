using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;


namespace WebRole1
{

    public class CommentDataServiceContext : TableServiceContext
    {
        public CommentDataServiceContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }

        public const string CommentTableName = "CommentTable";

        public IQueryable<CommentDataModel> CommentTable
        {
            get
            {
                return this.CreateQuery<CommentDataModel>(CommentTableName);
            }
        }

    }
}
