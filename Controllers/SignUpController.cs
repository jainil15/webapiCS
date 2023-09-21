using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.RequestResponseModel;
// could be in user  controller 
namespace webapi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]

  public class SignUpController : ControllerBase
  {
    private readonly AlbumDbContext _context;
    public SignUpController(AlbumDbContext context)
    {
      _context = context;
    }

    [HttpPost]
    public IActionResult Signup([FromForm] SignUpRequest request)
    {
      if (!ModelState.IsValid)
      {
        Console.WriteLine("Model is not valid!");
        return Problem();
      }
      var existingUser = _context.Users.FirstOrDefault(u => u.EmailAddress == request.EmailAddress);
      if (existingUser != null)
      {
        return Conflict(new { Message = "User with this email already exists." });
      }

      
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

      Models.User newUser = new(
        id: Guid.NewGuid(),
        name: request.UserName,
        emailAddress: request.EmailAddress,
        hashedPassword: hashedPassword
      );

      _context.Users.Add(newUser);
      _context.SaveChanges();

      return Ok(new { Message = "User registered successfully." });
    }
  }
}

