using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VitaHusApp.Model
{
    public class Video
    {
        public string Id { get; set; }  // Dette felt er string ifølge API-svaret
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; } // Dette felt matcher "url" fra API'et
    }

    public class Root
    {
        public List<Video> Videos { get; set; } = new List<Video>(); // Hvis du vil returnere flere videoer
    }
}

