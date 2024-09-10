using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VitaHusApp.Model
{
    public  class Root
    {
        public string Id { get; set; }
        public string? UpdatedBy { get; set; }

        public string? CreatedBy { get; set; }

     
        public DateTimeOffset CreatedAt { get; init; }

        public DateTimeOffset UpdatedAt { get; init; }

        
        public string? Title { get; set; }

        public string? Description { get; set; }

      
    }
}
