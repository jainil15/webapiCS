using Microsoft.CodeAnalysis.Differencing;
using webapi.Data;
using webapi.Models;
using webapi.RequestResponseModel;

namespace webapi.Services.Albums
{
    public interface IAlbumService
    {
        public Task<Album> CreateAlbum(Album album); 
        // public Album Update(UpdateAlbumRequest album);
        public bool DeleteAlbum(Guid Id);
        

    }
}
