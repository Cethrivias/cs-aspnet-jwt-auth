using CSAuth.Models;

namespace CSAuth.Services
{
  public interface IUserService
  {
    User? GetUser(string username, string password);
  }
}