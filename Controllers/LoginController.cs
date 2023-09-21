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
// could be in user  controller 
namespace webapi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class LoginController : ControllerBase
  {
    public readonly AlbumDbContext _context;
    public LoginController(AlbumDbContext context)
    {
      _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
      try
      {

        if (!ModelState.IsValid)
        {
          return Problem();
        }
        var usersWithMatchingName = _context.Users.Where(u => u.Name == request.UserName).ToList();
        var validUser = usersWithMatchingName.FirstOrDefault(u => BCrypt.Net.BCrypt.Verify(request.Password, u.HashedPassword));
        if (validUser == null)
        {
          return Unauthorized(new { Message = "Invalid credentials." });
        }

        // var cookieOptions = new CookieOptions
        // {
        //   Expires = DateTime.UtcNow.AddDays(100),
        //   HttpOnly = true,
        //   Secure = true,
        //   SameSite = SameSiteMode.Strict
        // };

        List<Claim> claims = new()
        {
          new Claim(ClaimTypes.Name, request.UserName),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "login");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        UserResponse userResponse = new()
        {
          UserName = validUser.Name,
          EmailAddress = validUser.EmailAddress,
        };

        return new JsonResult(userResponse);
      }
      catch (Exception err)
      {
        Console.WriteLine(err.StackTrace);
        return Problem();
      }
    }



  }
}