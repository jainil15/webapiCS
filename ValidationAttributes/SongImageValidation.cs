using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace webapi.ValidationAttributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class AllowedImageFileAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      if (value is IFormFile file)
      {
        var isImage = file.ContentType.StartsWith("image/");

        return isImage;
      }

      return false; // Not a valid IFormFile
    }
  }

  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class AllowedSongFileExtensionsAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      if (value is IFormFile file)
      {
        // Check if the file mimetype falls under the "audio" category
        var isSong = file.ContentType.StartsWith("audio/");

        return isSong;
      }

      return false; // Not a valid IFormFile
    }
  }
}
