using CSAuth.Models;

namespace CSAuth.Utils
{
  public interface IJwtIssuer
  {
    string WriteToken(User user);
  }
}