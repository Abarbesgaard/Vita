using MongoDB.Bson;
using Vita_WebApi_API.Dto;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Extensions;

public static class VideoExtension
{
   public static CreateVideoDto AsCreateVideoDto(this Video? video)
   {
      return new CreateVideoDto(
         video.CreatedBy,
         video.UpdatedBy,
         video.CreatedAt, 
         video.UpdatedAt, 
         video.Title, 
         video.Description, 
         video.Url
         );
   }
   public static GetVideoDto AsGetVideoDto(this Video? video)
     {
        return new GetVideoDto(
           video!.Id,
           video.CreatedAt, 
           video.UpdatedAt, 
           video.Title, 
           video.Title, 
           video.Description
           );
     }
}