using System.ComponentModel.DataAnnotations;

namespace webapi.RequestResponseModel
{
  public class AlbumResponse
  {

    public Guid Guid { get; set; }

    public string Title { get; set; }

    public string Artist { get; set; }

    public string AlbumImageUrl { get; set; }

    public List<SongResponse> Songs { get; set; }

    public AlbumResponse(Guid guid, string title, string artist, string albumImageUrl, List<SongResponse> songs)
    {
      Guid = guid;
      Title = title;
      Artist = artist;
      AlbumImageUrl = albumImageUrl;
      Songs = songs;
    }
  }
}