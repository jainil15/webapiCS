using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

class DriverServiceConfigurations
{
  public static DriveService GetDriveService()
  {
    string FilePath = Path.Combine(Directory.GetCurrentDirectory() + "\\DriveAuthentication.json");
    string[] scopes = { DriveService.Scope.Drive };
    UserCredential credential;
    using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
    {
      credential = credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
      {
        ClientId = "817359394196-5ir34dc04i6cu87avd8bu2bjvcgrviaf.apps.googleusercontent.com",
        ClientSecret = "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"
      },
          scopes,
          "user",
          CancellationToken.None,
          new FileDataStore("Drive.Api.Auth.Store")
  ).Result;
    }

    var service = new DriveService(new BaseClientService.Initializer
    {
      HttpClientInitializer = credential,
      ApplicationName = "jainil",
    });
    return service;
  }
}