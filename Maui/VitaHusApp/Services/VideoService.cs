
﻿using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;


using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using VitaHusApp.Model;



namespace VitaHusApp.Services
{
    public class VideoService
    {
        private static readonly string[] secrets = { "secret1" };
        private static readonly string Token = JwtBuilder.Create()
            .WithAlgorithm(new JWT.Algorithms.HMACSHA256Algorithm())
            .WithSecret(secrets)
            .AddClaim("sub", "auth0|66c859b3a06686c3440f12aa")
            .Encode();

        public async Task<List<Video>> GetAllVideo()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await httpClient.GetStringAsync("http://localhost:5039/videos/getall"); // Opdater URL efter behov

            return JsonConvert.DeserializeObject<List<Video>>(response); // Deserialiserer en liste af videoer
        }
    }
}