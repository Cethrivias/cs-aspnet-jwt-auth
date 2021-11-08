using CSAuth.Dtos;
using CSAuth.Services;
using CSAuth.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSAuth.Controllers
{
  [ApiController]
  [Route("api/identity")]
  [Produces("application/json")]
  public class IdentityController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IJwtIssuer _jwtIssuer;

    public IdentityController(IUserService userService, IJwtIssuer jwtIssuer)
    {
      _userService = userService;
      _jwtIssuer = jwtIssuer;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
    {
      var user = _userService.GetUser(request.Username, request.Password);
      if (user is null) {
        return new UnauthorizedResult();
      }

      var token = _jwtIssuer.WriteToken(user);

      return Ok(
        new LoginResponseDto()
        {
          Token = token
        }
      );
    }
  }
}