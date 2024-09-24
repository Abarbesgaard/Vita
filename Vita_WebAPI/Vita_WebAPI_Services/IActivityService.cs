using Vita_WebApi_Shared;
namespace Vita_WebAPI_Services;


public interface IActivityService
{
    Task<IEnumerable<Activity>> GetAll();
  
}