using System.ComponentModel.DataAnnotations;

namespace app.api.DTOs
{
    public class UserForLogin
    {
        [Required()]
        public string Username { get; set; }

        [Required()]
        public string Password { get; set; }
    }
}