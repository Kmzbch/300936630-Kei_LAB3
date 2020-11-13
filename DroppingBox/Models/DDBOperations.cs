using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DroppingBox.Models
{
    public class DDBOperations
    {
        // fields
        AmazonDynamoDBClient client;
        DynamoDBContext context;
        Amazon.Runtime.BasicAWSCredentials credentials;

        // Construtor
        public DDBOperations()
        {
            credentials = new BasicAWSCredentials(
                ConfigurationManager.AppSettings["accessId"],
                ConfigurationManager.AppSettings["secretKey"]);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            context = new DynamoDBContext(client);
        }

        // table creation
        public async Task CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "User",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };
            await client.CreateTableAsync(request);
        }

        //// table description
        //public string GetTableStatus(string tableName)
        //{
        //    var request = new DescribeTableRequest
        //    {
        //        TableName = tableName
        //    };

        //    try
        //    {
        //        var response = client.DescribeTable(request);

        //        TableDescription description = response.Table;

        //        // return CREATING/UPDATING/DELETING/ACTIVE
        //        return description.TableStatus;
        //    }
        //    catch (Exception e)
        //    {
        //        return "NOTABLE";
        //    }
        //}

        //// CREATE
        //public async Task Insert(User newUser)
        //{
        //    await context.SaveAsync(newUser);
        //}

        //// READ
        //public async Task<User> Load(int id)
        //{
        //    User loadUser = await context.LoadAsync<User>(id, new DynamoDBContextConfig
        //    {
        //        ConsistentRead = true
        //    });

        //    return loadUser;
        //}

        //// UPDATE
        //public async Task Update(User user)
        //{
        //    User userRetrieved = await context.LoadAsync<User>(user.Id);
        //    userRetrieved.Books = user.Books;

        //    await context.SaveAsync(userRetrieved);
        //}

    }
}