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
using Google.Apis.Drive.v3.Data;
using Google.Apis.Util.Store;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class SongController : ControllerBase
  {

    public readonly AlbumDbContext _context;
    private readonly DriveService _driveService;
    private readonly HttpClient _httpClient;


    public SongController(AlbumDbContext context, DriveService driveService, HttpClient httpClient)
    {
      _context = context;
      _driveService = driveService;
      _httpClient = httpClient;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> getSong(Guid id)
    {
      if (!ModelState.IsValid)
      {
        return Problem();
      }
      try
      {
        var x = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
        return Ok(x);
      }
      catch (Exception err)
      {
        return Problem();
      }
    }
  }
}

