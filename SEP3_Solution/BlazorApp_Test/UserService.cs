using Xunit;
using ClassLibrary_SEP3;
using System.Threading.Tasks;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest
{
    public class UserService
    {
        private List<User> users = new List<User>();

        public Task CreateUser(User user)
        {
            users.Add(user);
            return Task.CompletedTask;
        }

        public User Login(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
        
        public bool UserExists(string username)
        {
            return users.Exists(u => u.Username == username);
        }

        public bool passwordExists(string password)
        {
            return users.Exists(user => user.Password == password);
        }
    }
}