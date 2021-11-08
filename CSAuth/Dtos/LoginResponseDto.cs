using System.ComponentModel.DataAnnotations;

namespace CSAuth.Dtos {
  public class LoginResponseDto {
    [Required] public string Token { get; set; }
  }
}