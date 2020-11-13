using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroppingBox.Models
{
    public interface IUserRepository
    {

        List<User> GetAll();
        User GetByEmail(string email);
        void Create(User user);

        bool Exists(string email);
    }

    public class MemoryUserRepository : IUserRepository
    {
        private List<User> users
                    = new List<User>
                    {
                        new User
                        {
                            Email = "cdfray@gmail.com",
                            FirstName = "Kei",
                            LastName = "Mizubuchi",
                            Password = "password"
                        },
                        new User
                        {
                            Email = "aarnest@gmail.com",
                            FirstName = "Alice",
                            LastName = "Arnest",
                            Password = "password"
                        },

                    };

        public List<User> GetAll() { return users; }

        public User GetByEmail(string email) {
            return users.Find(u => u.Email.Equals(email)); }

        public void Create(User user) {
            users.Add(user);
        }

        public bool Exists(string email)
        {
            return users.Exists(u=>u.Email == email);
        }


    }
}
