using Amazon.DynamoDBv2.DataModel;
using System;

namespace DroppingBox.Models
{
    [DynamoDBTable("Files")]
    public class Files
    {

        [DynamoDBHashKey]
        public string FileId { get; set; }
        [DynamoDBProperty]
        public string FileName { get; set; }
        [DynamoDBProperty]
        public S3Link FileLink { get; set; }
        [DynamoDBProperty]
        public string Comment { get; set; }
    }
}
