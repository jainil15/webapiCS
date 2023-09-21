using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using webapi.RequestResponseModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Net.Http;
using Google.Apis.Drive.v3;
using System.IO;
using System.Diagnostics;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Google.Apis.Util.Store;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {

        public readonly AlbumDbContext _context;
        private readonly DriveService _driveService;
        private readonly HttpClient _httpClient;

        public AlbumController(AlbumDbContext context, DriveService driveService, HttpClient httpClient)
        {
            _context = context;
            _driveService = driveService;
            _httpClient = httpClient;
        }


        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetAlbums()
        {
            try
            {
                var albums = await _context.Albums.Include(a => a.Songs).ToListAsync();

                var albumres = albums.Select(album => new Album
                {
                    Id = album.Id,
                    Artist = album.Artist,
                    Title = album.Title,
                    ImageUrl = album.ImageUrl,
                    ReleaseDate = album.ReleaseDate,
                    Songs = album.Songs.Select(song => new Song
                    {
                        Id = song.Id,
                        AlbumId = song.AlbumId,
                        Artists = song.Artists,
                        Title = song.Title,
                        SongUrl = song.SongUrl

                    }).ToList()
                }).ToList();
                var json = JsonSerializer.Serialize(albumres);
                //
                return new JsonResult(albumres);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                return Problem();
            }

        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetAlbum(Guid id)
        {
            try
            {
                var x = await _context.Albums.Include(x => x.Songs).FirstOrDefaultAsync(m => m.Id == id);

                return Ok(x);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                return Problem();
            }
        }

        [HttpGet("image/{imageId}")]
        [Authorize]
        public async Task<IActionResult> GetImage(string imageId)
        {
            try
            {
                var request = _driveService.Files.Get(imageId);
                var stream = new MemoryStream();
                var file = await request.ExecuteAsync();
                var mimeType = file.MimeType;

                request.Download(stream);

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, mimeType);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                return Problem();
            }
        }

        [HttpGet("music/{songId}")]
        [Authorize]
        public async Task<IActionResult> GetSong(string songId)
        {
            try
            {
                var request = _driveService.Files.Get(songId);
                var stream = new MemoryStream();
                var file = await request.ExecuteAsync();
                var mimeType = file.MimeType;

                request.Download(stream);

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, mimeType);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                return Problem(songId, "Song not found", statusCode: 404);
            }
        }

        private string GetFolderId(DriveService service, string folderName)
        {
            // Search for the folder using its name
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Q = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}'";
            listRequest.Fields = "files(id)";

            var result = listRequest.Execute();
            var folder = result.Files.FirstOrDefault();

            if (folder != null)
            {
                return folder.Id;
            }
            else
            {
                Console.WriteLine("Folder not found.");
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAlbum([FromForm] CreateAlbumRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Problem();
                }

                var x = Request.Form;

                //api - key = AIzaSyA2lB5hfgjIW3HT9QjD9IhrSk6qfH56rwk
                //path = ../DriveAuthenctication.json

                string AlbumfolderId = GetFolderId(_driveService, "AlbumImage");
                string SongfolderId = GetFolderId(_driveService, "Songs");

                //Album image id
                string AlbumImageId = UploadFileToDrive(request.AlbumImage, AlbumfolderId);


                Album album = new()
                {
                    Id = Guid.NewGuid(),
                    Artist = request.Artist,
                    ReleaseDate = DateTime.Now,
                    Title = request.Title,
                    ImageUrl = $"https://localhost:7146/api/album/image/{AlbumImageId}",
                    Songs = request.Songs.Select(createSongRequest =>
                    {
                        var songFileId = UploadFileToDrive(createSongRequest.SongFile, SongfolderId);
                        return new Song
                        {

                            Id = Guid.NewGuid(),
                            Title = createSongRequest.Title,
                            Artists = createSongRequest.Artists,
                            SongUrl = $"https://localhost:7146/api/album/music/{songFileId}",

                        };
                    }).ToList(),
                };


                await _context.AddAsync(album);
                await _context.SaveChangesAsync();
                return Content(album.Id.ToString());
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpGet("batch")]
        [Authorize]
        public async Task<IActionResult> GetAlbumsBatch(int skip)
        {
            try
            {
                var batchSize = 10;
                var pageSize = batchSize*skip;
                

                var albums = await _context.Albums.Include(a => a.Songs).Take(pageSize).ToListAsync();

                var albumres = albums.Select(album => new Album
                {
                    Id = album.Id,
                    Artist = album.Artist,
                    Title = album.Title,
                    ImageUrl = album.ImageUrl,
                    ReleaseDate = album.ReleaseDate,
                    Songs = album.Songs.Select(song => new Song
                    {
                        Id = song.Id,
                        AlbumId = song.AlbumId,
                        Artists = song.Artists,
                        Title = song.Title,
                        SongUrl = song.SongUrl

                    }).ToList()
                }).ToList();
                
                //
                return new JsonResult(albumres);
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }


        [HttpDelete("delete/{id:guid}")]
        [Authorize]

        public async Task<IActionResult> DeleteAlbum(Guid Id)
        {
            var album = await _context.Albums.Include(a => a.Songs).FirstOrDefaultAsync(x => x.Id == Id);
            if (album == null)
            {
                return NotFound();
            }
            foreach (var song in album.Songs)
            {
                _context.Songs.Remove(song);
            }
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private string UploadFileToDrive(IFormFile file, string folderId)
        {
            var mimeType = file.ContentType;
            var fileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
                Parents = new List<string> { folderId },
                MimeType = mimeType,
            };
            FilesResource.CreateMediaUpload driveRequest;
            using (var stream = file.OpenReadStream())
            {
                Debug.WriteLine(file.FileName);
                Debug.WriteLine(stream);
                driveRequest = _driveService.Files.Create(fileMetaData, stream, mimeType);
                driveRequest.Fields = "id";
                driveRequest.Upload();
            }

            var uploadedFile = driveRequest.ResponseBody;
            Console.WriteLine("File Uploaded: " + uploadedFile.Id);

            return uploadedFile.Id;

        }


    }
}

