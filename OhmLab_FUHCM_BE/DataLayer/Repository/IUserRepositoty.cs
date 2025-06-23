using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IUserRepositoty
    {
        public Task<bool> CreateUser(User user);
        public Task<bool> UpdateUser(User user);
        public Task<bool> DeleteUser(User user);
        public Task<List<User>> GetAllUser();
        public Task<User> GetUserById(Guid id);
        public Task<User> GetUserByEmail(string email);


    }
}
