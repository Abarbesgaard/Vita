using MongoDB.Bson;
using Vita_WebApi_API.Dto;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Extensions;

public static class VideoExtension
{
   public static VideoDto AsDto(this Video? video)
   {
      return new VideoDto(
         video.Id,
         video.CreatedAt, 
         video.UpdatedAt, 
         video.Title, 
         video.Title, 
         video.Description
         );
   }
}