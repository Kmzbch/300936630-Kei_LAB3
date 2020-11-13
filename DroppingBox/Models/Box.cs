using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace DroppingBox.Models
{
    [DynamoDBTable("Box")]
    public class Box
    {
        [DynamoDBHashKey]
        public int BoxId { get; set; }
        [DynamoDBProperty]
        public List<string> Files { get; set; }
    }

}