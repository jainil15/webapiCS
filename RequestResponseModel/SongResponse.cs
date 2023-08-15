using System.ComponentModel.DataAnnotations;

namespace webapi.RequestResponseModel
{
  public class SongResponse
  {
    public Guid Guid { get; set; }
    public string Artists { get; set; }
    public string Title { get; set; }

    public string SongUrl { get; set; }

    public SongResponse(Guid guid, string artists, string title, string songUrl)
    {
      Guid = guid;
      Artists = artists;
      Title = title;
      SongUrl = songUrl;
    }
    
}
}
