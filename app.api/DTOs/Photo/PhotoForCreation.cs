using Microsoft.AspNetCore.Http;
using System;

namespace app.api.DTOs
{
    public class PhotoForCreation
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Descrition { get; set; }
        public DateTime DateAdded { get; private set; }
        public string PublicId { get; set; }
        public PhotoForCreation() => this.DateAdded = DateTime.Now;
    }
}
