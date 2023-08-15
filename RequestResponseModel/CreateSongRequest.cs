using System.ComponentModel.DataAnnotations;
using webapi.ValidationAttributes;

namespace webapi.RequestResponseModel
{
    public class CreateSongRequest
    {
        [Required(ErrorMessage="Song Title Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Song File Required")]
        
         [AllowedSongFileExtensions(ErrorMessage = "Invalid song file format.")]
        public IFormFile SongFile { get; set; }

        [Required(ErrorMessage = "Song Artists required")]
        [MinLength(2, ErrorMessage = "Artists Name must be at least 2 characters.")]
        [MaxLength(200, ErrorMessage="Artists Name must be at most 200 characters")]
        public string Artists { get; set; }
    }
}
