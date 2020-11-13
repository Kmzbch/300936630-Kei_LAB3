using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroppingBox.Models
{
    public interface IUserRepository
    {
//        Task<List<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task Create(User user);
        Task<bool> Exists(string email);
    }


    public class UserRepository : IUserRepository
    {
        private DDBOperations dDBOperations;

        public UserRepository()
        {
            dDBOperations = new DDBOperations();
        }

        //public async Task<List<User>> GetAll() { 
        //    return await dDBOperations.get; 
        //}

        public async Task<User> GetByEmail(string email) {
            return await dDBOperations.Load(email); }

        public async Task Create(User user) {
            await dDBOperations.Insert(user);
        }

        public async Task<bool> Exists(string email)
        {
            User user = await dDBOperations.Load(email);

            return user != null;
        }


    }
}
