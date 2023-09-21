using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// add user 
namespace webapi.Models
{
    public class Song
    {
        
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Artists { get; set; }
        public string SongUrl { get; set; }

        [JsonIgnore]
        public Album? Album { get; set; }

        public Guid? AlbumId { get; set; }
        public Song()
        {

        }
        public Song(Guid id, string title, string artists, string songUrl, Album album, Guid albumId)
        {
            Id = id;
            Title = title;
            Artists = artists;
            SongUrl = songUrl;
            Album = album;
            AlbumId = albumId;
        }
    }
}
