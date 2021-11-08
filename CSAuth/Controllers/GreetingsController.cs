using System.Linq;
using System.Security.Claims;
using CSAuth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSAuth.Controllers
{
  [ApiController]
  [Route("api/hello")]
  [Produces("application/json")]
  [Authorize]
  public class GreetingsController : ControllerBase
  {
    // GET
    [HttpGet]
    public ActionResult<GreetingsDto> Greet()
    {
      return Ok(
        new GreetingsDto
        {
          Hello = "World"
        }
      );
    }

    [HttpGet]
    [Route("by-name")]
    public ActionResult<GreetingsDto> GreetByName()
    {
      var username = HttpContext.User.Claims.Single(it => it.Type == ClaimTypes.Name).Value;

      return Ok(
        new GreetingsDto()
        {
          Hello = username
        }
      );
    }
  }
}