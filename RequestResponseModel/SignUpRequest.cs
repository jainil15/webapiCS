using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.RequestResponseModel
{
  public class SignUpRequest
  {


    [Required(ErrorMessage = "Username required")]
    [MinLength(2, ErrorMessage = "Username must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Username must be at most 200 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [MaxLength(200, ErrorMessage = "Password must be at most 200 characters")]
    public string Password { get; set; }


    [Required(ErrorMessage = "Email address required")]
    [EmailAddress(ErrorMessage = "Proper email address required")]
    [MaxLength(200, ErrorMessage = "Email must be at most 200 characters")]
    public string EmailAddress { get; set; }
  }
}
