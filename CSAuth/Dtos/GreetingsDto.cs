using System.ComponentModel.DataAnnotations;

namespace CSAuth.Dtos
{
  public class GreetingsDto
  {
    [Required] public string Hello { get; set; }
  }
}