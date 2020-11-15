using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DroppingBox.Models
{
    public class DDBOperations
    {
        // fields
        AmazonDynamoDBClient _client;
        DynamoDBContext _context;
        Amazon.Runtime.BasicAWSCredentials _credentials;

        // Construtor
        public DDBOperations()
        {
            //
            _credentials = new BasicAWSCredentials(
                "AKIAWY236QUIRI4U3HNE",
                "bu0oSqT27h40WljOOoUDVo7qtRh7PAPN0RYxrRQe");
            _client = new AmazonDynamoDBClient(_credentials, Amazon.RegionEndpoint.USEast1);
            _context = new DynamoDBContext(_client);

            //
            if (this.GetTableStatus("User").Equals("NOTABLE"))
            {
                Task.Run(async () =>
                {
                    await InitialDBCreation();
                }).Wait();
            }
        }

        private async Task InitialDBCreation()
        {
            // create User table
            await this.CreateTable();

            // wait for database being active
            do
            {
                Thread.Sleep(500);
            } while (!this.GetTableStatus("User").Equals("ACTIVE"));

            // inserting initial user
            List<File> files = new List<File>
            {
                        new File
                        {
                            FileId = Guid.NewGuid().ToString(),
                            FileName = "image.jpg",
                            FileLink = null,
                            Comment = "Good"
                        },
                        new File
                        {
                            FileId = Guid.NewGuid().ToString(),
                            FileName = "image2.jpg",
                            FileLink = null,
                            Comment = "Good"
                        },
            };

            User user = new User
            {
                Email = "cdfray@gmail.com",
                FirstName = "Kei",
                LastName = "Mizubuchi",
                Password = BCrypt.Net.BCrypt.HashPassword("password"),
                Files = files
            };

            await this.Insert(user);

            // wait for database being active
            do
            {
                Thread.Sleep(500);
            } while (!this.GetTableStatus("User").Equals("ACTIVE"));

        }

        // table creation
        public async Task CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "User",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition{
                        AttributeName="Email", 
                        AttributeType=ScalarAttributeType.S}
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName="Email",
                        KeyType=KeyType.HASH
                    },
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };
            await _client.CreateTableAsync(request);

        }


        //// table description
        public string GetTableStatus(string tableName)
        {
            var request = new DescribeTableRequest
            {
                TableName = tableName
            };

            try
            {
                TableDescription description = 
                    _client.DescribeTableAsync(request).Result.Table;

                return description.TableStatus;
            }
            catch (Exception e)
            {
                return "NOTABLE";
            }
        }


        // CREATE
        public async Task Insert(User newUser)
        {
            await _context.SaveAsync(newUser);
        }

        // READ
        public async Task<User> Load(string email)
        {
            User loadUser = await _context.LoadAsync<User>(email, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

            return loadUser;
        }

        // UPDATE
        public async Task Update(User user)
        {
            User userRetrieved = await _context.LoadAsync<User>(user.Email);
            if(userRetrieved != null)
            {
                await _context.SaveAsync(user);
            }

        }

    }
}