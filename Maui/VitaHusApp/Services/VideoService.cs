using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using VitaHusApp.Model;

namespace VitaHusApp.Services
{
    public static class VideoService
    {
      
        public static async Task<Root> GetVideo(string Id, string title)
        {
            var httpClient = new HttpClient();
             var response = await httpClient.GetStringAsync(""); //her skal url ind
            return JsonConvert.DeserializeObject<Root>(response);
        }
        public static async Task<Root> GetAllVideo(string Id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(""); //her skal url ind
            return JsonConvert.DeserializeObject<Root>(response);
        }
    }
}
