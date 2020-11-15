using Amazon.DynamoDBv2.DataModel;

namespace DroppingBox.Models
{
    [DynamoDBTable("File")]
    public class File
    {
        [DynamoDBHashKey]
        public string FileId { get; set; }
        [DynamoDBProperty]
        public string FileName { get; set; }
        [DynamoDBProperty]
        public string FileLink { get; set; }
        [DynamoDBProperty]
        public string Comment { get; set; }

    }
}
