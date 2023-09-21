using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
  public class User
  {

    [Key]
    public Guid Id { get; set; }


    public string Name { get; set; }


    public string HashedPassword { get; set; }


    public string EmailAddress { get; set; }

    //public Album? Album {get; set; } or add in album class userId
    public User(Guid id, string name, string emailAddress, string hashedPassword)
    {
      Id = id;
      Name = name;
      EmailAddress = emailAddress;
      HashedPassword = hashedPassword;
    }
  }
}