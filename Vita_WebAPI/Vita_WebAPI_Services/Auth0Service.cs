namespace Vita_WebAPI_Services;

public class Auth0Service
{
   public async Task Scoped()
   {
      var client = new HttpClient();
      var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-dj6iiunlxv3pukjx.us.auth0.com/api/v2/users");
      request.Headers.Add("Accept", "application/json");
      request.Headers.Add("Authorization", "Bearer 🔒");
      var content = new StringContent("{\"email\":\"nybruger@example.com\",\"user_metadata\":{},\"blocked\":false,\"app_metadata\":{},\"given_name\":\"sdfgsdfg\",\"family_name\":\"sdfgsdfg\",\"name\":\"sdfgdfgs\",\"nickname\":\"sdfgdfg\",\"picture\":\"https://t4.ftcdn.net/jpg/04/73/25/49/360_F_473254957_bxG9yf4ly7OBO5I0O5KABlN930GwaMQz.jpg\",\"connection\":\"Username-Password-Authentication\",\"password\":\"Test1234\",\"verify_email\":false,\"username\":\"dsgfdsfg\"}", null, "application/json");
      request.Content = content;
      var response = await client.SendAsync(request);
      response.EnsureSuccessStatusCode(); 
      await response.Content.ReadAsStringAsync();    
   }
}