using System.ComponentModel.DataAnnotations;
using webapi.ValidationAttributes;

namespace webapi.RequestResponseModel
{
    public class CreateAlbumRequest
    {
        [Required(ErrorMessage = "Album Title Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Album Artist Required")]
        public string Artist { get; set; }
        [Required(ErrorMessage = "Album Image/Logo Required")]

        [AllowedImageFile(ErrorMessage = "Invalid Format for Album Image.")]
        public IFormFile AlbumImage { get; set; }
        [Required(ErrorMessage = "Atleast one song required")]
        public List<CreateSongRequest> Songs { get; set; }
    }
}
