using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using webapi.Data;
using webapi.Models;
using webapi.RequestResponseModel;
// could be in user controller 
namespace webapi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class LogoutController : ControllerBase
  {
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LoggingOut()
    {

      try
      {
        await HttpContext.SignOutAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.StackTrace);
        return Problem();
      }
    }
  }
}