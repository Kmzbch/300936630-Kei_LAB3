using System.Threading.Tasks;

namespace DroppingBox.Models
{
    // Interface
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task Create(User user);
        Task Update(User user);
        Task<bool> Exists(string email);
    }

    public class UserRepository : IUserRepository
    {
        private DDBOperations dDBOperations;

        public UserRepository()
        {
            dDBOperations = new DDBOperations();
        }

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

        public async Task Update(User user)
        {
            await dDBOperations.Update(user);
        }

    }
}
