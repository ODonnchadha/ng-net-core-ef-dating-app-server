using System;

namespace app.api.DTOs
{
    public class PhotoForDetails
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDefault { get; set; }
    }
}
