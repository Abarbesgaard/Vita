using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Vita_WebAPI_Services;
public interface IAuth0Service
{
   Task AddUser(string bearerToken);
   Task GetUser(string id, string bearerToken);
}

public class Auth0Service : IAuth0Service
{
   private readonly HttpClient _client;
   private readonly string? _managementApiToken;
    private readonly ILogger<Auth0Service> _logger;
    public Auth0Service(HttpClient client, IConfiguration configuration, ILogger<Auth0Service> logger)
    {
       _client = client;
       _logger = logger;
       _managementApiToken = configuration["Auth0:ManagementApiToken"];
    }
   public async Task AddUser(string bearerToken)
   {
      var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-dj6iiunlxv3pukjx.us.auth0.com/api/v2/");
      request.Headers.Add("Accept", "application/json");
      request.Headers.Add("Authorization", $"Bearer {bearerToken}");
      var content = new StringContent("{\"email\":\"usertest@example.com\",\"user_metadata\":{},\"blocked\":false,\"app_metadata\":{},\"given_name\":\"string\",\"family_name\":\"string\",\"name\":\"string\",\"nickname\":\"string\",\"connection\":\"Username-Password-Authentication\",\"password\":\"Test1234\",\"username\":\"string\"}", null, "application/json");
      request.Content = content;
      var response = await _client.SendAsync(request);
      response.EnsureSuccessStatusCode();
      await response.Content.ReadAsStringAsync(); 
   }
   public async Task GetUser(string id, string bearerToken)
   {
      _logger.LogInformation($"Fetching user with id: {id}");
   
      var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-dj6iiunlxv3pukjx.us.auth0.com/api/v2/users/{id}");
      request.Headers.Add("Accept", "application/json");
      request.Headers.Add("Authorization", $"Bearer {bearerToken}");
      
      var response = await _client.SendAsync(request);
      if (!response.IsSuccessStatusCode)
      {
         var errorContent = await response.Content.ReadAsStringAsync();
         _logger.LogError($"Error fetching user: {errorContent}");
         throw new HttpRequestException($"Response status code: {response.StatusCode}. Error: {errorContent}");
      }
      _logger.LogInformation("User fetched successfully.");
   }

   public static string JsonStringBuilder()
   {
      var jsonStringBuilder = new StringBuilder();
      const string email = "nybruger@example.com";
      const string givenName = "sdfgsdfg";
      const string familyName = "sdfgsdfg";
      jsonStringBuilder.Append("{");
      jsonStringBuilder.Append($"\"email\":\"{email}\",");
      jsonStringBuilder.Append("\"user_metadata\":{},");
      jsonStringBuilder.Append("\"blocked\":false,");
      jsonStringBuilder.Append("\"app_metadata\":{},");
      jsonStringBuilder.Append($"\"given_name\":\"{givenName}\",");
      jsonStringBuilder.Append($"\"family_name\":\"{familyName}\",");
      jsonStringBuilder.Append("\"name\":\"sdfgdfgs\",");
      jsonStringBuilder.Append("\"nickname\":\"sdfgdfg\",");
      jsonStringBuilder.Append("\"picture\":\"https://t4.ftcdn.net/jpg/04/73/25/49/360_F_473254957_bxG9yf4ly7OBO5I0O5KABlN930GwaMQz.jpg\",");
      jsonStringBuilder.Append("\"connection\":\"Username-Password-Authentication\",");
      jsonStringBuilder.Append("\"password\":\"Test1234\",");
      jsonStringBuilder.Append("\"verify_email\":false,");
      jsonStringBuilder.Append("\"username\":\"dsgfdsfg\"");
      jsonStringBuilder.Append("}");
      return jsonStringBuilder.ToString();

   }
}