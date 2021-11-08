using System.Linq;
using CSAuth.Models;

namespace CSAuth.Services
{
  public class UserService : IUserService
  {
    private static readonly User[] Users = new[]
    {
      new User()
      {
        Id = 1,
        Password = "Test User Password",
        Username = "User1"
      }
    };

    public User? GetUser(string username, string password)
    {
      return Users.FirstOrDefault(it => it.Username == username && it.Password == password);
    }
  }
}