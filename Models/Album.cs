using System.ComponentModel.DataAnnotations;
// need to add user 
namespace webapi.Models
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Song> Songs { get; set; }
        public string Artist { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Album()
        {

        }
        public Album(Guid id, string title, string imageUrl, List<Song> songs, string artist, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            ImageUrl = imageUrl;
            Songs = songs;
            Artist = artist;
            ReleaseDate = releaseDate;
        }

        // public override string ToString()
        // {
        //     return Id.ToString();
        // }
    }
}
