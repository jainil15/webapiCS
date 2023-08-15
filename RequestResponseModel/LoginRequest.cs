using System.ComponentModel.DataAnnotations;

namespace webapi.RequestResponseModel
{
  public class LoginRequest
  {


    [Required(ErrorMessage = "Username required")]
    [MinLength(2, ErrorMessage = "Username must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Username must be at most 200 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [MaxLength(200, ErrorMessage = "Password must be at most 200 characters")]
    public string Password { get; set; }
  }
}