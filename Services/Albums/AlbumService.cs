using webapi.Data;
using webapi.Models;
using webapi.RequestResponseModel;
// todo
namespace webapi.Services.Albums
{
  public class AlbumService : IAlbumService
  {
    private readonly AlbumDbContext _context;
    public AlbumService(AlbumDbContext context)
    {
      _context = context;
    }
    public async Task<Album> CreateAlbum(Album album)
    {
      await _context.AddAsync(album);
      await _context.SaveChangesAsync();
      return album;
    }

    public bool DeleteAlbum(Guid Id)
    {
      throw new NotImplementedException();
    }
  }
}